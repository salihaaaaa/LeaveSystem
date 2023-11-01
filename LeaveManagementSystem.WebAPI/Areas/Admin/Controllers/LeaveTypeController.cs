using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.WebAPI.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ILeaveTypeAdderService _leaveTypeAdderService;
        private readonly ILeaveTypeGetterService _leaveTypeGetterService;
        private readonly ILeaveTypeUpdaterService _leaveTypeUpdaterService;
        private readonly ILeaveTypeDeleterService _leaveTypeDeleterService;

        public LeaveTypeController(ILeaveTypeAdderService leaveTypeAdderService,
            ILeaveTypeGetterService leaveTypeGetterService,
            ILeaveTypeUpdaterService leaveTypeUpdaterService,
            ILeaveTypeDeleterService leaveTypeDeleterService)
        {
            _leaveTypeAdderService = leaveTypeAdderService;
            _leaveTypeGetterService = leaveTypeGetterService;
            _leaveTypeUpdaterService = leaveTypeUpdaterService;
            _leaveTypeDeleterService = leaveTypeDeleterService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<IEnumerable<LeaveTypeResponse>>> Get()
        {
            List<LeaveTypeResponse> leaveTypes = await _leaveTypeGetterService.GetAllLeaveType();
            return Ok(leaveTypes);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<LeaveTypeResponse>> Add(LeaveTypeAddRequest leaveTypeAddRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LeaveTypeResponse leaveTypeResponse = await _leaveTypeAdderService.AddLeaveType(leaveTypeAddRequest);
            return CreatedAtAction("Get", leaveTypeResponse);
        }

        [HttpGet("{leaveTypeID}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<LeaveTypeResponse>> Get(Guid? leaveTypeID)
        {
            LeaveTypeResponse? leaveTypeResponse = await _leaveTypeGetterService.GetLeaveTypeByLeaveTypeID(leaveTypeID);

            if (leaveTypeResponse == null)
            {
                return NotFound();
            }

            return Ok(leaveTypeResponse);
        }

        [HttpPut("{leaveTypeID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? leaveTypeID, LeaveTypeUpdateRequest leaveTypeUpdateRequest)
        {
            LeaveTypeResponse? leaveTypeResponse = await _leaveTypeGetterService.GetLeaveTypeByLeaveTypeID(leaveTypeID);

            if (leaveTypeResponse == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LeaveTypeResponse updateLeaveType = await _leaveTypeUpdaterService.UpdateLeaveType(leaveTypeUpdateRequest);
            return Ok(updateLeaveType);
        }

        [HttpDelete("{leaveTypeID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? leaveTypeID)
        {
            LeaveTypeResponse? leaveTypeResponse = await _leaveTypeGetterService.GetLeaveTypeByLeaveTypeID(leaveTypeID);

            if (leaveTypeResponse == null)
            {
                return NotFound();
            }

            await _leaveTypeDeleterService.DeleteLeaveType(leaveTypeResponse.LeaveTypeID);
            return NoContent();
        }
    }
}
