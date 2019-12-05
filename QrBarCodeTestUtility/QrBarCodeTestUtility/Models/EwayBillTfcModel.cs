using System.Collections.Generic;

namespace QrBarCodeTestUtility.Models
{
    public class EwayBillTfcModel : TfcInputModelBase
    {
        public EwayBillTfcInputModel Eway { get; set; }
        public List<TfcVehicleList> vehicleListDetails { get; set; }
        public List<TfcLineItemList> itemList { get; set; }
    }

    public class EwayBillTfcInputModel
    {
        public string supplyType { get; set; }
        public string subSupplyType { get; set; }
        public string docType { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string fromPlace { get; set; }
        public long? fromPincode { get; set; }
        public string fromGstin { get; set; }
        public string fromTrdName { get; set; }
        public string fromAddr1 { get; set; }
        public string fromAddr2 { get; set; }

        public string fromStateCode { get; set; }
        public string actFromStateCode { get; set; }
        public string toGstin { get; set; }
        public string toTrdName { get; set; }
        public string toAddr1 { get; set; }
        public string toAddr2 { get; set; }
        public string toPlace { get; set; }
        public long? toPincode { get; set; }

        public string toStateCode { get; set; }
        public string actToStateCode { get; set; }
        public double totalValue { get; set; }
        public double cgstValue { get; set; }
        public double sgstValue { get; set; }
        public double igstValue { get; set; }
        public double cessValue { get; set; }
        public double cessNonAdvolValue { get; set; }
        public string transporterId { get; set; }
        public string transporterName { get; set; }
        public string transDocNo { get; set; }
        public string transMode { get; set; }
        public string transDistance { get; set; }
        public string transDocDate { get; set; }
        public string vehicleNo { get; set; }
        public string vehicleType { get; set; }
        public string transactionType { get; set; }
        public string BarCode { get; set; }
        public string QRCode { get; set; }
        public string ewbNo { get; set; }
        public string ewayBillDate { get; set; }
        public string genMode { get; set; }
        public string userGstin { get; set; }

        public double totInvValue { get; set; }
        public double otherValue { get; set; }

        public string status { get; set; }
        public int actualDist { get; set; }
        public int noValidDays { get; set; }
        public string validUpto { get; set; }
        public int extendedTimes { get; set; }
        public string rejectStatus { get; set; }
    }

    public class TfcVehicleList : VehiclListDetail
    {
        public int FormPosition { get; set; }
    }

    public class TfcLineItemList : ItemList
    {
        public int FormPosition { get; set; }

    }

    public class VehiclListDetail
    {
        public string updMode { get; set; }
        public string vehicleNo { get; set; }
        public string fromPlace { get; set; }
        public int fromState { get; set; }
        public long tripshtNo { get; set; }
        public string userGSTINTransin { get; set; }
        public string enteredDate { get; set; }
        public string transMode { get; set; }
        public string transDocNo { get; set; }
        public string transDocDate { get; set; }
        public string cewbNo { get; set; }
    }

    public class ItemList
    {
        public string productName { get; set; }
        public string productDesc { get; set; }
        public int hsnCode { get; set; }
        public double quantity { get; set; }
        public string qtyUnit { get; set; }
        public double cgstRate { get; set; }
        public double sgstRate { get; set; }
        public double igstRate { get; set; }
        public double cessRate { get; set; }
        public double cessAdvol { get; set; }
        public double cessNonAdvolRate { get; set; }
        public double taxableAmount { get; set; }
    }
}
