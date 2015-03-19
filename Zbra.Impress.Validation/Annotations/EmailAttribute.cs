using System;

namespace ZBRA.Impress.Validation.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EmailAttribute : Attribute
    {
    }
}
