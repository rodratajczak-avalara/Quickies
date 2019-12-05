using System;
using Newtonsoft.Json;

namespace QrBarCodeTestUtility.Models
{
    public class ValidationError
    {
        [JsonConverter(typeof(ValidationErrorSeverityEnumConvert))]
        public ValidationErrorSeverity Severity { get; set; }
        public string Message { get; set; }
    }

    public enum ValidationErrorSeverity
    {
        ErrorNotice,
        UnableToFile,
        Warning
    }

    public class ValidationErrorSeverityEnumConvert : JsonConverter
    {
        private const string ERROR_NOTICE = "ERROR-NOTICE";
        private const string UNABLE_TO_FILE = "UNABLE-TO-FILE";
        private const string WARNING = "WARNING";

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ValidationErrorSeverity severity = (ValidationErrorSeverity)value;
            switch (severity)
            {
                case ValidationErrorSeverity.ErrorNotice:
                    writer.WriteValue(ERROR_NOTICE);
                    break;
                case ValidationErrorSeverity.UnableToFile:
                    writer.WriteValue(UNABLE_TO_FILE);
                    break;
                case ValidationErrorSeverity.Warning:
                    writer.WriteValue(WARNING);
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var enumString = (string)reader.Value;
            ValidationErrorSeverity? severity = null;

            if (String.Compare(enumString, ERROR_NOTICE, ignoreCase: true) == 0)
            {
                severity = ValidationErrorSeverity.ErrorNotice;
            }
            else if (String.Compare(enumString, UNABLE_TO_FILE, ignoreCase: true) == 0)
            {
                severity = ValidationErrorSeverity.UnableToFile;
            }
            else if (String.Compare(enumString, WARNING, ignoreCase: true) == 0)
            {
                severity = ValidationErrorSeverity.Warning;
            }

            return severity;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
