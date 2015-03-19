
using System;
namespace ZBRA.Impress.Validation.Annotations
{
    public interface IValidationAttribute
    {
        object GetValidator(Type propertyType);
    }
}
