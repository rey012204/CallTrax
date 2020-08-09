using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallFlow
    {
        public CallFlow()
        {
            Call = new HashSet<Call>();
            CallFlowStep = new HashSet<CallFlowStep>();
            Tollfree = new HashSet<Tollfree>();
        }

        public long CallFlowId { get; set; }
        public long ClientId { get; set; }
        public string FlowName { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<Call> Call { get; set; }
        public virtual ICollection<CallFlowStep> CallFlowStep { get; set; }
        public virtual ICollection<Tollfree> Tollfree { get; set; }
    }
}
