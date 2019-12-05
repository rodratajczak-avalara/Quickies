using System;

namespace QrBarCodeTestUtility.Models
{
    /// <summary>
    /// FormMaster Model
    /// </summary>
    public class FormMasterModel
    {
        /// <summary>
        /// The unique id of form master.
        /// </summary>
        public Int64 FormMasterId { get; set; }

        /// <summary>
        /// The form type identifier.
        /// </summary>
        public Int64 FormTypeId { get; set; }

        /// <summary>
        /// The unique code identifying the tax form.
        /// </summary>
        public String TaxFormCode { get; set; }

        /// <summary>
        /// The name of the legacy return.
        /// </summary>
        public String LegacyReturnName { get; set; }

        /// <summary>
        /// The name of the tax form.
        /// </summary>
        public String TaxFormName { get; set; }

        /// <summary>
        /// The description.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if the FormMaster is effective; otherwise, <c>false</c>.
        /// </summary>
        public Boolean IsEffective { get; set; }

        /// <summary>
        /// The country.
        /// </summary>
        public String Country { get; set; }

        /// <summary>
        /// The region.
        /// </summary>
        public String Region { get; set; }

        /// <summary>
        /// The name of the authority.
        /// </summary>
        public String AuthorityName { get; set; }

        /// <summary>
        /// The short code.
        /// </summary>
        public String ShortCode { get; set; }

        /// <summary>
        /// The nullable value of the due day.
        /// </summary>
        public Int16? DueDay { get; set; }

        /// <summary>
        /// The nullable value of the delinquent day.
        /// </summary>
        public Int16? DelinquentDay { get; set; }

