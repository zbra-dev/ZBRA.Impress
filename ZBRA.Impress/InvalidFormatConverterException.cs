using System;

namespace ZBRA.Impress
{
    public class InvalidFormatConverterException : ConverterException
    {
        public InvalidFormatConverterException(string message, Exception e)
            : base(message, e) { }
    }
}
