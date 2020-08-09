using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallFlowStep
    {
        public CallFlowStep()
        {
            CallGather = new HashSet<CallGather>();
        }

        public long CallFlowStepId { get; set; }
        public long CallFlowId { get; set; }
        public short CallFlowStepType { get; set; }
        public bool IsWelcomeStep { get; set; }

        public virtual CallFlow CallFlow { get; set; }
        public virtual CallFlowStepType CallFlowStepTypeNavigation { get; set; }
        public virtual CallFlowStepDial CallFlowStepDial { get; set; }
        public virtual CallFlowStepGather CallFlowStepGather { get; set; }
        public virtual CallFlowStepSay CallFlowStepSay { get; set; }
        public virtual ICollection<CallGather> CallGather { get; set; }
    }
}
