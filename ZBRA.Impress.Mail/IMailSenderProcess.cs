using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail
{
    public interface IMailSenderProcess : IMailSender
    {
        void Start();
        void Stop();
    }
}
