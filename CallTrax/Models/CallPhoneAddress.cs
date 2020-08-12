using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallPhoneAddress
    {
        public CallPhoneAddress()
        {
            CallFromPhoneAddress = new HashSet<CallFromPhoneAddress>();
            CallToPhoneAddress = new HashSet<CallToPhoneAddress>();
        }

        public long CallPhoneAddressId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        public virtual ICollection<CallFromPhoneAddress> CallFromPhoneAddress { get; set; }
        public virtual ICollection<CallToPhoneAddress> CallToPhoneAddress { get; set; }
    }
}
