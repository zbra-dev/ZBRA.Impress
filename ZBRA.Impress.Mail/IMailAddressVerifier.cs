using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail
{
    interface IMailAddressVerifier
    {

        bool VerifyExists(string mailAddress);
    }
}
