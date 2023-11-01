using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Enums;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;

        public AccountController(UserManager<ApplicationUser> userManager, 
                    SignInManager<ApplicationUser> signInManager, 
                    RoleManager<ApplicationRole> roleManager,
                    IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                Name = registerDTO.Name,
                UserName = registerDTO.Email,
                PhoneNumber = registerDTO.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                // Check user role
                if (registerDTO.Role == RoleOptions.Employee)
                {
                    // Create 'Employee' role
                    if (await _roleManager.FindByNameAsync(RoleOptions.Employee.ToString()) == null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = RoleOptions.Employee.ToString()
                        };

                        await _roleManager.CreateAsync(applicationRole);
                    }

                    // Add the new user into 'Employee' role
                    await _userManager.AddToRoleAsync(user, RoleOptions.Employee.ToString());
                }

                // Sign in
                await _signInManager.SignInAsync(user, false);

                var authenticationResponse = _jwtService.CreateJwtTokenAsync(user);

                return Ok(authenticationResponse);
            }
            else
            {
                var errors = result.Errors.Select(error => error.Description);
                return BadRequest(errors);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);

            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user != null)
                {
                    // If user is 'Admin'
                    if (await _userManager.IsInRoleAsync(user, RoleOptions.Admin.ToString()))
                    {
                        // Sign in
                        await _signInManager.SignInAsync(user, false);
                        var authenticationResponse = _jwtService.CreateJwtTokenAsync(user);
                        return Ok(authenticationResponse);
                    }
                    // If user is 'Employee'
                    else
                    {
                        // Sign in
                        await _signInManager.SignInAsync(user, false);
                        var authenticationResponse = _jwtService.CreateJwtTokenAsync(user);
                        return Ok(authenticationResponse);
                    }
                }
                return BadRequest("User not found");
            }
            return BadRequest("Invalid email or password");
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        [HttpGet("isEmailAlreadyRegistered")]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Ok(true); // Email not exist in the database
            }
            else
            {
                return Ok(false); // Email already exists in the database
            }
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            return Ok(user);
        }
    }
}
