using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.DTO;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtTokenAsync(ApplicationUser user);
    }
}
