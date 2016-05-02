using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail.Net
{
    public class MailVerificationException : MailException
    {
        internal MailVerificationException(string message) : base(message) { }
        internal MailVerificationException(System.Exception ex) : base(ex) { }
    }
}
