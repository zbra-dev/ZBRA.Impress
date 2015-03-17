
using System;
namespace Zbra.Impress.Validation.Annotations
{
    public interface IValidationAttribute
    {
        object GetValidator(Type propertyType);
    }
}
