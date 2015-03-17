using System;

namespace Zbra.Impress
{
    public class InvalidFormatConverterException : ConverterException
    {
        public InvalidFormatConverterException(string message, Exception e)
            : base(message, e) { }
    }
}
