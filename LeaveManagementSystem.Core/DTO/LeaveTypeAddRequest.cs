﻿using System.ComponentModel.DataAnnotations;

using LeaveManagementSystem.Core.Domain.Entities;

namespace LeaveManagementSystem.Core.DTO
{
    public class LeaveTypeAddRequest
    {
        [Required(ErrorMessage = "{0} cannot be blank")]
        [Display(Name = "Leave Type")]
        public string? LeaveTypeName { get; set; }

        public LeaveType ToLeaveType()
        {
            return new LeaveType()
            {
                LeaveTypeName = LeaveTypeName
            };
        }
    }
}
