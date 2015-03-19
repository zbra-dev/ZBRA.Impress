using System;

namespace ZBRA.Impress
{
    public class EnumConversionException : Exception
    {
        public EnumConversionException(string message)
            : base(message) { }
    }
}
