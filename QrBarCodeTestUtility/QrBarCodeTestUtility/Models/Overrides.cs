using System.Collections.Generic;

namespace QrBarCodeTestUtility.Models
{
    public class Overrides : Dictionary<string, OverrideMap>
    {

    }

    public class OverrideMap
    {
        /// <summary>
        /// DataType
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// Original
        /// </summary>
        public string Original { get; set; }
        /// <summary>
        /// Modified
        /// </summary>
        public string Modified { get; set; }
    }
}
