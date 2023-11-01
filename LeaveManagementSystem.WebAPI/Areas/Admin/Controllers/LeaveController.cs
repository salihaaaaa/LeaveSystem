using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;

namespace LeaveManagementSystem.WebAPI.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveAdderService _leaveAdderService;
        private readonly ILeaveGetterService _leaveGetterService;
        private readonly ILeaveUpdaterService _leaveUpdaterService;
        private readonly ILeaveDeleterService _leaveDeleterService;

        public LeaveController(ILeaveAdderService leaveAdderService,
                            ILeaveGetterService leaveGetterService,
                            ILeaveUpdaterService leaveUpdaterService,
                            ILeaveDeleterService leaveDeleterService)
        {
            _leaveAdderService = leaveAdderService;
            _leaveGetterService = leaveGetterService;
            _leaveUpdaterService = leaveUpdaterService;
            _leaveDeleterService = leaveDeleterService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveResponse>>> Get()
        {
            List<LeaveResponse> leaves = await _leaveGetterService.GetAllLeave();

            return Ok(leaves);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveResponse>> Get(Guid? id)
        {
            LeaveResponse? leaveResponse = await _leaveGetterService.GetLeaveByLeaveID(id);

            if (leaveResponse == null)
            {
                return NotFound();
            }

            return Ok(leaveResponse);
        }

        [HttpPost]
        public async Task<ActionResult<LeaveResponse>> Add(LeaveAddRequest leaveAddRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LeaveResponse leaveResponse = await _leaveAdderService.AddLeave(leaveAddRequest);
            return CreatedAtAction("Get", leaveResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid? id, LeaveUpdateRequest leaveUpdateRequest)
        {
            LeaveResponse? leaveResponse = await _leaveGetterService.GetLeaveByLeaveID(id);

            if (leaveResponse == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LeaveResponse updatedLeave = await _leaveUpdaterService.UpdateLeave(leaveUpdateRequest);
            return Ok(updatedLeave);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            LeaveResponse? leaveResponse = await _leaveGetterService.GetLeaveByLeaveID(id);

            if (leaveResponse == null)
            {
                return NotFound();
            }

            await _leaveDeleterService.DeleteLeave(leaveResponse.LeaveID);
            return NoContent();
        }
    }
}
