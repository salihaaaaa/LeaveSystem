using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Helpers;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveTypeAdderService : ILeaveTypeAdderService
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public LeaveTypeAdderService(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<LeaveTypeResponse> AddLeaveType(LeaveTypeAddRequest? leaveTypeAddRequest)
        {
            //Validation
            if (leaveTypeAddRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveTypeAddRequest));
            }

            ValidationHelper.ModelValidation(leaveTypeAddRequest);

            LeaveType leaveType = leaveTypeAddRequest.ToLeaveType();
            leaveType.LeaveTypeID = Guid.NewGuid();
            await _leaveTypeRepository.AddLeaveType(leaveType);
            return leaveType.ToLeaveTypeResponse();
        }
    }
}