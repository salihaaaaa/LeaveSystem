using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveDeleterService : ILeaveDeleterService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveDeleterService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<bool> DeleteLeave(Guid? leaveID)
        {
            if (leaveID == null)
            {
                throw new ArgumentNullException(nameof(leaveID));
            }

            Leave? leave = await _leaveRepository.GetLeaveByLeaveID(leaveID.Value);

            if (leave == null)
            {
                return false;
            }

            await _leaveRepository.DeleteLeaveByLeaveID(leaveID.Value);
            return true;
        }
    }
}
