using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail.Net
{
    public class NetMailAddressVerifierConfiguration
    {

        public string RequesterEmail { get; set; }
        public string RequesterHost { get; set; }

        public NetMailAddressVerifierConfiguration()
        {
            this.RequesterEmail = "YourGmailIDHere@gmail.com";
        }

    }
}
