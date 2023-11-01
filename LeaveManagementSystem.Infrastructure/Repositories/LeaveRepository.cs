using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LeaveManagementSystem.Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Leave> AddLeave(Leave leave)
        {
            _db.Leaves.Add(leave);
            await _db.SaveChangesAsync();
            return leave;
        }

        public async Task<bool> DeleteLeaveByLeaveID(Guid leaveID)
        {
            _db.Leaves
                .RemoveRange(_db.Leaves
                .Where(temp => temp.LeaveID == leaveID));

            int rowsDeleted = await _db.SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<List<Leave>> GetAllLeave()
        {
            return await _db.Leaves
                .Include("User")
                .Include("LeaveType")
                .ToListAsync();
        }

        public async Task<List<Leave>> GetFilteredLeave(Expression<Func<Leave, bool>> predicate)
        {
            return await _db.Leaves
                .Include("User")
                .Include("LeaveType")
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Leave?> GetLeaveByLeaveID(Guid leaveID)
        {
            return await _db.Leaves
                .Include("User")
                .Include("LeaveType")
                .FirstOrDefaultAsync(temp => temp.LeaveID == leaveID);
        }

        public async Task<List<Leave>> GetLeaveByUserID(Guid userID)
        {
            return await _db.Leaves
                .Include("LeaveType")
                .Where(temp => temp.UserID == userID)
                .ToListAsync();
        }

        public async Task<Leave> UpdateLeave(Leave leave)
        {
            Leave? matchingLeave = await _db.Leaves
                .FirstOrDefaultAsync(temp => temp.LeaveID == leave.LeaveID);

            if (matchingLeave == null)
            {
                return leave;
            }

            matchingLeave.LeaveType = leave.LeaveType;
            matchingLeave.StartDate = leave.StartDate;
            matchingLeave.EndDate = leave.EndDate;
            matchingLeave.Reason = leave.Reason;
            matchingLeave.Status = leave.Status;

            int countUpdated = await _db.SaveChangesAsync();

            return matchingLeave;
        }
    }
}
