using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;



namespace QrBarCodeTestUtility.Models
{
    public class ComputedFormData
    {
        public Version Version { get; set; }
        public CIFHeader Header { get; set; }
        public OrderedDictionary returnsData { get; set; }
        public OrderedDictionary pdfFeed { get; set; }
        public OrderedDictionary summary { get; set; }
        public Overrides overrides { get; set; }
        public JObject inputDataChanges { get; set; }
        public RepeatGroups repeatGroups { get; set; }
        public Dictionary<Int32, HashSet<Int32>> omittedPages { get; set; }
        public List<LogEntry> logs { get; set; }
        public string pdfFileKey { get; set; }
        public ValidationResult ValidationResult { get; set; }
        public List<string> signatureFieldNames { get; set; }
    }

    public class CIFHeader
    {
        public string TaxFormCode { get; set; }
        public int? FilingYear { get; set; }
        public int? FilingMonth { get; set; }
        public string PeriodStart { get; set; }
        public string PeriodEnd { get; set; }
        public string FilingFrequency { get; set; }
        public string FilingMethod { get; set; }
        public string SourceSystem { get; set; }
        public string CompanyIdentifier { get; set; }
        public string CountryCode { get; set; }
        public string Jurisdiction { get; set; }
        public bool WithoutDiscount { get; set; }
        public string TFOReturnId { get; set; }
        public string Environment { get; set; }
        public string TFOCompanyId { get; set; }
    }

}
