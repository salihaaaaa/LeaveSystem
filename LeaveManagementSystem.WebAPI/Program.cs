using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.ServiceContracts;
using LeaveManagementSystem.Core.Services;
using LeaveManagementSystem.Infrastructure.DBContext;
using LeaveManagementSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();

builder.Services.AddScoped<ILeaveTypeAdderService, LeaveTypeAdderService>();
builder.Services.AddScoped<ILeaveTypeGetterService, LeaveTypeGetterService>();
builder.Services.AddScoped<ILeaveTypeUpdaterService, LeaveTypeUpdaterService>();
builder.Services.AddScoped<ILeaveTypeDeleterService, LeaveTypeDeleterService>();
builder.Services.AddScoped<ILeaveAdderService, LeaveAdderService>();
builder.Services.AddScoped<ILeaveGetterService, LeaveGetterService>();
builder.Services.AddScoped<ILeaveSorterService, LeaveSorterService>();
builder.Services.AddScoped<ILeaveUpdaterService, LeaveUpdaterService>();
builder.Services.AddScoped<ILeaveDeleterService, LeaveDeleterService>();

builder.Services.AddTransient<IJwtService, JwtService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Enable Identity in this project
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredUniqueChars = 5; //Non repeated character
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        //builder.WithOrigins("http://localhost:5800")
        builder.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting(); //Identifying action method based route
app.UseCors();
app.UseAuthentication(); //Reading Identity cookie (login or not)
app.UseAuthorization(); //Validate access permission of the user
app.MapControllers();

app.Run();
