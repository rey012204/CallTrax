using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallAction
    {
        public long CallActionId { get; set; }
        public DateTime ActionDateTime { get; set; }
        public long CallId { get; set; }
        public string ActionDescription { get; set; }

        public virtual Call Call { get; set; }
    }
}
