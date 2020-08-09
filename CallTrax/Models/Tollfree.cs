using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class Tollfree
    {
        public long TollfreeId { get; set; }
        public string TollfreeNumber { get; set; }
        public long CallFlowId { get; set; }

        public virtual CallFlow CallFlow { get; set; }
    }
}
