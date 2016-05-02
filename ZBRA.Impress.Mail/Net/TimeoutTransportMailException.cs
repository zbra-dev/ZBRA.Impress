using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail.Net
{
    public class TimeoutTransportMailException : TransportMailMessage
    {
        public TimeoutTransportMailException(System.Exception ex) : base(ex) { }
    }
}
