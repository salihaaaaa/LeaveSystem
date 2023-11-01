using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;

namespace LeaveManagementSystem.WebAPI.Areas.Employee.Controllers
{
    [Route("api/employee/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class LeaveController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILeaveTypeGetterService _leaveTypeGetterService;
        private readonly ILeaveAdderService _leaveAdderService;
        private readonly ILeaveGetterService _leaveGetterService;

        public LeaveController(UserManager<ApplicationUser> userManager,
            ILeaveTypeGetterService leaveTypeGetterService,
            ILeaveAdderService leaveAdderService,
            ILeaveGetterService leaveGetterService)
        {
            _userManager = userManager;
            _leaveTypeGetterService = leaveTypeGetterService;
            _leaveAdderService = leaveAdderService;
            _leaveGetterService = leaveGetterService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated or token is invalid.");
            }

            List<LeaveResponse> leaves = await _leaveGetterService.GetLeaveByUserID(currentUser.Id);
            return Ok(leaves);
        }


        [HttpPost]
        public async Task<IActionResult> Add(LeaveAddRequest leaveAddRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ApplicationUser currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized("User is not authenticated or token is invalid.");
            }

            leaveAddRequest.UserID = currentUser.Id;

            LeaveResponse leaveResponse = await _leaveAdderService.AddLeave(leaveAddRequest);
            return CreatedAtAction("Get", leaveResponse);
        }
    }
}
