using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail.Net
{
    public class AuthenticationRequiredMailMessage : MailException
    {
        public AuthenticationRequiredMailMessage(System.Exception ex) : base(ex) { }
    }
}
