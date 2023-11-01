using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveTypeDeleterService : ILeaveTypeDeleterService
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public LeaveTypeDeleterService(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<bool> DeleteLeaveType(Guid? leaveTypeID)
        {
            if (leaveTypeID == null)
            {
                throw new ArgumentNullException(nameof(leaveTypeID));
            }

            LeaveType? leaveType = await _leaveTypeRepository.GetLeaveTypeByLeaveTypeID(leaveTypeID.Value);

            if (leaveType == null)
            {
                return false;
            }

            await _leaveTypeRepository.DeleteLeaveTypeByLeaveTypeID(leaveTypeID.Value);
            return true;
        }
    }
}
