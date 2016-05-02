﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail
{
    public interface IMailAttachment
    {
        /// <summary>
        /// The stream of data to copy to the mail attachment
        /// </summary>
        /// <returns></returns>
        System.IO.Stream GetContentStream();
        /// <summary>
        /// The stream content content MIME type. 
        /// Exampls : text/plain ,  application/vnd.ms-excel , application/pdf
        /// </summary>
        /// <returns></returns>
        string GetContentType();
    }
}
