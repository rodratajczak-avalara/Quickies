using System;
using System.Collections.Generic;

namespace QrBarCodeTestUtility.Models
{
    public class ValidationResult
    {
        public List<ValidationError> TopLevelErrors { get; set; }
        public Dictionary<String, List<ValidationError>> FieldErrors { get; set; }
    }
}
