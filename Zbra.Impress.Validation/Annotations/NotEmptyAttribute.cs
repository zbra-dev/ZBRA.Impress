using System;

namespace Zbra.Impress.Validation.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = true)]
    public class NotEmptyAttribute : Attribute, IValidationAttribute
    {
        public object GetValidator(Type propertyType)
        {
            Type type = typeof(NotNullValidator<>).MakeGenericType(propertyType);
            return Activator.CreateInstance(type);
        }
    }
}
