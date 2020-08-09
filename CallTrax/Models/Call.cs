using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class Call
    {
        public Call()
        {
            CallGather = new HashSet<CallGather>();
        }

        public long CallId { get; set; }
        public DateTime DateTimeReceived { get; set; }
        public long CallFlowId { get; set; }

        public virtual CallFlow CallFlow { get; set; }
        public virtual ICollection<CallGather> CallGather { get; set; }
    }
}
