using System;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveDeleterService
    {
        Task<bool> DeleteLeave(Guid? leaveID);
    }
}
