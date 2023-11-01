using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Helpers;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveAdderService : ILeaveAdderService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveAdderService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<LeaveResponse> AddLeave(LeaveAddRequest? leaveAddRequest)
        {
            //Validation
            if (leaveAddRequest == null)
            {
                throw new ArgumentNullException(nameof(leaveAddRequest));
            }

            ValidationHelper.ModelValidation(leaveAddRequest);

            Leave leave = leaveAddRequest.ToLeave();
            leave.LeaveID = Guid.NewGuid();
            await _leaveRepository.AddLeave(leave);
            return leave.ToLeaveResponse();
        }
    }
}
