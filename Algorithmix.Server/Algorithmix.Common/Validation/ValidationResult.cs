using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Common.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }

        public override string ToString()
        {
            return $"Valid: {IsValid}. {ValidationErrors.Count()} errors";
        }
    }
}
