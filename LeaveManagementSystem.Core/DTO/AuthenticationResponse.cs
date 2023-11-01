﻿namespace LeaveManagementSystem.Core.DTO
{
    public class AuthenticationResponse
    {
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; } 
        public string? Role { get; set; }
    }
}
