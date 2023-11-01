using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveTypeGetterService : ILeaveTypeGetterService
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public LeaveTypeGetterService(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<List<LeaveTypeResponse>> GetAllLeaveType()
        {
            var leaveTypes = await _leaveTypeRepository.GetAllLeaveType();

            return leaveTypes.Select(temp => temp.ToLeaveTypeResponse()).ToList();
        }

        public async Task<LeaveTypeResponse?> GetLeaveTypeByLeaveTypeID(Guid? leaveTypeID)
        {
            if (leaveTypeID == null)
            {
                return null;
            }

            LeaveType? leaveType = await _leaveTypeRepository.GetLeaveTypeByLeaveTypeID(leaveTypeID.Value);

            if (leaveType == null)
            {
                return null;
            }

            return leaveType.ToLeaveTypeResponse();
        }
    }
}
