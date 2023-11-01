using Microsoft.AspNetCore.Identity;

using LeaveManagementSystem.Core.Domain.Entities;

namespace LeaveManagementSystem.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? Name { get; set; }

        public virtual ICollection<Leave>? Leaves { get; set; }
    }
}
