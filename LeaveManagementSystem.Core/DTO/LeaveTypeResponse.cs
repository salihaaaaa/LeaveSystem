using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.DTO
{
    public class LeaveTypeResponse
    {
        public Guid LeaveTypeID { get; set; }
        public string? LeaveTypeName { get; set; }

        public override string ToString()
        {
            return $"Leave Type ID: {LeaveTypeID}, Leave Type Name: {LeaveTypeName}";
        }

        public LeaveTypeUpdateRequest ToLeaveTypeUpdateRequest()
        {
            return new LeaveTypeUpdateRequest()
            {
                LeaveTypeID = LeaveTypeID,
                LeaveTypeName = LeaveTypeName
            };
        }
    }
}

public static class LeaveTypeExtensions
{
    public static LeaveTypeResponse ToLeaveTypeResponse(this LeaveType leaveType)
    {
        return new LeaveTypeResponse()
        {
            LeaveTypeID = leaveType.LeaveTypeID,
            LeaveTypeName = leaveType.LeaveTypeName
        };
    }
}
