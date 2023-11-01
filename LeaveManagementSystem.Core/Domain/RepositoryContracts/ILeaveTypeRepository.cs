using LeaveManagementSystem.Core.Domain.Entities;

namespace LeaveManagementSystem.Core.Domain.RepositoryContracts
{
    public interface ILeaveTypeRepository
    {
        Task<LeaveType> AddLeaveType(LeaveType leaveType);

        Task<List<LeaveType>> GetAllLeaveType();

        Task<LeaveType?> GetLeaveTypeByLeaveTypeID(Guid leaveTypeID);

        Task<LeaveType?> GetLeaveTypeByLeaveTypeName(string leaveTypeName);

        Task<bool> DeleteLeaveTypeByLeaveTypeID(Guid leaveTypeID);

        Task<LeaveType> UpdateLeaveType(LeaveType leaveType);
    }
}
