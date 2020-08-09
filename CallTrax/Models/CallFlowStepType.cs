using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallFlowStepType
    {
        public CallFlowStepType()
        {
            CallFlowStep = new HashSet<CallFlowStep>();
        }

        public short CallFlowStepTypeId { get; set; }
        public string StepTypeName { get; set; }

        public virtual ICollection<CallFlowStep> CallFlowStep { get; set; }
    }
}
