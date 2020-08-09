using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class ClientContact
    {
        public long ClientContactId { get; set; }
        public long ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public virtual Client Client { get; set; }
    }
}
