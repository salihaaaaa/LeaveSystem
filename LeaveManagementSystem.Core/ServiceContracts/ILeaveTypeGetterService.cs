using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveTypeGetterService
    {
        Task<List<LeaveTypeResponse>> GetAllLeaveType();

        Task<LeaveTypeResponse?> GetLeaveTypeByLeaveTypeID(Guid? leaveTypeID);
    }
}
