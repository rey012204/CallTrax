using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallFlowStepOption
    {
        public long CallFlowStepOptionId { get; set; }
        public long CallFlowStepId { get; set; }
        public string OptionDescription { get; set; }
        public string OptionValue { get; set; }
        public long NextCallFlowStepId { get; set; }

        public virtual CallFlowStepGather CallFlowStep { get; set; }
    }
}
