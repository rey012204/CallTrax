using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallGather
    {
        public long CallGatherId { get; set; }
        public string CallSid { get; set; }
        public long CallFlowStepId { get; set; }
        public string GatherValue { get; set; }

        public virtual CallFlowStep CallFlowStep { get; set; }
        public virtual Call CallS { get; set; }
    }
}
