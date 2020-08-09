using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallFlowStepGatherRetry
    {
        public long CallFlowStepId { get; set; }
        public long RetryCallFlowStepId { get; set; }
        public long FailCallFlowStepId { get; set; }

        public virtual CallFlowStepGather CallFlowStep { get; set; }
    }
}
