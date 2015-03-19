
using System;
namespace ZBRA.Impress.Validation.Consistency
{
    public class ReceivedParametersConsistencyException : Exception
    {
        private ValidationResult result;

        public ReceivedParametersConsistencyException(ValidationResult result)
        {
            this.result = result;
        }

        public ValidationResult GetValidationResult()
        {
            return result;
        }
    }
}
