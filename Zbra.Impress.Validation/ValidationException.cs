using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBRA.Impress.Validation
{
    public class ValidationException : Exception
    {
        private ValidationResult result;

        public ValidationException(ValidationResult result)
        {
            this.result = result;
        }

        public ValidationResult GetValidationResult()
        {
            return result;
        }
    }
}
