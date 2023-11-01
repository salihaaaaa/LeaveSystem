using System.Linq.Expressions;

using LeaveManagementSystem.Core.Domain.Entities;

namespace LeaveManagementSystem.Core.Domain.RepositoryContracts
{
    public interface ILeaveRepository
    {
        Task<Leave> AddLeave(Leave leave);

        Task<List<Leave>> GetAllLeave();

        Task<List<Leave>> GetFilteredLeave(Expression<Func<Leave, bool>> predicate);

        Task<Leave?> GetLeaveByLeaveID(Guid leave);

        Task<List<Leave>> GetLeaveByUserID(Guid userID);

        Task<bool> DeleteLeaveByLeaveID(Guid leaveID);

        Task<Leave> UpdateLeave(Leave leave);
    }
}
