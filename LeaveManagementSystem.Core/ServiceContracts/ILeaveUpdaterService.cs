using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveUpdaterService
    {
        Task<LeaveResponse> UpdateLeave(LeaveUpdateRequest? leaveUpdateRequest);
    }
}
