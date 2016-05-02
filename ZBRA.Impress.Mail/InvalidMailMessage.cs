using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail
{
    public class InvalidMailMessage : MailException
    {
        public InvalidMailMessage(System.Exception ex) : base(ex) { }

        public InvalidMailMessage(string message) : base(message) { }
    }
}
