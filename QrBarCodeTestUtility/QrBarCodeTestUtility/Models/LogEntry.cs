using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace QrBarCodeTestUtility.Models
{
    public class LogEntry
    {
        //public string LoggerName{ get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public LogEntryLevel Level { get; set; }
        public string Message { get; set; }
    }

    public enum LogEntryLevel
    {
        FATAL = 0,
        ERROR = 10,
        WARNING = 20,
        INFO = 30,
        DEBUG = 40
    }
}
