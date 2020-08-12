using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallAction
    {
        public long CallActionId { get; set; }
        public DateTime ActionDateTime { get; set; }
        public string CallSid { get; set; }
        public string ActionDescription { get; set; }

        public virtual Call CallS { get; set; }
    }
}
