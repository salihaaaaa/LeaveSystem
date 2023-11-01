using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Enums;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveSorterService
    {
        Task<List<LeaveResponse>> GetSortedLeave(List<LeaveResponse> allLeaves, string sortBy, SortOrderOptions sortOrder);
    }
}
