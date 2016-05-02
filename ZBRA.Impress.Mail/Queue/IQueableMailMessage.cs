using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail.Queue
{
    public interface IQueableMailMessage : IMailMessage
    {
        long? Id { get; }
        DateTime CreationDate { get; }
        DateTime NextRetryDate { get; }
        int Retries { get; }
        void IncrementRetry();
    }
}
