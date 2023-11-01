using System.ComponentModel.DataAnnotations;

using LeaveManagementSystem.Core.Domain.Entities;

namespace LeaveManagementSystem.Core.DTO
{
    public class LeaveTypeUpdateRequest
    {
        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Leave Type ID")]
        public Guid LeaveTypeID { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Leave Type")]
        public string? LeaveTypeName { get; set; }

        public LeaveType ToLeaveType()
        {
            return new LeaveType()
            {
                LeaveTypeID = LeaveTypeID,
                LeaveTypeName = LeaveTypeName
            };
        }
    }
}
