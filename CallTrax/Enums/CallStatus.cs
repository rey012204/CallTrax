using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallTrax.Enums
{
    public enum CallStatus
    {
        Queued = 1,
        Ringing,
        InProgress,
        Completed,
        Busy,
        Failed,
        NoAnswer,
        Cancelled
    }
}
