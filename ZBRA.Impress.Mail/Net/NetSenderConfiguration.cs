using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail.Net
{
    public class NetSenderConfiguration
    {
        public bool Enabled { get; set; }
        public bool EnableSsl { get; set; }

        public NetSenderConfiguration()
        {
            Enabled = true;
            EnableSsl = false;
        }
    }
}
