namespace QrBarCodeTestUtility.Models
{
    public class TfcInputModelBase
    {
        public Header Header { get; set; }
        public TaxpayerProfile TaxpayerProfile { get; set; }
        public TaxpayerFormProfile TaxpayerFormProfile { get; set; }
        public TfcInputModelBase()
        {
            Header = new Header();
            TaxpayerFormProfile = new TaxpayerFormProfile();
            TaxpayerProfile = new TaxpayerProfile();
        }
    }

    public class Header
    {
        public string TaxFormCode { get; set; }
        public int FilingYear { get; set; }
        public int FilingMonth { get; set; }
        public string PeriodStart { get; set; }
        public string PeriodEnd { get; set; }
        public string FilingFrequency { get; set; }
        public string FilingMethod { get; set; }
        public string SourceSystem { get; set; }
        public string CountryCode { get; set; }
        public string Jurisdiction { get; set; }

    }

    public class TaxpayerProfile
    {
        public string TaxpayerName { get; set; }
        public string EIN { get; set; }
        public string ContactAddress1 { get; set; }
        public string ContactAddress2 { get; set; }
        public string ContactCity { get; set; }
        public string ContactEmail { get; set; }
        public string ContactFax { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactFullName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactPostalCode { get; set; }
        public string ContactRegion { get; set; }
        public string ContactTitle { get; set; }
        public string StateRegistrationId { get; set; }
        public string TaxpayerAddress1 { get; set; }
        public string TaxpayerAddress2 { get; set; }
        public string TaxpayerCity { get; set; }
        public string TaxpayerCountry { get; set; }
        public string TaxpayerPhone { get; set; }
    }

    public class TaxpayerFormProfile
    {
        public string DueDate { get; set; }
        public string PeriodEnd { get; set; }
        public string PeriodStart { get; set; }
        public string TodaysDate { get; set; }
        public string TotalGrossSalesThisRegion { get; set; }
    }
}
