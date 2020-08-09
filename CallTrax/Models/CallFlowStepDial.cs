using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallFlowStepDial
    {
        public long CallFlowStepId { get; set; }
        public string DialPhoneNumber { get; set; }

        public virtual CallFlowStep CallFlowStep { get; set; }
    }
}
