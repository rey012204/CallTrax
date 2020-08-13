using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallGatherAttempt
    {
        public long CallGatherAttemptId { get; set; }
        public string CallSid { get; set; }
        public long CallFlowStepId { get; set; }
        public short Attempts { get; set; }

        public virtual CallFlowStepGather CallFlowStep { get; set; }
        public virtual Call CallS { get; set; }
    }
}
