using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveAdderService
    {
        Task<LeaveResponse> AddLeave(LeaveAddRequest? leaveAddRequest);
    }
}
