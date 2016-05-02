﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail.StorableQueue
{
    public class StorableMailAttachment : IMailAttachment
    {
        public long? Id { get; set; }
        public StorableMailMessage ParentMessage { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }


        public StorableMailAttachment(StorableMailMessage parentMessage)
        {
            this.ParentMessage = parentMessage;
        }

        public Stream GetContentStream()
        {
            return new MemoryStream(Content);
        }

        public string GetContentType()
        {
            return ContentType;
        }
    }
}
