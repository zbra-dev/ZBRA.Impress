using System;

namespace Zbra.Impress.Validation.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EmailAttribute : Attribute
    {
    }
}
