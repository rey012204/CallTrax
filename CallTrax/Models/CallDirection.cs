using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallDirection
    {
        public CallDirection()
        {
            Call = new HashSet<Call>();
        }

        public short CallDirectionId { get; set; }
        public string CallDirectionName { get; set; }

        public virtual ICollection<Call> Call { get; set; }
    }
}
