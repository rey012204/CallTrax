using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallGather
    {
        public long CallGatherId { get; set; }
        public long CallId { get; set; }
        public long CallFlowStepId { get; set; }
        public string GatherValue { get; set; }

        public virtual Call Call { get; set; }
        public virtual CallFlowStep CallFlowStep { get; set; }
    }
}
