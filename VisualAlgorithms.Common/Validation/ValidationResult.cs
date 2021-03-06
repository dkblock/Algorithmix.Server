using System.Collections.Generic;

namespace VisualAlgorithms.Common.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }
    }
}
