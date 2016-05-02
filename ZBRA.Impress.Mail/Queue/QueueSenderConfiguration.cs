using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail.Queue
{
    public class QueueSenderConfiguration
    {
        public bool Enabled { get; set; }
        public int QueueRate { get; set; }
        public int MessageRate { get; set; }
        public int MaxRetries { get; set; }

        public QueueSenderConfiguration()
        {
            Enabled = true;
            QueueRate = 5 * 60 * 1000; // 5 minutes in miliseconds;
            MessageRate = 500; // 0.5 seconds in miliseconds;
            MaxRetries = 5; // 
        }
    }
}
