using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveGetterService : ILeaveGetterService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveGetterService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<List<LeaveResponse>> GetAllLeave()
        {
            var leaves = await _leaveRepository.GetAllLeave();

            return leaves
                .Select(temp => temp.ToLeaveResponse()).ToList();
        }

        public async Task<List<LeaveResponse>> GetFilteredLeave(string searchBy, string? searchString)
        {
            List<Leave> leaves = searchBy switch
            {
                nameof(LeaveResponse.Status) =>
                await _leaveRepository.GetFilteredLeave(temp => temp.Status.Contains(searchString)),

                nameof(LeaveResponse.LeaveTypeID) =>
                await _leaveRepository.GetFilteredLeave(temp => temp.LeaveType.LeaveTypeName.Contains(searchString)),

                _ => await _leaveRepository.GetAllLeave()
            };

            return leaves.Select(temp => temp.ToLeaveResponse()).ToList();
        }

        public async Task<LeaveResponse?> GetLeaveByLeaveID(Guid? leaveID)
        {
            if (leaveID == null)
            {
                return null;
            }

            Leave? leave = await _leaveRepository.GetLeaveByLeaveID(leaveID.Value);

            if (leave == null)
            {
                return null;
            }

            return leave.ToLeaveResponse();
        }

        public async Task<List<LeaveResponse>> GetLeaveByUserID(Guid? userID)
        {
            var leaves = await _leaveRepository.GetLeaveByUserID(userID.Value);

            return leaves
                .Select(temp => temp.ToLeaveResponse()).ToList();
        }
    }
}
