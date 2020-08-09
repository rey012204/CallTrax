using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallFlowStepGather
    {
        public CallFlowStepGather()
        {
            CallFlowStepOption = new HashSet<CallFlowStepOption>();
        }

        public long CallFlowStepId { get; set; }
        public string GatherName { get; set; }
        public short Attempts { get; set; }

        public virtual CallFlowStep CallFlowStep { get; set; }
        public virtual CallFlowStepGatherRetry CallFlowStepGatherRetry { get; set; }
        public virtual ICollection<CallFlowStepOption> CallFlowStepOption { get; set; }
    }
}
