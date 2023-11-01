using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveTypeUpdaterService
    {
        Task<LeaveTypeResponse> UpdateLeaveType(LeaveTypeUpdateRequest? leaveTypeUpdateRequest);
    }
}
