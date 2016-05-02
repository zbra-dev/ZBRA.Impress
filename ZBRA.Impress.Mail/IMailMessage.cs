using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Mail
{
    /// <summary>
    /// Read only Message Data
    /// </summary>
    public interface IMailMessage
    {

        string From { get; }
        string To { get; }
        string Cc { get; }
        string ReplyTo { get; }
        string Bcc { get; }
        string Subject { get; }
        string Body { get; }
        bool IsHtmlBody { get; }

        IReadOnlyList<IMailAttachment> Attachments { get; }
    }
}
