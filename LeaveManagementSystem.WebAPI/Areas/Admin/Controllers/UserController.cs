using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Enums;

namespace LeaveManagementSystem.WebAPI.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
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
                if (registerDTO.Role == RoleOptions.Admin)
                {
                    // Create 'Admin' role
                    if (await _roleManager.FindByNameAsync(RoleOptions.Admin.ToString()) == null)
                    {
                        ApplicationRole adminRole = new ApplicationRole
                        {
                            Name = RoleOptions.Admin.ToString()
                        };

                        await _roleManager.CreateAsync(adminRole);
                    }

                    // Add the new user into 'Admin' role
                    await _userManager.AddToRoleAsync(user, RoleOptions.Admin.ToString());
                }
                else
                {
                    // Create 'Employee' role
                    if (await _roleManager.FindByNameAsync(RoleOptions.Employee.ToString()) == null)
                    {
                        ApplicationRole employeeRole = new ApplicationRole
                        {
                            Name = RoleOptions.Employee.ToString()
                        };

                        await _roleManager.CreateAsync(employeeRole);
                    }

                    // Add the new user into 'Employee' role
                    await _userManager.AddToRoleAsync(user, RoleOptions.Employee.ToString());
                }

                return Ok(user);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ApplicationUser>> GetUsers()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUserById(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, ApplicationUser updatedUser)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = updatedUser.Email;
            user.Name = updatedUser.Name;
            user.PhoneNumber = updatedUser.PhoneNumber;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(user); 
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Update", error.Description);
                }

                return BadRequest(ModelState);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            IdentityResult result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return NoContent(); // 204 No Content
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Delete", error.Description);
                }

                return BadRequest(ModelState);
            }
        }
    }
}