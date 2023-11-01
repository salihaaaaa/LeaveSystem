using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveGetterService
    {
        Task<List<LeaveResponse>> GetAllLeave();

        Task<List<LeaveResponse>> GetFilteredLeave(string searchBy, string? searchString);

        Task<LeaveResponse?> GetLeaveByLeaveID(Guid? leaveID);

        Task<List<LeaveResponse>> GetLeaveByUserID(Guid? userID);
    }
}
