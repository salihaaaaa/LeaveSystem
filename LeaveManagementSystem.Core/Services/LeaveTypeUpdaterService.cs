using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Helpers;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveTypeUpdaterService : ILeaveTypeUpdaterService
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public LeaveTypeUpdaterService(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<LeaveTypeResponse> UpdateLeaveType(LeaveTypeUpdateRequest? leaveTypeUpdateRequest)
        {
            //Validation
            if (leaveTypeUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveTypeUpdateRequest));
            }

            ValidationHelper.ModelValidation(leaveTypeUpdateRequest);

            LeaveType? matchingLeaveType = await _leaveTypeRepository.GetLeaveTypeByLeaveTypeID(leaveTypeUpdateRequest.LeaveTypeID);

            if (matchingLeaveType == null)
            {
                throw new ArgumentException("Given leave type id doesn't exist");
            }

            //Update all details
            matchingLeaveType.LeaveTypeName = leaveTypeUpdateRequest.LeaveTypeName;

            await _leaveTypeRepository.UpdateLeaveType(matchingLeaveType);
            return matchingLeaveType.ToLeaveTypeResponse();
        }
    }
}
