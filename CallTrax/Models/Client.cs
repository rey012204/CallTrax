using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class Client
    {
        public Client()
        {
            CallFlow = new HashSet<CallFlow>();
            ClientContact = new HashSet<ClientContact>();
        }

        public long ClientId { get; set; }
        public string ClientName { get; set; }

        public virtual ICollection<CallFlow> CallFlow { get; set; }
        public virtual ICollection<ClientContact> ClientContact { get; set; }
    }
}
