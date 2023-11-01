using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Infrastructure.Repositories
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<LeaveType> AddLeaveType(LeaveType leaveType)
        {
            _db.LeaveTypes.Add(leaveType);
            await _db.SaveChangesAsync();
            return leaveType;
        }

        public async Task<bool> DeleteLeaveTypeByLeaveTypeID(Guid leaveTypeID)
        {
            _db.LeaveTypes
                .RemoveRange(_db.LeaveTypes
                .Where(temp => temp.LeaveTypeID == leaveTypeID));

            int rowsDeleted = await _db
                .SaveChangesAsync();

            return rowsDeleted > 0;
        }

        public async Task<LeaveType?> GetLeaveTypeByLeaveTypeID(Guid leaveTypeID)
        {
            return await _db.LeaveTypes
                .FirstOrDefaultAsync(temp => temp.LeaveTypeID == leaveTypeID);
        }

        public async Task<List<LeaveType>> GetAllLeaveType()
        {
            return await _db.LeaveTypes.ToListAsync();
        }

        public async Task<LeaveType> UpdateLeaveType(LeaveType leaveType)
        {
            LeaveType? matchingLeaveType = await _db.LeaveTypes
                .FirstOrDefaultAsync(temp => temp.LeaveTypeID == leaveType.LeaveTypeID);

            if (matchingLeaveType == null)
            {
                return leaveType;
            }

            matchingLeaveType.LeaveTypeName = leaveType.LeaveTypeName;

            int countUpdated = await _db
                .SaveChangesAsync();

            return matchingLeaveType;
        }

        public async Task<LeaveType?> GetLeaveTypeByLeaveTypeName(string leaveTypeName)
        {
            return await _db.LeaveTypes
                .FirstOrDefaultAsync(temp => temp.LeaveTypeName == leaveTypeName);
        }
    }
}
