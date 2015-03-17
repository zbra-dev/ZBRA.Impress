using System;

namespace Zbra.Impress
{
    public class EnumConversionException : Exception
    {
        public EnumConversionException(string message)
            : base(message) { }
    }
}
