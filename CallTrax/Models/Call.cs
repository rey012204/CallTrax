using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class Call
    {
        public Call()
        {
            CallAction = new HashSet<CallAction>();
            CallFromPhoneAddress = new HashSet<CallFromPhoneAddress>();
            CallGather = new HashSet<CallGather>();
            CallToPhoneAddress = new HashSet<CallToPhoneAddress>();
        }

        public string CallSid { get; set; }
        public DateTime DateTimeReceived { get; set; }
        public long CallFlowId { get; set; }
        public string AccountSid { get; set; }
        public string FromPhoneNumber { get; set; }
        public string ToPhoneNumber { get; set; }
        public short CallStatus { get; set; }
        public short Direction { get; set; }
        public string ApiVersion { get; set; }
        public string ForwardedFrom { get; set; }
        public string CallerName { get; set; }
        public string ParentCallSid { get; set; }

        public virtual CallDirection DirectionNavigation { get; set; }
        public virtual ICollection<CallAction> CallAction { get; set; }
        public virtual ICollection<CallFromPhoneAddress> CallFromPhoneAddress { get; set; }
        public virtual ICollection<CallGather> CallGather { get; set; }
        public virtual ICollection<CallToPhoneAddress> CallToPhoneAddress { get; set; }
    }
}
