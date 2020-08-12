using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallFromPhoneAddress
    {
        public long CallFromPhoneAddressId { get; set; }
        public string CallSid { get; set; }
        public long CallPhoneAddressId { get; set; }

        public virtual CallPhoneAddress CallPhoneAddress { get; set; }
        public virtual Call CallS { get; set; }
    }
}
