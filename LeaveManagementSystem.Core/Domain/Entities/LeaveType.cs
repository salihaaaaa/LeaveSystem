﻿using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Core.Domain.Entities
{
    public class LeaveType
    {
        [Key]
        public Guid LeaveTypeID { get; set; }

        [StringLength(50)]
        public string? LeaveTypeName { get; set; }
    }
}
