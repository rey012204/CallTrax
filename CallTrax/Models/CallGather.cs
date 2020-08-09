using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallGather
    {
        public long CallGatherId { get; set; }
        public long CallId { get; set; }
        public string GatherName { get; set; }
        public string GatherValue { get; set; }
        public string GatherValueName { get; set; }

        public virtual Call Call { get; set; }
    }
}