        /// <summary>
        /// The nullable value of the fiscal year start month.
        /// </summary>
        public Byte? FiscalYearStartMonth { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if the FormMaster has multi frequencies; otherwise, <c>false</c>.
        /// </summary>
        public Boolean HasMultiFrequencies { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if the POA (Point of Order) is required; otherwise, <c>false</c>.
        /// </summary>
        public Boolean IsPOARequired { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if the registration is required; otherwise, <c>false</c>.
        /// </summary>
        public Boolean IsRegistrationRequired { get; set; }

        /// <summary>
        ///Flag is <c>true</c> if it has multi-registration methods; otherwise, <c>false</c>.
        /// </summary>
        public Boolean HasMultiRegistrationMethods { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if the FormMaster has schedules; otherwise, <c>false</c>.
        /// </summary>
        public Boolean HasSchedules { get; set; }

        /// <summary>
        ///Flag is <c>true</c> if the FormMaster has multi-filing methods; otherwise, <c>false</c>.
        /// </summary>
        public Boolean HasMultiFilingMethods { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if the FormMaster has multi-pay methods; otherwise, <c>false</c>.
        /// </summary>
        public Boolean HasMultiPayMethods { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if EFT (Electronic Fund Transfers) is required; otherwise, <c>false</c>.
        /// </summary>
        public Boolean IsEFTRequired { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if file pay method is linked; otherwise, <c>false</c>.
        /// </summary>
        public Boolean IsFilePayMethodLinked { get; set; }

        /// <summary>
        /// The mailing received rule identifier.
        /// </summary>
        public Int64 MailingReceivedRuleId { get; set; }

        /// <summary>
        /// The proof of mailing identifier.
        /// </summary>
        public Int64 ProofOfMailingId { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if the negative amount is allowed; otherwise, <c>false</c>.
        /// </summary>
        public Boolean IsNegAmountAllowed { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if negative overall taxis allowed; otherwise, <c>false</c>.
        /// </summary>
        public Boolean AllowNegativeOverallTax { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if netting is required; otherwise, <c>false</c>.
        /// </summary>
        public Boolean IsNettingRequired { get; set; }

        /// <summary>
        /// The rounding method identifier.
        /// </summary>
        public Int64 RoundingMethodId { get; set; }

        /// <summary>
        /// The vendor discount annual maximum.
        /// </summary>
        public Decimal VendorDiscountAnnualMax { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if versions requires authority approval; otherwise, <c>false</c>.
        /// </summary>
        public Boolean VersionsRequireAuthorityApproval { get; set; }

        /// <summary>
        /// The outlet reporting method identifier.
        /// </summary>
        public Int64 OutletReportingMethodId { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if the FormMaster has reporting codes; otherwise, <c>false</c>.
        /// </summary>
        public Boolean HasReportingCodes { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if the FormMaster has prepayments; otherwise, <c>false</c>.
        /// </summary>
        public Boolean HasPrepayments { get; set; }

        /// <summary>
        ///Flag is <c>true</c> if gross includes interstate sales; otherwise, <c>false</c>.
        /// </summary>
        public Boolean GrossIncludesInterstateSales { get; set; }

        /// <summary>
        /// The gross includes tax.
        /// </summary>
        public String GrossIncludesTax { get; set; }

        /// <summary>
        /// Flag is <c>true</c> there is a fee for EFile; otherwise, <c>false</c>.
        /// </summary>
        public Boolean HasEfileFee { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if there is a feee for EPay; otherwise, <c>false</c>.
        /// </summary>
        public Boolean HasEpayFee { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if the FormMaster has dependencies; otherwise, <c>false</c>.
        /// </summary>
        public Boolean HasDependencies { get; set; }

        /// <summary>
        /// The required efile trigger.
        /// </summary>
        public String RequiredEfileTrigger { get; set; }

        /// <summary>
        /// The required EFT (Electronic Fund Transfers) trigger.
        /// </summary>
        public String RequiredEftTrigger { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if there is a vendor discount for EFile]; otherwise, <c>false</c>.
        /// </summary>
        public Boolean VendorDiscountEfile { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if there is a vendor discount paper; otherwise, <c>false</c>.
        /// </summary>
        public Boolean VendorDiscountPaper { get; set; }

        /// <summary>
        /// Flag is nullable, its true if the FormMaster is peer reviewed.
        /// </summary>
        public Boolean? PeerReviewed { get; set; }

        /// <summary>
        /// The peer reviewed identifier.
        /// </summary>
        public Int64? PeerReviewedId { get; set; }

        /// <summary>
        /// Date when FormMaster is peer reviewed.
        /// </summary>
        public DateTime? PeerReviewedDate { get; set; }

        /// <summary>
        /// User id of the user who created the FormMaster.
        /// </summary>
        public Int64 CreatedUserId { get; set; }

        /// <summary>
        /// Date when the FormMaster is created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// User id of the user who last modified the FormMaster.
        /// </summary>
        public Int64 ModifiedUserId { get; set; }

        /// <summary>
        /// Date when the FormMaster was last modified.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// The DOR (Department of Revenue) address mail to.
        /// </summary>
        public String DORAddressMailTo { get; set; }

        /// <summary>
        /// The DOR (Department of Revenue) address1.
        /// </summary>
        public String DORAddress1 { get; set; }

        /// <summary>
        /// The DOR (Department of Revenue) address2.
        /// </summary>
        public String DORAddress2 { get; set; }

        /// <summary>
        /// The DOR (Department of Revenue) address city.
        /// </summary>
        public String DORAddressCity { get; set; }

        /// <summary>
        /// The DOR (Department of Revenue) address region.
        /// </summary>
        public String DORAddressRegion { get; set; }

        /// <summary>
        /// The DOR (Department of Revenue) address postal code.
        /// </summary>
        public String DORAddressPostalCode { get; set; }

        /// <summary>
        /// The DOR (Department of Revenue) address country.
        /// </summary>
        public String DORAddressCountry { get; set; }

        /// <summary>
        /// The zero dollar return address mail to.
        /// </summary>
        public String ZeroAddressMailTo { get; set; }

        /// <summary>
        /// The zero dollar return address1.
        /// </summary>
        public String ZeroAddress1 { get; set; }

        /// <summary>
        /// The zero dollar return address2.
        /// </summary>
        public String ZeroAddress2 { get; set; }

        /// <summary>
        /// The zero dollar return address city.
        /// </summary>
        public String ZeroAddressCity { get; set; }

        /// <summary>
        /// The zero dollar return address region.
        /// </summary>
        public String ZeroAddressRegion { get; set; }

        /// <summary>
        /// The zero dollar return address postal code.
        /// </summary>
        public String ZeroAddressPostalCode { get; set; }

        /// <summary>
        /// The zero dollar return address country.
        /// </summary>
        public String ZeroAddressCountry { get; set; }

        /// <summary>
        /// The amended tax address mail to.
        /// </summary>
        public String AmendedAddressMailTo { get; set; }

        /// <summary>
        /// The amended tax address1.
        /// </summary>
        public String AmendedAddress1 { get; set; }

        /// <summary>
        /// The amended tax address2.
        /// </summary>
        public String AmendedAddress2 { get; set; }

        /// <summary>
        /// The amended tax address city.
        /// </summary>
        public String AmendedAddressCity { get; set; }

        /// <summary>
        /// The amended tax address region.
        /// </summary>
        public String AmendedAddressRegion { get; set; }

        /// <summary>
        /// The amended tax address postal code.
        /// </summary>
        public String AmendedAddressPostalCode { get; set; }

        /// <summary>
        /// The amended tax address country.
        /// </summary>
        public String AmendedAddressCountry { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if it allows online back filing; otherwise, <c>false</c>.
        /// </summary>
        public Boolean OnlineBackFiling { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if it allows online amended returns; otherwise, <c>false</c>.
        /// </summary>
        public Boolean OnlineAmendedReturns { get; set; }

        /// <summary>
        /// The prepayment frequency.
        /// </summary>
        public String PrepaymentFrequency { get; set; }

        /// <summary>
        /// Flag is nullable, its value is true if the outlet location identifiers is required.
        /// </summary>
        public Boolean? OutletLocationIdentifiersRequired { get; set; }

        /// <summary>
        /// The listing sort order.
        /// </summary>
        public String ListingSortOrder { get; set; }

        /// <summary>
        /// The DOR (Department of Revenue) website.
        /// </summary>
        public String DORWebsite { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if it allow file for all outlets]; otherwise, <c>false</c>.
        /// </summary>
        public Boolean FileForAllOutlets { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if paper forms do not have discounts; otherwise, <c>false</c>.
        /// </summary>
        public Boolean PaperFormsDoNotHaveDiscounts { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if it allows stack aggregation; otherwise, <c>false</c>.
        /// </summary>
        public Boolean StackAggregation { get; set; }

        /// <summary>
        /// The rounding precision.
        /// </summary>
        public Byte? RoundingPrecision { get; set; }

        /// <summary>
        /// The inconsistency tolerance.
        /// </summary>
        public Decimal? InconsistencyTolerance { get; set; }

        /// <summary>
        /// The effective date of the form version.
        /// </summary>
        public DateTime EffDate { get; set; }

        /// <summary>
        /// The end date of the form version.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The effective date of the FormMaster.
        /// </summary>
        public DateTime FormMasterEffDate { get; set; }

        /// <summary>
        /// The end date of the FormMaster.
        /// </summary>
        public DateTime FormMasterEndDate { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if it is visible to customers; otherwise, <c>false</c>.
        /// </summary>
        public Boolean VisibleToCustomers { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if it requires outlet setup; otherwise, <c>false</c>.
        /// </summary>
        public Boolean RequiresOutletSetup { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if ACH (Automated Clearing House) credit allowed; otherwise, <c>false</c>.
        /// </summary>
        public Boolean AchCreditAllowed { get; set; }

        /// <summary>
        /// The report level.
        /// </summary>
        public String ReportLevel { get; set; }

        /// <summary>
        /// Flag is <c>true</c> if post office is validated; otherwise, <c>false</c>.
        /// </summary>
        public Boolean PostOfficeValidated { get; set; }

        /// <summary>
        /// The stack aggregation option.
        /// </summary>
        public String StackAggregationOption { get; set; }

        /// <summary>
        /// The SST (Streamlined Sales Tax) behavior.
        /// </summary>
        public String SstBehavior { get; set; }

        /// <summary>
        /// The non SST (Streamlined Sales Tax) behavior.
        /// </summary>
        public String NonSstBehavior { get; set; }

        /// <summary>
        /// The DOR (Department of Revenue) phone number.
        /// </summary>
        public String DORPhoneNumber { get; set; }

        /// <summary>
        /// The FormVersionId associated with the FormMaster record.
        /// This property can either be the most recent FormVersionId or the FormVersionId for the passed in filing period.
        /// This property is only returned when calling the available forms API.
        /// </summary>
        public Int64? FormVersionId { get; set; }

        /// <summary>
        /// The default prepay percentage to be used on the filing calendar.
        /// </summary>
        public Int32 DefaultPrepayPercentage { get; set; }

        /// <summary>
        /// Gets or sets the prepayment calculation method.
        /// </summary>
        /// <value>
        /// The prepayment calculation method. (Current Period, Same Period Last Year, Average of Last Year)
        /// </value>
        public int? PrepaymentCalculationMethodId { get; set; }

        /// <summary>
        /// Whether or not fixed prepayments are allow for this form.
        /// </summary>
        public Boolean AllowFixedPrepayAmount { get; set; }

        /// <summary>
        /// Gets or sets the major (TFM Form Version).
        /// </summary>
        /// <value>
        /// The major.
        /// </value>
        public int Major { get; set; }

        /// <summary>
        /// Gets or sets the minor (TFM Form Version).
        /// </summary>
        /// <value>
        /// The minor.
        /// </value>
        public int Minor { get; set; } = 0;

        /// <summary>
        /// Gets or sets the revision (TFM Form Version).
        /// </summary>
        /// <value>
        /// The revision.
        /// </value>
        public int Revision { get; set; } = 0;

        /// <summary>
        /// Gets the form version.
        /// </summary>
        /// <value>
        /// The form version.
        /// </value>
        public string FormVersion { get { return $"{Major}.{Minor}.{Revision}"; } }

        /// <summary>
        /// Minimum months between occasional filings.
        /// </summary>
        public Int32 MonthsBetweenOccasionalFilings { get; set; }

        /// <summary>
        /// Filing criteria
        /// </summary>
        public string OccasionalFilingCriteria { get; set; }

        /// <summary>
        /// SplitOutOfStateLocations
        /// </summary>
        public bool SplitOutOfStateLocations { get; set; }

        public int BatchedEnvelopeRuleId { get; set; }

        /// <summary>
        /// SST returns are allowed to be filed without payment and then the payment is allowed to be transmitted later.
        /// </summary>
        public Boolean AllowFilingWithoutPayment { get; set; }

        #region Form Preparation Status
        /// <summary>
        /// Form is Ready to Prep
        /// </summary>
        public Boolean ReadyToPrep { get; set; }

        /// <summary>
        /// Expected Release Date for a form
        /// </summary>
        public DateTime? ExpectedReleaseDate { get; set; }

        /// <summary>
        /// Internal Notes for a form readiness
        /// </summary>
        public String InternalNotes { get; set; }

        /// <summary>
        /// External Notes for a form readiness
        /// </summary>
        public String ExternalNotes { get; set; }

        /// <summary>
        /// Associated Ticket Url for a form readiness
        /// </summary>
        public String AssociatedTicketUrl { get; set; }
        #endregion
    }
}
