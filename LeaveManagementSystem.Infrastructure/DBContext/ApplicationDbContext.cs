using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LeaveType>().ToTable("LeaveTypes");
            modelBuilder.Entity<Leave>().ToTable("Leaves");

            //Seed data to leave type
            string leaveTypeJson = File.ReadAllText("leaveType.json");
            List<LeaveType> leaveTypes = System.Text.Json.JsonSerializer.Deserialize<List<LeaveType>>(leaveTypeJson);

            foreach (LeaveType leaveType in leaveTypes)
            {
                modelBuilder.Entity<LeaveType>().HasData(leaveType);
            }
        }
    }
}
