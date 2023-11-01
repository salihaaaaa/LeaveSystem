using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Enums;
using LeaveManagementSystem.Core.ServiceContracts;

namespace LeaveManagementSystem.Core.Services
{
    public class LeaveSorterService : ILeaveSorterService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveSorterService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<List<LeaveResponse>> GetSortedLeave(List<LeaveResponse> allLeaves, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allLeaves;
            }

            List<LeaveResponse> sortedLeaves = (sortBy, sortOrder) switch
            {
                (nameof(LeaveResponse.Name), SortOrderOptions.ASC) =>
                   allLeaves.OrderBy(temp => temp.Name,
                   StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(LeaveResponse.Name), SortOrderOptions.DESC) =>
                    allLeaves.OrderByDescending(temp => temp.Name,
                    StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(LeaveResponse.Status), SortOrderOptions.ASC) =>
                    allLeaves.OrderBy(temp => temp.Status,
                    StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(LeaveResponse.Status), SortOrderOptions.DESC) =>
                    allLeaves.OrderByDescending(temp => temp.Status,
                    StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(LeaveResponse.LeaveType), SortOrderOptions.ASC) =>
                    allLeaves.OrderBy(temp => temp.LeaveType,
                    StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(LeaveResponse.LeaveType), SortOrderOptions.DESC) =>
                    allLeaves.OrderByDescending(temp => temp.LeaveType,
                    StringComparer.OrdinalIgnoreCase).ToList(),

                _ => allLeaves
            };

            return sortedLeaves;
        }
    }
}
