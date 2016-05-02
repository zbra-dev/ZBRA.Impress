using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail
{
    public class MailException : Exception
    {
        public MailException(System.Exception ex) : base(ex.Message, ex) { }

        public MailException(string message) : base(message) { }
    }
}
