using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallFlowStepSay
    {
        public long CallFlowStepId { get; set; }
        public string SayText { get; set; }
        public string Language { get; set; }

        public virtual CallFlowStep CallFlowStep { get; set; }
    }
}
