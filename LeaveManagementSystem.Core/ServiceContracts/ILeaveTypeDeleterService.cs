using System;

namespace LeaveManagementSystem.Core.ServiceContracts
{
    public interface ILeaveTypeDeleterService
    {
        Task<bool> DeleteLeaveType(Guid? leaveTypeID);
    }
}
