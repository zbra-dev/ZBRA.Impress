using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail.Queue
{
    public class QueueMailAttachment : IMailAttachment
    {
        private string contentType;
        private Stream stream;

        public QueueMailAttachment(string contentType, Stream stream)
        {
            this.contentType = contentType;
            this.stream = stream;
        }


        public System.IO.Stream GetContentStream()
        {
            return stream;
        }

        public string GetContentType()
        {
            return this.contentType;
        }
    }
}
