CREATE OR ALTER PROCEDURE dbo.sp_Merge_Document_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.Document ON

MERGE dbo.Document AS target
USING (SELECT DocumentId, DocumentTypeId, CompanyId, DocumentDate, DocumentCode, DocumentStatusId, PurchaseOrderNo, CustomerVendorCode, SalespersonCode, CustomerUsageType, ReferenceCode, IsReconciled, TotalAmount, TotalTax, ExemptNo, ModifiedDate, ModifiedUserId, TaxDate, DocumentLineCount, HashCode, TotalTaxable, TotalExempt, BatchCode, SoftwareVersion, LocationCode, AdjustmentReasonId, AdjustmentDescription, IsLocked, Version, TotalTaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, CurrencyCode, PaymentDate, OriginAddressId, DestinationAddressId, ExchangeRate, ExchangeRateEffDate, AdjustedStatusId, Region, Country, PosLaneCode, BusinessIdentificationNo, IsSellerImporterOfRecord, BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL, Description, Email FROM dbo.Document_Temp_Shard_1) 
	AS source (DocumentId, DocumentTypeId, CompanyId, DocumentDate, DocumentCode, DocumentStatusId, PurchaseOrderNo, CustomerVendorCode, SalespersonCode, CustomerUsageType, ReferenceCode, IsReconciled, TotalAmount, TotalTax, ExemptNo, ModifiedDate, ModifiedUserId, TaxDate, DocumentLineCount, HashCode, TotalTaxable, TotalExempt, BatchCode, SoftwareVersion, LocationCode, AdjustmentReasonId, AdjustmentDescription, IsLocked, Version, TotalTaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, CurrencyCode, PaymentDate, OriginAddressId, DestinationAddressId, ExchangeRate, ExchangeRateEffDate, AdjustedStatusId, Region, Country, PosLaneCode, BusinessIdentificationNo, IsSellerImporterOfRecord, BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL, Description, Email)
ON (target.DocumentId = source.DocumentId)
WHEN MATCHED THEN
UPDATE SET DocumentTypeId = source.DocumentTypeId, DocumentCode = source.DocumentCode, DocumentStatusId = source.DocumentStatusId, PurchaseOrderNo = source.PurchaseOrderNo, CustomerVendorCode = source.CustomerVendorCode, SalespersonCode = source.SalespersonCode, CustomerUsageType = source.CustomerUsageType, ReferenceCode = source.ReferenceCode, IsReconciled = source.IsReconciled, TotalAmount = source.TotalAmount, TotalTax = source.TotalTax, ExemptNo = source.ExemptNo, ModifiedDate = source.ModifiedDate, ModifiedUserId = source.ModifiedUserId, TaxDate = source.TaxDate, DocumentLineCount = source.DocumentLineCount, HashCode = source.HashCode, TotalTaxable = source.TotalTaxable, TotalExempt = source.TotalExempt, BatchCode = source.BatchCode, SoftwareVersion = source.SoftwareVersion, LocationCode = source.LocationCode, AdjustmentReasonId = source.AdjustmentReasonId, AdjustmentDescription = source.AdjustmentDescription, IsLocked = source.IsLocked, Version = source.Version, TotalTaxCalculated = source.TotalTaxCalculated, TaxOverrideAmount = source.TaxOverrideAmount, TaxOverrideTypeId = source.TaxOverrideTypeId, TaxOverrideReason = source.TaxOverrideReason, CurrencyCode = source.CurrencyCode, PaymentDate = source.PaymentDate, OriginAddressId = source.OriginAddressId, DestinationAddressId = source.DestinationAddressId, ExchangeRate = source.ExchangeRate, ExchangeRateEffDate = source.ExchangeRateEffDate, AdjustedStatusId = source.AdjustedStatusId, Region = source.Region, Country = source.Country, PosLaneCode = source.PosLaneCode, BusinessIdentificationNo = source.BusinessIdentificationNo, IsSellerImporterOfRecord = source.IsSellerImporterOfRecord, BRBuyerType = source.BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF = source.BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF = source.BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF = source.BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF = source.BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS = source.BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS = source.BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL = source.BRBuyer_IsExempt_CSLL, Description = source.Description, Email = source.Email
WHEN NOT MATCHED THEN
INSERT (DocumentId, DocumentTypeId, CompanyId, DocumentDate, DocumentCode, DocumentStatusId, PurchaseOrderNo, CustomerVendorCode, SalespersonCode, CustomerUsageType, ReferenceCode, IsReconciled, TotalAmount, TotalTax, ExemptNo, ModifiedDate, ModifiedUserId, TaxDate, DocumentLineCount, HashCode, TotalTaxable, TotalExempt, BatchCode, SoftwareVersion, LocationCode, AdjustmentReasonId, AdjustmentDescription, IsLocked, Version, TotalTaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, CurrencyCode, PaymentDate, OriginAddressId, DestinationAddressId, ExchangeRate, ExchangeRateEffDate, AdjustedStatusId, Region, Country, PosLaneCode, BusinessIdentificationNo, IsSellerImporterOfRecord, BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL, Description, Email)
VALUES (source.DocumentId, source.DocumentTypeId, source.CompanyId, source.DocumentDate, source.DocumentCode, source.DocumentStatusId, source.PurchaseOrderNo, source.CustomerVendorCode, source.SalespersonCode, source.CustomerUsageType, source.ReferenceCode, source.IsReconciled, source.TotalAmount, source.TotalTax, source.ExemptNo, source.ModifiedDate, source.ModifiedUserId, source.TaxDate, source.DocumentLineCount, source.HashCode, source.TotalTaxable, source.TotalExempt, source.BatchCode, source.SoftwareVersion, source.LocationCode, source.AdjustmentReasonId, source.AdjustmentDescription, source.IsLocked, source.Version, source.TotalTaxCalculated, source.TaxOverrideAmount, source.TaxOverrideTypeId, source.TaxOverrideReason, source.CurrencyCode, source.PaymentDate, source.OriginAddressId, source.DestinationAddressId, source.ExchangeRate, source.ExchangeRateEffDate, source.AdjustedStatusId, source.Region, source.Country, source.PosLaneCode, source.BusinessIdentificationNo, source.IsSellerImporterOfRecord, source.BRBuyerType, source.BRBuyer_IsExemptOrCannotWH_IRRF, source.BRBuyer_IsExemptOrCannotWH_PISRF, source.BRBuyer_IsExemptOrCannotWH_COFINSRF, source.BRBuyer_IsExemptOrCannotWH_CSLLRF, source.BRBuyer_IsExempt_PIS, source.BRBuyer_IsExempt_COFINS, source.BRBuyer_IsExempt_CSLL, source.Description, source.Email)
;

SET IDENTITY_INSERT dbo.Document OFF

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_Document_From_Temp_Shard_2
AS
BEGIN

SET IDENTITY_INSERT dbo.Document ON

MERGE dbo.Document AS target
USING (SELECT DocumentId, DocumentTypeId, CompanyId, DocumentDate, DocumentCode, DocumentStatusId, PurchaseOrderNo, CustomerVendorCode, SalespersonCode, CustomerUsageType, ReferenceCode, IsReconciled, TotalAmount, TotalTax, ExemptNo, ModifiedDate, ModifiedUserId, TaxDate, DocumentLineCount, HashCode, TotalTaxable, TotalExempt, BatchCode, SoftwareVersion, LocationCode, AdjustmentReasonId, AdjustmentDescription, IsLocked, Version, TotalTaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, CurrencyCode, PaymentDate, OriginAddressId, DestinationAddressId, ExchangeRate, ExchangeRateEffDate, AdjustedStatusId, Region, Country, PosLaneCode, BusinessIdentificationNo, IsSellerImporterOfRecord, BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL, Description, Email FROM dbo.Document_Temp_Shard_2) 
	AS source (DocumentId, DocumentTypeId, CompanyId, DocumentDate, DocumentCode, DocumentStatusId, PurchaseOrderNo, CustomerVendorCode, SalespersonCode, CustomerUsageType, ReferenceCode, IsReconciled, TotalAmount, TotalTax, ExemptNo, ModifiedDate, ModifiedUserId, TaxDate, DocumentLineCount, HashCode, TotalTaxable, TotalExempt, BatchCode, SoftwareVersion, LocationCode, AdjustmentReasonId, AdjustmentDescription, IsLocked, Version, TotalTaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, CurrencyCode, PaymentDate, OriginAddressId, DestinationAddressId, ExchangeRate, ExchangeRateEffDate, AdjustedStatusId, Region, Country, PosLaneCode, BusinessIdentificationNo, IsSellerImporterOfRecord, BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL, Description, Email)
ON (target.DocumentId = source.DocumentId)
WHEN MATCHED THEN
UPDATE SET DocumentTypeId = source.DocumentTypeId, DocumentCode = source.DocumentCode, DocumentStatusId = source.DocumentStatusId, PurchaseOrderNo = source.PurchaseOrderNo, CustomerVendorCode = source.CustomerVendorCode, SalespersonCode = source.SalespersonCode, CustomerUsageType = source.CustomerUsageType, ReferenceCode = source.ReferenceCode, IsReconciled = source.IsReconciled, TotalAmount = source.TotalAmount, TotalTax = source.TotalTax, ExemptNo = source.ExemptNo, ModifiedDate = source.ModifiedDate, ModifiedUserId = source.ModifiedUserId, TaxDate = source.TaxDate, DocumentLineCount = source.DocumentLineCount, HashCode = source.HashCode, TotalTaxable = source.TotalTaxable, TotalExempt = source.TotalExempt, BatchCode = source.BatchCode, SoftwareVersion = source.SoftwareVersion, LocationCode = source.LocationCode, AdjustmentReasonId = source.AdjustmentReasonId, AdjustmentDescription = source.AdjustmentDescription, IsLocked = source.IsLocked, Version = source.Version, TotalTaxCalculated = source.TotalTaxCalculated, TaxOverrideAmount = source.TaxOverrideAmount, TaxOverrideTypeId = source.TaxOverrideTypeId, TaxOverrideReason = source.TaxOverrideReason, CurrencyCode = source.CurrencyCode, PaymentDate = source.PaymentDate, OriginAddressId = source.OriginAddressId, DestinationAddressId = source.DestinationAddressId, ExchangeRate = source.ExchangeRate, ExchangeRateEffDate = source.ExchangeRateEffDate, AdjustedStatusId = source.AdjustedStatusId, Region = source.Region, Country = source.Country, PosLaneCode = source.PosLaneCode, BusinessIdentificationNo = source.BusinessIdentificationNo, IsSellerImporterOfRecord = source.IsSellerImporterOfRecord, BRBuyerType = source.BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF = source.BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF = source.BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF = source.BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF = source.BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS = source.BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS = source.BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL = source.BRBuyer_IsExempt_CSLL, Description = source.Description, Email = source.Email
WHEN NOT MATCHED THEN
INSERT (DocumentId, DocumentTypeId, CompanyId, DocumentDate, DocumentCode, DocumentStatusId, PurchaseOrderNo, CustomerVendorCode, SalespersonCode, CustomerUsageType, ReferenceCode, IsReconciled, TotalAmount, TotalTax, ExemptNo, ModifiedDate, ModifiedUserId, TaxDate, DocumentLineCount, HashCode, TotalTaxable, TotalExempt, BatchCode, SoftwareVersion, LocationCode, AdjustmentReasonId, AdjustmentDescription, IsLocked, Version, TotalTaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, CurrencyCode, PaymentDate, OriginAddressId, DestinationAddressId, ExchangeRate, ExchangeRateEffDate, AdjustedStatusId, Region, Country, PosLaneCode, BusinessIdentificationNo, IsSellerImporterOfRecord, BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL, Description, Email)
VALUES (source.DocumentId, source.DocumentTypeId, source.CompanyId, source.DocumentDate, source.DocumentCode, source.DocumentStatusId, source.PurchaseOrderNo, source.CustomerVendorCode, source.SalespersonCode, source.CustomerUsageType, source.ReferenceCode, source.IsReconciled, source.TotalAmount, source.TotalTax, source.ExemptNo, source.ModifiedDate, source.ModifiedUserId, source.TaxDate, source.DocumentLineCount, source.HashCode, source.TotalTaxable, source.TotalExempt, source.BatchCode, source.SoftwareVersion, source.LocationCode, source.AdjustmentReasonId, source.AdjustmentDescription, source.IsLocked, source.Version, source.TotalTaxCalculated, source.TaxOverrideAmount, source.TaxOverrideTypeId, source.TaxOverrideReason, source.CurrencyCode, source.PaymentDate, source.OriginAddressId, source.DestinationAddressId, source.ExchangeRate, source.ExchangeRateEffDate, source.AdjustedStatusId, source.Region, source.Country, source.PosLaneCode, source.BusinessIdentificationNo, source.IsSellerImporterOfRecord, source.BRBuyerType, source.BRBuyer_IsExemptOrCannotWH_IRRF, source.BRBuyer_IsExemptOrCannotWH_PISRF, source.BRBuyer_IsExemptOrCannotWH_COFINSRF, source.BRBuyer_IsExemptOrCannotWH_CSLLRF, source.BRBuyer_IsExempt_PIS, source.BRBuyer_IsExempt_COFINS, source.BRBuyer_IsExempt_CSLL, source.Description, source.Email)
;

SET IDENTITY_INSERT dbo.Document OFF

END
GO





CREATE OR ALTER PROCEDURE dbo.sp_Merge_Document_From_Temp_Shard_3
AS
BEGIN

SET IDENTITY_INSERT dbo.Document ON

MERGE dbo.Document AS target
USING (SELECT DocumentId, DocumentTypeId, CompanyId, DocumentDate, DocumentCode, DocumentStatusId, PurchaseOrderNo, CustomerVendorCode, SalespersonCode, CustomerUsageType, ReferenceCode, IsReconciled, TotalAmount, TotalTax, ExemptNo, ModifiedDate, ModifiedUserId, TaxDate, DocumentLineCount, HashCode, TotalTaxable, TotalExempt, BatchCode, SoftwareVersion, LocationCode, AdjustmentReasonId, AdjustmentDescription, IsLocked, Version, TotalTaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, CurrencyCode, PaymentDate, OriginAddressId, DestinationAddressId, ExchangeRate, ExchangeRateEffDate, AdjustedStatusId, Region, Country, PosLaneCode, BusinessIdentificationNo, IsSellerImporterOfRecord, BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL, Description, Email FROM dbo.Document_Temp_Shard_3) 
	AS source (DocumentId, DocumentTypeId, CompanyId, DocumentDate, DocumentCode, DocumentStatusId, PurchaseOrderNo, CustomerVendorCode, SalespersonCode, CustomerUsageType, ReferenceCode, IsReconciled, TotalAmount, TotalTax, ExemptNo, ModifiedDate, ModifiedUserId, TaxDate, DocumentLineCount, HashCode, TotalTaxable, TotalExempt, BatchCode, SoftwareVersion, LocationCode, AdjustmentReasonId, AdjustmentDescription, IsLocked, Version, TotalTaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, CurrencyCode, PaymentDate, OriginAddressId, DestinationAddressId, ExchangeRate, ExchangeRateEffDate, AdjustedStatusId, Region, Country, PosLaneCode, BusinessIdentificationNo, IsSellerImporterOfRecord, BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL, Description, Email)
ON (target.DocumentId = source.DocumentId)
WHEN MATCHED THEN
UPDATE SET DocumentTypeId = source.DocumentTypeId, DocumentCode = source.DocumentCode, DocumentStatusId = source.DocumentStatusId, PurchaseOrderNo = source.PurchaseOrderNo, CustomerVendorCode = source.CustomerVendorCode, SalespersonCode = source.SalespersonCode, CustomerUsageType = source.CustomerUsageType, ReferenceCode = source.ReferenceCode, IsReconciled = source.IsReconciled, TotalAmount = source.TotalAmount, TotalTax = source.TotalTax, ExemptNo = source.ExemptNo, ModifiedDate = source.ModifiedDate, ModifiedUserId = source.ModifiedUserId, TaxDate = source.TaxDate, DocumentLineCount = source.DocumentLineCount, HashCode = source.HashCode, TotalTaxable = source.TotalTaxable, TotalExempt = source.TotalExempt, BatchCode = source.BatchCode, SoftwareVersion = source.SoftwareVersion, LocationCode = source.LocationCode, AdjustmentReasonId = source.AdjustmentReasonId, AdjustmentDescription = source.AdjustmentDescription, IsLocked = source.IsLocked, Version = source.Version, TotalTaxCalculated = source.TotalTaxCalculated, TaxOverrideAmount = source.TaxOverrideAmount, TaxOverrideTypeId = source.TaxOverrideTypeId, TaxOverrideReason = source.TaxOverrideReason, CurrencyCode = source.CurrencyCode, PaymentDate = source.PaymentDate, OriginAddressId = source.OriginAddressId, DestinationAddressId = source.DestinationAddressId, ExchangeRate = source.ExchangeRate, ExchangeRateEffDate = source.ExchangeRateEffDate, AdjustedStatusId = source.AdjustedStatusId, Region = source.Region, Country = source.Country, PosLaneCode = source.PosLaneCode, BusinessIdentificationNo = source.BusinessIdentificationNo, IsSellerImporterOfRecord = source.IsSellerImporterOfRecord, BRBuyerType = source.BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF = source.BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF = source.BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF = source.BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF = source.BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS = source.BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS = source.BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL = source.BRBuyer_IsExempt_CSLL, Description = source.Description, Email = source.Email
WHEN NOT MATCHED THEN
INSERT (DocumentId, DocumentTypeId, CompanyId, DocumentDate, DocumentCode, DocumentStatusId, PurchaseOrderNo, CustomerVendorCode, SalespersonCode, CustomerUsageType, ReferenceCode, IsReconciled, TotalAmount, TotalTax, ExemptNo, ModifiedDate, ModifiedUserId, TaxDate, DocumentLineCount, HashCode, TotalTaxable, TotalExempt, BatchCode, SoftwareVersion, LocationCode, AdjustmentReasonId, AdjustmentDescription, IsLocked, Version, TotalTaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, CurrencyCode, PaymentDate, OriginAddressId, DestinationAddressId, ExchangeRate, ExchangeRateEffDate, AdjustedStatusId, Region, Country, PosLaneCode, BusinessIdentificationNo, IsSellerImporterOfRecord, BRBuyerType, BRBuyer_IsExemptOrCannotWH_IRRF, BRBuyer_IsExemptOrCannotWH_PISRF, BRBuyer_IsExemptOrCannotWH_COFINSRF, BRBuyer_IsExemptOrCannotWH_CSLLRF, BRBuyer_IsExempt_PIS, BRBuyer_IsExempt_COFINS, BRBuyer_IsExempt_CSLL, Description, Email)
VALUES (source.DocumentId, source.DocumentTypeId, source.CompanyId, source.DocumentDate, source.DocumentCode, source.DocumentStatusId, source.PurchaseOrderNo, source.CustomerVendorCode, source.SalespersonCode, source.CustomerUsageType, source.ReferenceCode, source.IsReconciled, source.TotalAmount, source.TotalTax, source.ExemptNo, source.ModifiedDate, source.ModifiedUserId, source.TaxDate, source.DocumentLineCount, source.HashCode, source.TotalTaxable, source.TotalExempt, source.BatchCode, source.SoftwareVersion, source.LocationCode, source.AdjustmentReasonId, source.AdjustmentDescription, source.IsLocked, source.Version, source.TotalTaxCalculated, source.TaxOverrideAmount, source.TaxOverrideTypeId, source.TaxOverrideReason, source.CurrencyCode, source.PaymentDate, source.OriginAddressId, source.DestinationAddressId, source.ExchangeRate, source.ExchangeRateEffDate, source.AdjustedStatusId, source.Region, source.Country, source.PosLaneCode, source.BusinessIdentificationNo, source.IsSellerImporterOfRecord, source.BRBuyerType, source.BRBuyer_IsExemptOrCannotWH_IRRF, source.BRBuyer_IsExemptOrCannotWH_PISRF, source.BRBuyer_IsExemptOrCannotWH_COFINSRF, source.BRBuyer_IsExemptOrCannotWH_CSLLRF, source.BRBuyer_IsExempt_PIS, source.BRBuyer_IsExempt_COFINS, source.BRBuyer_IsExempt_CSLL, source.Description, source.Email)
;

SET IDENTITY_INSERT dbo.Document OFF

END
GO


CREATE OR ALTER PROCEDURE dbo.sp_Merge_DocumentLine_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.DocumentLine ON

MERGE dbo.DocumentLine AS target
USING (SELECT DocumentLineId, DocumentId, [LineNo], ItemCode, TaxCode, OriginAddressId, DestinationAddressId, Quantity, LineAmount, ExemptAmount, DiscountAmount, DiscountTypeId, TaxableAmount, ExemptNo, RevAccount, Ref1, Ref2, IsSSTP, IsItemTaxable, CustomerUsageType, Description, Sourcing, GoodsServiceCode, TaxEngine, BoundaryOverrideId, TweEntityUseCode, Tax, ExemptCertId, TaxCodeId, TaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, TaxDate, ReportingDate, AccountingMethodId, TaxIncluded, IsExempt, BusinessIdentificationNo, UnitOfMeasurement, StateSstNexusTypeId FROM dbo.DocumentLine_Temp_Shard_1) 
	AS source (DocumentLineId, DocumentId, [LineNo], ItemCode, TaxCode, OriginAddressId, DestinationAddressId, Quantity, LineAmount, ExemptAmount, DiscountAmount, DiscountTypeId, TaxableAmount, ExemptNo, RevAccount, Ref1, Ref2, IsSSTP, IsItemTaxable, CustomerUsageType, Description, Sourcing, GoodsServiceCode, TaxEngine, BoundaryOverrideId, TweEntityUseCode, Tax, ExemptCertId, TaxCodeId, TaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, TaxDate, ReportingDate, AccountingMethodId, TaxIncluded, IsExempt, BusinessIdentificationNo, UnitOfMeasurement, StateSstNexusTypeId)
ON (target.DocumentLineId = source.DocumentLineId)
WHEN MATCHED THEN
UPDATE SET DocumentId = source.DocumentId,
	[LineNo] = source.[LineNo],
	ItemCode = source.ItemCode,
	TaxCode = source.TaxCode,
	OriginAddressId = source.OriginAddressId,
	DestinationAddressId = source.DestinationAddressId,
	Quantity = source.Quantity,
	LineAmount = source.LineAmount,
	ExemptAmount = source.ExemptAmount,
	DiscountAmount = source.DiscountAmount,
	DiscountTypeId = source.DiscountTypeId,
	TaxableAmount = source.TaxableAmount,
	ExemptNo = source.ExemptNo,
	RevAccount = source.RevAccount,
	Ref1 = source.Ref1,
	Ref2 = source.Ref2,
	IsSSTP = source.IsSSTP,
	IsItemTaxable = source.IsItemTaxable,
	CustomerUsageType = source.CustomerUsageType,
	Description = source.Description,
	Sourcing = source.Sourcing,
	GoodsServiceCode = source.GoodsServiceCode,
	TaxEngine = source.TaxEngine,
	BoundaryOverrideId = source.BoundaryOverrideId,
	TweEntityUseCode = source.TweEntityUseCode,
	Tax = source.Tax,
	ExemptCertId = source.ExemptCertId,
	TaxCodeId = source.TaxCodeId,
	TaxCalculated = source.TaxCalculated,
	TaxOverrideAmount = source.TaxOverrideAmount,
	TaxOverrideTypeId = source.TaxOverrideTypeId,
	TaxOverrideReason = source.TaxOverrideReason,
	TaxDate = source.TaxDate,
	ReportingDate = source.ReportingDate,
	AccountingMethodId = source.AccountingMethodId,
	TaxIncluded = source.TaxIncluded,
	IsExempt = source.IsExempt,
	BusinessIdentificationNo = source.BusinessIdentificationNo,
	UnitOfMeasurement = source.UnitOfMeasurement,
	StateSstNexusTypeId = source.StateSstNexusTypeId
WHEN NOT MATCHED THEN
INSERT (DocumentLineId, DocumentId, [LineNo], ItemCode, TaxCode, OriginAddressId, DestinationAddressId, Quantity, LineAmount, ExemptAmount, DiscountAmount, DiscountTypeId, TaxableAmount, ExemptNo, RevAccount, Ref1, Ref2, IsSSTP, IsItemTaxable, CustomerUsageType, Description, Sourcing, GoodsServiceCode, TaxEngine, BoundaryOverrideId, TweEntityUseCode, Tax, ExemptCertId, TaxCodeId, TaxCalculated, TaxOverrideAmount, TaxOverrideTypeId, TaxOverrideReason, TaxDate, ReportingDate, AccountingMethodId, TaxIncluded, IsExempt, BusinessIdentificationNo, UnitOfMeasurement, StateSstNexusTypeId)
VALUES (source.DocumentLineId, source.DocumentId, source.[LineNo], source.ItemCode, source.TaxCode, source.OriginAddressId, source.DestinationAddressId, source.Quantity, source.LineAmount, source.ExemptAmount, source.DiscountAmount, source.DiscountTypeId, source.TaxableAmount, source.ExemptNo, source.RevAccount, source.Ref1, source.Ref2, source.IsSSTP, source.IsItemTaxable, source.CustomerUsageType, source.Description, source.Sourcing, source.GoodsServiceCode, source.TaxEngine, source.BoundaryOverrideId, source.TweEntityUseCode, source.Tax, source.ExemptCertId, source.TaxCodeId, source.TaxCalculated, source.TaxOverrideAmount, source.TaxOverrideTypeId, source.TaxOverrideReason, source.TaxDate, source.ReportingDate, source.AccountingMethodId, source.TaxIncluded, source.IsExempt, source.BusinessIdentificationNo, source.UnitOfMeasurement, source.StateSstNexusTypeId)
;

SET IDENTITY_INSERT dbo.DocumentLine OFF

END
GO





CREATE OR ALTER PROCEDURE dbo.sp_Merge_DocumentLineDetail_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.DocumentLineDetail ON

MERGE dbo.DocumentLineDetail AS target
USING (SELECT DocumentLineDetailId, DocumentLineId, JurisTypeId, SERCode, JurisCode, StateFIPS, TaxableAmount, Rate, Tax, Sourcing, TaxTypeId, ExemptAmount, ExemptReasonId, Region, InState, NonTaxableAmount, RateSourceId, RateRuleId, NonTaxableTypeId, NonTaxableRuleId, CountyFIPS, AddressId, JurisName, StateAssignedNo, Country, JurisdictionId, TaxName, TaxAuthorityTypeId, TaxRegionId, TaxCalculated, TaxOverride, SignatureCode, RateTypeId, DocumentId, TaxableUnits, NonTaxableUnits, ExemptUnits, UnitOfBasisId, ReturnsRateID, ReturnsDeductionID, ReturnsTaxTypeID, IsNonPassThru FROM dbo.DocumentLineDetail_Temp_Shard_1) 
	AS source (DocumentLineDetailId, DocumentLineId, JurisTypeId, SERCode, JurisCode, StateFIPS, TaxableAmount, Rate, Tax, Sourcing, TaxTypeId, ExemptAmount, ExemptReasonId, Region, InState, NonTaxableAmount, RateSourceId, RateRuleId, NonTaxableTypeId, NonTaxableRuleId, CountyFIPS, AddressId, JurisName, StateAssignedNo, Country, JurisdictionId, TaxName, TaxAuthorityTypeId, TaxRegionId, TaxCalculated, TaxOverride, SignatureCode, RateTypeId, DocumentId, TaxableUnits, NonTaxableUnits, ExemptUnits, UnitOfBasisId, ReturnsRateID, ReturnsDeductionID, ReturnsTaxTypeID, IsNonPassThru)
ON (target.DocumentLineDetailId = source.DocumentLineDetailId)
WHEN MATCHED THEN
UPDATE SET DocumentLineId = source.DocumentLineId,
	JurisTypeId = source.JurisTypeId,
	SERCode = source.SERCode,
	JurisCode = source.JurisCode,
	StateFIPS = source.StateFIPS,
	TaxableAmount = source.TaxableAmount,
	Rate = source.Rate,
	Tax = source.Tax,
	Sourcing = source.Sourcing,
	TaxTypeId = source.TaxTypeId,
	ExemptAmount = source.ExemptAmount,
	ExemptReasonId = source.ExemptReasonId,
	Region = source.Region,
	InState = source.InState,
	NonTaxableAmount = source.NonTaxableAmount,
	RateSourceId = source.RateSourceId,
	RateRuleId = source.RateRuleId,
	NonTaxableTypeId = source.NonTaxableTypeId,
	NonTaxableRuleId = source.NonTaxableRuleId,
	CountyFIPS = source.CountyFIPS,
	AddressId = source.AddressId,
	JurisName = source.JurisName,
	StateAssignedNo = source.StateAssignedNo,
	Country = source.Country,
	JurisdictionId = source.JurisdictionId,
	TaxName = source.TaxName,
	TaxAuthorityTypeId = source.TaxAuthorityTypeId,
	TaxRegionId = source.TaxRegionId,
	TaxCalculated = source.TaxCalculated,
	TaxOverride = source.TaxOverride,
	SignatureCode = source.SignatureCode,
	RateTypeId = source.RateTypeId,
	DocumentId = source.DocumentId,
	TaxableUnits = source.TaxableUnits,
	NonTaxableUnits = source.NonTaxableUnits,
	ExemptUnits = source.ExemptUnits,
	UnitOfBasisId = source.UnitOfBasisId,
	ReturnsRateID = source.ReturnsRateID,
	ReturnsDeductionID = source.ReturnsDeductionID,
	ReturnsTaxTypeID = source.ReturnsTaxTypeID,
	IsNonPassThru = source.IsNonPassThru
WHEN NOT MATCHED THEN
INSERT (DocumentLineDetailId, DocumentLineId, JurisTypeId, SERCode, JurisCode, StateFIPS, TaxableAmount, Rate, Tax, Sourcing, TaxTypeId, ExemptAmount, ExemptReasonId, Region, InState, NonTaxableAmount, RateSourceId, RateRuleId, NonTaxableTypeId, NonTaxableRuleId, CountyFIPS, AddressId, JurisName, StateAssignedNo, Country, JurisdictionId, TaxName, TaxAuthorityTypeId, TaxRegionId, TaxCalculated, TaxOverride, SignatureCode, RateTypeId, DocumentId, TaxableUnits, NonTaxableUnits, ExemptUnits, UnitOfBasisId, ReturnsRateID, ReturnsDeductionID, ReturnsTaxTypeID, IsNonPassThru)
VALUES (source.DocumentLineDetailId, source.DocumentLineId, source.JurisTypeId, source.SERCode, source.JurisCode, source.StateFIPS, source.TaxableAmount, source.Rate, source.Tax, source.Sourcing, source.TaxTypeId, source.ExemptAmount, source.ExemptReasonId, source.Region, source.InState, source.NonTaxableAmount, source.RateSourceId, source.RateRuleId, source.NonTaxableTypeId, source.NonTaxableRuleId, source.CountyFIPS, source.AddressId, source.JurisName, source.StateAssignedNo, source.Country, source.JurisdictionId, source.TaxName, source.TaxAuthorityTypeId, source.TaxRegionId, source.TaxCalculated, source.TaxOverride, source.SignatureCode, source.RateTypeId, source.DocumentId, source.TaxableUnits, source.NonTaxableUnits, source.ExemptUnits, source.UnitOfBasisId, source.ReturnsRateID, source.ReturnsDeductionID, source.ReturnsTaxTypeID, source.IsNonPassThru)
;

SET IDENTITY_INSERT dbo.DocumentLineDetail OFF

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_DocumentParameterBag_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.DocumentParameterBag ON

MERGE dbo.DocumentParameterBag AS target
USING (SELECT DocumentParameterBagId, DocumentId, Name, Value, UOMId, UOMIdSystem, ValueSystem FROM dbo.DocumentParameterBag_Temp_Shard_1) 
	AS source (DocumentParameterBagId, DocumentId, Name, Value, UOMId, UOMIdSystem, ValueSystem)
ON (target.DocumentParameterBagId = source.DocumentParameterBagId)
WHEN MATCHED THEN
UPDATE SET DocumentId = source.DocumentId,
	Name = source.Name,
	Value = source.Value,
	UOMId = source.UOMId,
	UOMIdSystem = source.UOMIdSystem,
	ValueSystem = source.ValueSystem
WHEN NOT MATCHED THEN
INSERT (DocumentParameterBagId, DocumentId, Name, Value, UOMId, UOMIdSystem, ValueSystem)
VALUES (source.DocumentParameterBagId, source.DocumentId, source.Name, source.Value, source.UOMId, source.UOMIdSystem, source.ValueSystem)
;

SET IDENTITY_INSERT dbo.DocumentParameterBag OFF

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_Account_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.Account ON

MERGE dbo.Account AS target
USING (SELECT AccountId, LicenseKey, Salt, AccountName, SiteId, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, AccountStatusId, CRMId, CRMIdSTR FROM dbo.Account_Temp_Shard_1) 
	AS source (AccountId, LicenseKey, Salt, AccountName, SiteId, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, AccountStatusId, CRMId, CRMIdSTR)
ON (target.AccountId = source.AccountId)
WHEN MATCHED THEN
UPDATE SET LicenseKey = source.LicenseKey,
	Salt = source.Salt,
	AccountName = source.AccountName,
	SiteId = source.SiteId,
	EffDate = source.EffDate,
	EndDate = source.EndDate,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	AccountStatusId = source.AccountStatusId,
	CRMId = source.CRMId,
	CRMIdSTR = source.CRMIdSTR
WHEN NOT MATCHED THEN
INSERT (AccountId, LicenseKey, Salt, AccountName, SiteId, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, AccountStatusId, CRMId, CRMIdSTR)
VALUES (source.AccountId, source.LicenseKey, source.Salt, source.AccountName, source.SiteId, source.EffDate, source.EndDate, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.AccountStatusId, source.CRMId, source.CRMIdSTR)
;

SET IDENTITY_INSERT dbo.Account OFF

END
GO


CREATE OR ALTER PROCEDURE dbo.sp_Merge_TaxServiceConfig_From_Temp_Shard_1
AS
BEGIN

MERGE dbo.TaxServiceConfig AS target
USING (SELECT AccountId, RequireOriginAddress, RequireMappedItemCode, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EcmsEnabled, EcmsCertUse, EcmsCompleteCertsRequired, EcmsOverrideCode, EcmsSstCertsRequired, MaxLines, EcmsCertUseCa, IsJaasDisabled, SSTPolicyOverrideDate, ItemDescPolicyOverrideDate, UseIsSellerImporterOfRecordFromNexus FROM dbo.TaxServiceConfig_Temp_Shard_1) 
	AS source (AccountId, RequireOriginAddress, RequireMappedItemCode, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EcmsEnabled, EcmsCertUse, EcmsCompleteCertsRequired, EcmsOverrideCode, EcmsSstCertsRequired, MaxLines, EcmsCertUseCa, IsJaasDisabled, SSTPolicyOverrideDate, ItemDescPolicyOverrideDate, UseIsSellerImporterOfRecordFromNexus)
ON (target.AccountId = source.AccountId)
WHEN MATCHED THEN
UPDATE SET RequireOriginAddress = source.RequireOriginAddress,
	RequireMappedItemCode = source.RequireMappedItemCode,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	EcmsEnabled = source.EcmsEnabled,
	EcmsCertUse = source.EcmsCertUse,
	EcmsCompleteCertsRequired = source.EcmsCompleteCertsRequired,
	EcmsOverrideCode = source.EcmsOverrideCode,
	EcmsSstCertsRequired = source.EcmsSstCertsRequired,
	MaxLines = source.MaxLines,
	EcmsCertUseCa = source.EcmsCertUseCa,
	IsJaasDisabled = source.IsJaasDisabled,
	SSTPolicyOverrideDate = source.SSTPolicyOverrideDate,
	ItemDescPolicyOverrideDate = source.ItemDescPolicyOverrideDate,
	UseIsSellerImporterOfRecordFromNexus = source.UseIsSellerImporterOfRecordFromNexus
WHEN NOT MATCHED THEN
INSERT (AccountId, RequireOriginAddress, RequireMappedItemCode, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EcmsEnabled, EcmsCertUse, EcmsCompleteCertsRequired, EcmsOverrideCode, EcmsSstCertsRequired, MaxLines, EcmsCertUseCa, IsJaasDisabled, SSTPolicyOverrideDate, ItemDescPolicyOverrideDate, UseIsSellerImporterOfRecordFromNexus)
VALUES (source.AccountId, source.RequireOriginAddress, source.RequireMappedItemCode, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.EcmsEnabled, source.EcmsCertUse, source.EcmsCompleteCertsRequired, source.EcmsOverrideCode, source.EcmsSstCertsRequired, source.MaxLines, source.EcmsCertUseCa, source.IsJaasDisabled, source.SSTPolicyOverrideDate, source.ItemDescPolicyOverrideDate, source.UseIsSellerImporterOfRecordFromNexus)
;

END
GO



CREATE OR ALTER PROCEDURE dbo.sp_Merge_AddressServiceConfig_From_Temp_Shard_1
AS
BEGIN

MERGE dbo.AddressServiceConfig AS target
USING (SELECT AccountId, IsUpperCase, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, IsJaasDisabled FROM dbo.AddressServiceConfig_Temp_Shard_1) 
	AS source (AccountId, IsUpperCase, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, IsJaasDisabled)
ON (target.AccountId = source.AccountId)
WHEN MATCHED THEN
UPDATE SET IsUpperCase = source.IsUpperCase,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	IsJaasDisabled = source.IsJaasDisabled
WHEN NOT MATCHED THEN
INSERT (AccountId, IsUpperCase, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, IsJaasDisabled)
VALUES (source.AccountId, source.IsUpperCase, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.IsJaasDisabled)
;

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_DocumentCodeChangeList_From_Temp_Shard_1
AS
BEGIN

MERGE dbo.DocumentCodeChangeList AS target
USING (SELECT AccountId, DocumentCode, Committed, ModifiedDate FROM dbo.DocumentCodeChangeList_Temp_Shard_1) 
	AS source (AccountId, DocumentCode, Committed, ModifiedDate)
ON (target.AccountId = source.AccountId AND target.DocumentCode = source.DocumentCode AND target.Committed = source.Committed)
WHEN MATCHED THEN
UPDATE SET ModifiedDate = source.ModifiedDate
WHEN NOT MATCHED THEN
INSERT (AccountId, DocumentCode, Committed, ModifiedDate)
VALUES (source.AccountId, source.DocumentCode, source.Committed, source.ModifiedDate)
;

END
GO






CREATE OR ALTER PROCEDURE dbo.sp_Merge_BoundaryOverride_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.BoundaryOverride ON

MERGE dbo.BoundaryOverride AS target
USING (SELECT BoundaryOverrideId, AccountId, AddrLo, AddrHi, OddEven, StreetPre, StreetName, StreetSuffix, StreetPost, City, County, ZIP5Lo, ZIP5Hi, ZIP4Lo, ZIP4Hi, StateFIPS, CountyFIPS, PlaceFIPS, PlaceClassCode, StateAssignedCode, Longitude, Latitude, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, RateId, HasSTJ, BoundaryLevel, CountyJurisName, CityJurisName, TaxRegionId, State FROM dbo.BoundaryOverride_Temp_Shard_1) 
	AS source (BoundaryOverrideId, AccountId, AddrLo, AddrHi, OddEven, StreetPre, StreetName, StreetSuffix, StreetPost, City, County, ZIP5Lo, ZIP5Hi, ZIP4Lo, ZIP4Hi, StateFIPS, CountyFIPS, PlaceFIPS, PlaceClassCode, StateAssignedCode, Longitude, Latitude, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, RateId, HasSTJ, BoundaryLevel, CountyJurisName, CityJurisName, TaxRegionId, State)
ON (target.BoundaryOverrideId = source.BoundaryOverrideId)
WHEN MATCHED THEN
UPDATE SET AccountId = source.AccountId,
	AddrLo = source.AddrLo,
	AddrHi = source.AddrHi,
	OddEven = source.OddEven,
	StreetPre = source.StreetPre,
	StreetName = source.StreetName,
	StreetSuffix = source.StreetSuffix,
	StreetPost = source.StreetPost,
	City = source.City,
	County = source.County,
	ZIP5Lo = source.ZIP5Lo,
	ZIP5Hi = source.ZIP5Hi,
	ZIP4Lo = source.ZIP4Lo,
	ZIP4Hi = source.ZIP4Hi,
	StateFIPS = source.StateFIPS,
	CountyFIPS = source.CountyFIPS,
	PlaceFIPS = source.PlaceFIPS,
	PlaceClassCode = source.PlaceClassCode,
	StateAssignedCode = source.StateAssignedCode,
	Longitude = source.Longitude,
	Latitude = source.Latitude,
	EffDate = source.EffDate,
	EndDate = source.EndDate,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	RateId = source.RateId,
	HasSTJ = source.HasSTJ,
	BoundaryLevel = source.BoundaryLevel,
	CountyJurisName = source.CountyJurisName,
	CityJurisName = source.CityJurisName,
	TaxRegionId = source.TaxRegionId,
	State = source.State
WHEN NOT MATCHED THEN
INSERT (BoundaryOverrideId, AccountId, AddrLo, AddrHi, OddEven, StreetPre, StreetName, StreetSuffix, StreetPost, City, County, ZIP5Lo, ZIP5Hi, ZIP4Lo, ZIP4Hi, StateFIPS, CountyFIPS, PlaceFIPS, PlaceClassCode, StateAssignedCode, Longitude, Latitude, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, RateId, HasSTJ, BoundaryLevel, CountyJurisName, CityJurisName, TaxRegionId, State)
VALUES (source.BoundaryOverrideId, source.AccountId, source.AddrLo, source.AddrHi, source.OddEven, source.StreetPre, source.StreetName, source.StreetSuffix, source.StreetPost, source.City, source.County, source.ZIP5Lo, source.ZIP5Hi, source.ZIP4Lo, source.ZIP4Hi, source.StateFIPS, source.CountyFIPS, source.PlaceFIPS, source.PlaceClassCode, source.StateAssignedCode, source.Longitude, source.Latitude, source.EffDate, source.EndDate, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.RateId, source.HasSTJ, source.BoundaryLevel, source.CountyJurisName, source.CityJurisName, source.TaxRegionId, source.State)
;

SET IDENTITY_INSERT dbo.BoundaryOverride OFF

END
GO






CREATE OR ALTER PROCEDURE dbo.sp_Merge_JurisdictionOverride_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.JurisdictionOverride ON

MERGE dbo.JurisdictionOverride AS target
USING (SELECT JurisdictionOverrideId, BoundaryOverrideId, AccountId, Address, City, Region, PostalCode, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EffDate, EndDate FROM dbo.JurisdictionOverride_Temp_Shard_1) 
	AS source (JurisdictionOverrideId, BoundaryOverrideId, AccountId, Address, City, Region, PostalCode, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EffDate, EndDate)
ON (target.JurisdictionOverrideId = source.JurisdictionOverrideId)
WHEN MATCHED THEN
UPDATE SET BoundaryOverrideId = source.BoundaryOverrideId,
	AccountId = source.AccountId,
	Address = source.Address,
	City = source.City,
	Region = source.Region,
	PostalCode = source.PostalCode,
	Description = source.Description,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	EffDate = source.EffDate,
	EndDate = source.EndDate
WHEN NOT MATCHED THEN
INSERT (JurisdictionOverrideId, BoundaryOverrideId, AccountId, Address, City, Region, PostalCode, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EffDate, EndDate)
VALUES (source.JurisdictionOverrideId, source.BoundaryOverrideId, source.AccountId, source.Address, source.City, source.Region, source.PostalCode, source.Description, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.EffDate, source.EndDate)
;

SET IDENTITY_INSERT dbo.JurisdictionOverride OFF

END
GO







CREATE OR ALTER PROCEDURE dbo.sp_Merge_Service_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.Service ON

MERGE dbo.Service AS target
USING (SELECT ServiceId, AccountId, ServiceTypeId, Quantity, CompanyId, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EffDate, EndDate FROM dbo.Service_Temp_Shard_1) 
	AS source (ServiceId, AccountId, ServiceTypeId, Quantity, CompanyId, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EffDate, EndDate)
ON (target.ServiceId = source.ServiceId)
WHEN MATCHED THEN
UPDATE SET AccountId = source.AccountId,
	ServiceTypeId = source.ServiceTypeId,
	Quantity = source.Quantity,
	CompanyId = source.CompanyId,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	EffDate = source.EffDate,
	EndDate = source.EndDate
WHEN NOT MATCHED THEN
INSERT (ServiceId, AccountId, ServiceTypeId, Quantity, CompanyId, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EffDate, EndDate)
VALUES (source.ServiceId, source.AccountId, source.ServiceTypeId, source.Quantity, source.CompanyId, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.EffDate, source.EndDate)
;

SET IDENTITY_INSERT dbo.Service OFF

END
GO





CREATE OR ALTER PROCEDURE dbo.sp_Merge_Subscription_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.Subscription ON

MERGE dbo.Subscription AS target
USING (SELECT SubscriptionId, AccountId, PartnerId, ReferenceCode, SubscriptionTypeId, CountryCode, RegionCode, EffDate, EndDate, CancelDate, IsActive, CreatedDate, ModifiedDate, CreatedUserId, ModifiedUserId FROM dbo.Subscription_Temp_Shard_1) 
	AS source (SubscriptionId, AccountId, PartnerId, ReferenceCode, SubscriptionTypeId, CountryCode, RegionCode, EffDate, EndDate, CancelDate, IsActive, CreatedDate, ModifiedDate, CreatedUserId, ModifiedUserId)
ON (target.SubscriptionId = source.SubscriptionId)
WHEN MATCHED THEN
UPDATE SET AccountId = source.AccountId,
	PartnerId = source.PartnerId,
	ReferenceCode = source.ReferenceCode,
	SubscriptionTypeId = source.SubscriptionTypeId,
	CountryCode = source.CountryCode,
	RegionCode = source.RegionCode,
	EffDate = source.EffDate,
	EndDate = source.EndDate,
	CancelDate = source.CancelDate,
	IsActive = source.IsActive,
	CreatedDate = source.CreatedDate,
	ModifiedDate = source.ModifiedDate,
	CreatedUserId = source.CreatedUserId,
	ModifiedUserId = source.ModifiedUserId
WHEN NOT MATCHED THEN
INSERT (SubscriptionId, AccountId, PartnerId, ReferenceCode, SubscriptionTypeId, CountryCode, RegionCode, EffDate, EndDate, CancelDate, IsActive, CreatedDate, ModifiedDate, CreatedUserId, ModifiedUserId)
VALUES (source.SubscriptionId, source.AccountId, source.PartnerId, source.ReferenceCode, source.SubscriptionTypeId, source.CountryCode, source.RegionCode, source.EffDate, source.EndDate, source.CancelDate, source.IsActive, source.CreatedDate, source.ModifiedDate, source.CreatedUserId, source.ModifiedUserId)
;

SET IDENTITY_INSERT dbo.Subscription OFF

END
GO






CREATE OR ALTER PROCEDURE dbo.sp_Merge_User_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.[User] ON

MERGE dbo.[User] AS target
USING (SELECT UserId, AccountId, UserName, FirstName, LastName, Password, PasswordStatusId, Email, PostalCode, SecurityRoleId, IsActive, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, FailedLoginAttempts, CompanyId, SubjectId FROM dbo.User_Temp_Shard_1) 
	AS source (UserId, AccountId, UserName, FirstName, LastName, Password, PasswordStatusId, Email, PostalCode, SecurityRoleId, IsActive, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, FailedLoginAttempts, CompanyId, SubjectId)
ON (target.[UserId] = source.[UserId])
WHEN MATCHED THEN
UPDATE SET AccountId = source.AccountId,
	UserName = source.UserName,
	FirstName = source.FirstName,
	LastName = source.LastName,
	Password = source.Password,
	PasswordStatusId = source.PasswordStatusId,
	Email = source.Email,
	PostalCode = source.PostalCode,
	SecurityRoleId = source.SecurityRoleId,
	IsActive = source.IsActive,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	FailedLoginAttempts = source.FailedLoginAttempts,
	CompanyId = source.CompanyId,
	SubjectId = source.SubjectId
WHEN NOT MATCHED THEN
INSERT (UserId, AccountId, UserName, FirstName, LastName, Password, PasswordStatusId, Email, PostalCode, SecurityRoleId, IsActive, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, FailedLoginAttempts, CompanyId, SubjectId)
VALUES (source.UserId, source.AccountId, source.UserName, source.FirstName, source.LastName, source.Password, source.PasswordStatusId, source.Email, source.PostalCode, source.SecurityRoleId, source.IsActive, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.FailedLoginAttempts, source.CompanyId, source.SubjectId)
;

SET IDENTITY_INSERT dbo.[User] OFF

END
GO



CREATE OR ALTER PROCEDURE dbo.sp_Merge_Company_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.Company ON

MERGE dbo.Company AS target
USING (SELECT CompanyId, AccountId, ParentId, SSTPID, CompanyCode, CompanyName, IsDefault, DefaultLocationId, IsActive, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, TIN, HasProfile, IsReportingEntity, SSTEffDate, RegalBankId, EntityNo, DefaultCountry, BaseCurrencyCode, RoundingLevelId, CashBasisAccountingEnabled, WarningsEnabled, IsTest, TaxDependancyLevelId, InProgress, BusinessIdentificationNo, VATDeductionRightId, MOSSId, MOSSCountry FROM dbo.Company_Temp_Shard_1) 
	AS source (CompanyId, AccountId, ParentId, SSTPID, CompanyCode, CompanyName, IsDefault, DefaultLocationId, IsActive, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, TIN, HasProfile, IsReportingEntity, SSTEffDate, RegalBankId, EntityNo, DefaultCountry, BaseCurrencyCode, RoundingLevelId, CashBasisAccountingEnabled, WarningsEnabled, IsTest, TaxDependancyLevelId, InProgress, BusinessIdentificationNo, VATDeductionRightId, MOSSId, MOSSCountry)
ON (target.CompanyId = source.CompanyId)
WHEN MATCHED THEN
UPDATE SET AccountId = source.AccountId,
	ParentId = source.ParentId,
	SSTPID = source.SSTPID,
	CompanyCode = source.CompanyCode,
	CompanyName = source.CompanyName,
	IsDefault = source.IsDefault,
	DefaultLocationId = source.DefaultLocationId,
	IsActive = source.IsActive,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	TIN = source.TIN,
	HasProfile = source.HasProfile,
	IsReportingEntity = source.IsReportingEntity,
	SSTEffDate = source.SSTEffDate,
	RegalBankId = source.RegalBankId,
	EntityNo = source.EntityNo,
	DefaultCountry = source.DefaultCountry,
	BaseCurrencyCode = source.BaseCurrencyCode,
	RoundingLevelId = source.RoundingLevelId,
	CashBasisAccountingEnabled = source.CashBasisAccountingEnabled,
	WarningsEnabled = source.WarningsEnabled,
	IsTest = source.IsTest,
	TaxDependancyLevelId = source.TaxDependancyLevelId,
	InProgress = source.InProgress,
	BusinessIdentificationNo = source.BusinessIdentificationNo,
	VATDeductionRightId = source.VATDeductionRightId,
	MOSSId = source.MOSSId,
	MOSSCountry = source.MOSSCountry
WHEN NOT MATCHED THEN
INSERT (CompanyId, AccountId, ParentId, SSTPID, CompanyCode, CompanyName, IsDefault, DefaultLocationId, IsActive, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, TIN, HasProfile, IsReportingEntity, SSTEffDate, RegalBankId, EntityNo, DefaultCountry, BaseCurrencyCode, RoundingLevelId, CashBasisAccountingEnabled, WarningsEnabled, IsTest, TaxDependancyLevelId, InProgress, BusinessIdentificationNo, VATDeductionRightId, MOSSId, MOSSCountry)
VALUES (source.CompanyId, source.AccountId, source.ParentId, source.SSTPID, source.CompanyCode, source.CompanyName, source.IsDefault, source.DefaultLocationId, source.IsActive, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.TIN, source.HasProfile, source.IsReportingEntity, source.SSTEffDate, source.RegalBankId, source.EntityNo, source.DefaultCountry, source.BaseCurrencyCode, source.RoundingLevelId, source.CashBasisAccountingEnabled, source.WarningsEnabled, source.IsTest, source.TaxDependancyLevelId, source.InProgress, source.BusinessIdentificationNo, source.VATDeductionRightId, source.MOSSId, source.MOSSCountry)
;

SET IDENTITY_INSERT dbo.Company OFF

END
GO





CREATE OR ALTER PROCEDURE dbo.sp_Merge_TaxRule_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.TaxRule ON

MERGE dbo.TaxRule AS target
USING (SELECT TaxRuleId, CompanyId, StateFIPS, JurisName, JurisCode, JurisTypeId, TaxCodeId, CustomerUsageType, TaxTypeId, RateTypeId, TaxRuleTypeId, IsAllJuris, Value, Cap, Threshold, Options, EffDate, EndDate, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, State, CountyFIPS, IsSTPro, Country, Sourcing, UnitOfBasisId, AttributeOptions, ReturnsRateID, ReturnsDeductionID, ReturnsTaxTypeID, AttributeApplicability, TaxTypeMappingId, RateTypeTaxTypeMappingId, NonPassThruExpression, CurrencyCode, PreferredProgramId, UOMId, HashKey FROM dbo.TaxRule_Temp_Shard_1) 
	AS source (TaxRuleId, CompanyId, StateFIPS, JurisName, JurisCode, JurisTypeId, TaxCodeId, CustomerUsageType, TaxTypeId, RateTypeId, TaxRuleTypeId, IsAllJuris, Value, Cap, Threshold, Options, EffDate, EndDate, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, State, CountyFIPS, IsSTPro, Country, Sourcing, UnitOfBasisId, AttributeOptions, ReturnsRateID, ReturnsDeductionID, ReturnsTaxTypeID, AttributeApplicability, TaxTypeMappingId, RateTypeTaxTypeMappingId, NonPassThruExpression, CurrencyCode, PreferredProgramId, UOMId, HashKey)
ON (target.TaxRuleId = source.TaxRuleId)
WHEN MATCHED THEN
UPDATE SET CompanyId = source.CompanyId,
	StateFIPS = source.StateFIPS,
	JurisName = source.JurisName,
	JurisCode = source.JurisCode,
	JurisTypeId = source.JurisTypeId,
	TaxCodeId = source.TaxCodeId,
	CustomerUsageType = source.CustomerUsageType,
	TaxTypeId = source.TaxTypeId,
	RateTypeId = source.RateTypeId,
	TaxRuleTypeId = source.TaxRuleTypeId,
	IsAllJuris = source.IsAllJuris,
	Value = source.Value,
	Cap = source.Cap,
	Threshold = source.Threshold,
	Options = source.Options,
	EffDate = source.EffDate,
	EndDate = source.EndDate,
	Description = source.Description,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	State = source.State,
	CountyFIPS = source.CountyFIPS,
	IsSTPro = source.IsSTPro,
	Country = source.Country,
	Sourcing = source.Sourcing,
	UnitOfBasisId = source.UnitOfBasisId,
	AttributeOptions = source.AttributeOptions,
	ReturnsRateID = source.ReturnsRateID,
	ReturnsDeductionID = source.ReturnsDeductionID,
	ReturnsTaxTypeID = source.ReturnsTaxTypeID,
	AttributeApplicability = source.AttributeApplicability,
	TaxTypeMappingId = source.TaxTypeMappingId,
	RateTypeTaxTypeMappingId = source.RateTypeTaxTypeMappingId,
	NonPassThruExpression = source.NonPassThruExpression,
	CurrencyCode = source.CurrencyCode,
	PreferredProgramId = source.PreferredProgramId,
	UOMId = source.UOMId,
	HashKey = source.HashKey
WHEN NOT MATCHED THEN
INSERT (TaxRuleId, CompanyId, StateFIPS, JurisName, JurisCode, JurisTypeId, TaxCodeId, CustomerUsageType, TaxTypeId, RateTypeId, TaxRuleTypeId, IsAllJuris, Value, Cap, Threshold, Options, EffDate, EndDate, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, State, CountyFIPS, IsSTPro, Country, Sourcing, UnitOfBasisId, AttributeOptions, ReturnsRateID, ReturnsDeductionID, ReturnsTaxTypeID, AttributeApplicability, TaxTypeMappingId, RateTypeTaxTypeMappingId, NonPassThruExpression, CurrencyCode, PreferredProgramId, UOMId, HashKey)
VALUES (source.TaxRuleId, source.CompanyId, source.StateFIPS, source.JurisName, source.JurisCode, source.JurisTypeId, source.TaxCodeId, source.CustomerUsageType, source.TaxTypeId, source.RateTypeId, source.TaxRuleTypeId, source.IsAllJuris, source.Value, source.Cap, source.Threshold, source.Options, source.EffDate, source.EndDate, source.Description, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.State, source.CountyFIPS, source.IsSTPro, source.Country, source.Sourcing, source.UnitOfBasisId, source.AttributeOptions, source.ReturnsRateID, source.ReturnsDeductionID, source.ReturnsTaxTypeID, source.AttributeApplicability, source.TaxTypeMappingId, source.RateTypeTaxTypeMappingId, source.NonPassThruExpression, source.CurrencyCode, source.PreferredProgramId, source.UOMId, source.HashKey)
;

SET IDENTITY_INSERT dbo.TaxRule OFF

END
GO





CREATE OR ALTER PROCEDURE dbo.sp_Merge_CompanyContact_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.CompanyContact ON

MERGE dbo.CompanyContact AS target
USING (SELECT CompanyContactId, CompanyId, CompanyContactCode, ContactRoleTypeId, FirstName, MiddleName, LastName, Title, Line1, Line2, Line3, City, Region, PostalCode, Country, Email, Phone, Phone2, Fax, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate FROM dbo.CompanyContact_Temp_Shard_1) 
	AS source (CompanyContactId, CompanyId, CompanyContactCode, ContactRoleTypeId, FirstName, MiddleName, LastName, Title, Line1, Line2, Line3, City, Region, PostalCode, Country, Email, Phone, Phone2, Fax, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
ON (target.CompanyContactId = source.CompanyContactId)
WHEN MATCHED THEN
UPDATE SET CompanyId = source.CompanyId,
	CompanyContactCode = source.CompanyContactCode,
	ContactRoleTypeId = source.ContactRoleTypeId,
	FirstName = source.FirstName,
	MiddleName = source.MiddleName,
	LastName = source.LastName,
	Title = source.Title,
	Line1 = source.Line1,
	Line2 = source.Line2,
	Line3 = source.Line3,
	City = source.City,
	Region = source.Region,
	PostalCode = source.PostalCode,
	Country = source.Country,
	Email = source.Email,
	Phone = source.Phone,
	Phone2 = source.Phone2,
	Fax = source.Fax,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate
WHEN NOT MATCHED THEN
INSERT (CompanyContactId, CompanyId, CompanyContactCode, ContactRoleTypeId, FirstName, MiddleName, LastName, Title, Line1, Line2, Line3, City, Region, PostalCode, Country, Email, Phone, Phone2, Fax, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (source.CompanyContactId, source.CompanyId, source.CompanyContactCode, source.ContactRoleTypeId, source.FirstName, source.MiddleName, source.LastName, source.Title, source.Line1, source.Line2, source.Line3, source.City, source.Region, source.PostalCode, source.Country, source.Email, source.Phone, source.Phone2, source.Fax, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate)
;

SET IDENTITY_INSERT dbo.CompanyContact OFF

END
GO



CREATE OR ALTER PROCEDURE dbo.sp_Merge_CompanyLocation_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.CompanyLocation ON

MERGE dbo.CompanyLocation AS target
USING (SELECT CompanyLocationId, CompanyId, LocationCode, Description, AddressTypeId, AddressCategoryId, Line1, Line2, Line3, City, Region, PostalCode, Country, StateAssignedCode, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, IsDefault, County, IsRegistered, DBAName, OutletName, StartDate, EndDate, LastTransactionDate, RegisteredDate FROM dbo.CompanyLocation_Temp_Shard_1) 
	AS source (CompanyLocationId, CompanyId, LocationCode, Description, AddressTypeId, AddressCategoryId, Line1, Line2, Line3, City, Region, PostalCode, Country, StateAssignedCode, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, IsDefault, County, IsRegistered, DBAName, OutletName, StartDate, EndDate, LastTransactionDate, RegisteredDate)
ON (target.CompanyLocationId = source.CompanyLocationId)
WHEN MATCHED THEN
UPDATE SET CompanyId = source.CompanyId,
	LocationCode = source.LocationCode,
	Description = source.Description,
	AddressTypeId = source.AddressTypeId,
	AddressCategoryId = source.AddressCategoryId,
	Line1 = source.Line1,
	Line2 = source.Line2,
	Line3 = source.Line3,
	City = source.City,
	Region = source.Region,
	PostalCode = source.PostalCode,
	Country = source.Country,
	StateAssignedCode = source.StateAssignedCode,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	IsDefault = source.IsDefault,
	County = source.County,
	IsRegistered = source.IsRegistered,
	DBAName = source.DBAName,
	OutletName = source.OutletName,
	StartDate = source.StartDate,
	EndDate = source.EndDate,
	LastTransactionDate = source.LastTransactionDate,
	RegisteredDate = source.RegisteredDate
WHEN NOT MATCHED THEN
INSERT (CompanyLocationId, CompanyId, LocationCode, Description, AddressTypeId, AddressCategoryId, Line1, Line2, Line3, City, Region, PostalCode, Country, StateAssignedCode, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, IsDefault, County, IsRegistered, DBAName, OutletName, StartDate, EndDate, LastTransactionDate, RegisteredDate)
VALUES (source.CompanyLocationId, source.CompanyId, source.LocationCode, source.Description, source.AddressTypeId, source.AddressCategoryId, source.Line1, source.Line2, source.Line3, source.City, source.Region, source.PostalCode, source.Country, source.StateAssignedCode, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.IsDefault, source.County, source.IsRegistered, source.DBAName, source.OutletName, source.StartDate, source.EndDate, source.LastTransactionDate, source.RegisteredDate)
;

SET IDENTITY_INSERT dbo.CompanyLocation OFF

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_ExemptCert_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.ExemptCert ON

MERGE dbo.ExemptCert AS target
USING (SELECT ExemptCertId, CompanyId, CustomerCode, CustomerName, Address1, Address2, Address3, City, Region, PostalCode, Country, ExemptCertTypeId, DocumentRefNo, BusinessTypeId, BusinessTypeOtherDescription, ExemptReasonId, ExemptReasonOtherDescription, EffDate, RegionsApplicable, ExemptCertStatusId, CreatedDate, LastTransactionDate, ExpiryDate, CreatedUserId, ModifiedDate, ModifiedUserId, CountryIssued, AvaCertId, ExemptCertReviewStatusId FROM dbo.ExemptCert_Temp_Shard_1) 
	AS source (ExemptCertId, CompanyId, CustomerCode, CustomerName, Address1, Address2, Address3, City, Region, PostalCode, Country, ExemptCertTypeId, DocumentRefNo, BusinessTypeId, BusinessTypeOtherDescription, ExemptReasonId, ExemptReasonOtherDescription, EffDate, RegionsApplicable, ExemptCertStatusId, CreatedDate, LastTransactionDate, ExpiryDate, CreatedUserId, ModifiedDate, ModifiedUserId, CountryIssued, AvaCertId, ExemptCertReviewStatusId)
ON (target.ExemptCertId = source.ExemptCertId)
WHEN MATCHED THEN
UPDATE SET CompanyId = source.CompanyId,
	CustomerCode = source.CustomerCode,
	CustomerName = source.CustomerName,
	Address1 = source.Address1,
	Address2 = source.Address2,
	Address3 = source.Address3,
	City = source.City,
	Region = source.Region,
	PostalCode = source.PostalCode,
	Country = source.Country,
	ExemptCertTypeId = source.ExemptCertTypeId,
	DocumentRefNo = source.DocumentRefNo,
	BusinessTypeId = source.BusinessTypeId,
	BusinessTypeOtherDescription = source.BusinessTypeOtherDescription,
	ExemptReasonId = source.ExemptReasonId,
	ExemptReasonOtherDescription = source.ExemptReasonOtherDescription,
	EffDate = source.EffDate,
	RegionsApplicable = source.RegionsApplicable,
	ExemptCertStatusId = source.ExemptCertStatusId,
	CreatedDate = source.CreatedDate,
	LastTransactionDate = source.LastTransactionDate,
	ExpiryDate = source.ExpiryDate,
	CreatedUserId = source.CreatedUserId,
	ModifiedDate = source.ModifiedDate,
	ModifiedUserId = source.ModifiedUserId,
	CountryIssued = source.CountryIssued,
	AvaCertId = source.AvaCertId,
	ExemptCertReviewStatusId = source.ExemptCertReviewStatusId
WHEN NOT MATCHED THEN
INSERT (ExemptCertId, CompanyId, CustomerCode, CustomerName, Address1, Address2, Address3, City, Region, PostalCode, Country, ExemptCertTypeId, DocumentRefNo, BusinessTypeId, BusinessTypeOtherDescription, ExemptReasonId, ExemptReasonOtherDescription, EffDate, RegionsApplicable, ExemptCertStatusId, CreatedDate, LastTransactionDate, ExpiryDate, CreatedUserId, ModifiedDate, ModifiedUserId, CountryIssued, AvaCertId, ExemptCertReviewStatusId)
VALUES (source.ExemptCertId, source.CompanyId, source.CustomerCode, source.CustomerName, source.Address1, source.Address2, source.Address3, source.City, source.Region, source.PostalCode, source.Country, source.ExemptCertTypeId, source.DocumentRefNo, source.BusinessTypeId, source.BusinessTypeOtherDescription, source.ExemptReasonId, source.ExemptReasonOtherDescription, source.EffDate, source.RegionsApplicable, source.ExemptCertStatusId, source.CreatedDate, source.LastTransactionDate, source.ExpiryDate, source.CreatedUserId, source.ModifiedDate, source.ModifiedUserId, source.CountryIssued, source.AvaCertId, source.ExemptCertReviewStatusId)
;

SET IDENTITY_INSERT dbo.ExemptCert OFF

END
GO


CREATE OR ALTER PROCEDURE dbo.sp_Merge_Nexus_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.Nexus ON

MERGE dbo.Nexus AS target
USING (SELECT NexusId, CompanyId, State, JurisTypeId, JurisCode, JurisName, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, ShortName, SignatureCode, StateAssignedNo, NexusTypeId, Country, Sourcing, AccountingMethodId, HasLocalNexus, LocalNexusTypeId, HasPermanentEstablishment, TaxId, NexusTaxTypeGroupIdSK, VATNumberTypeId, VATOptionsId, MOSSId, IsSellerImporterOfRecord FROM dbo.Nexus_Temp_Shard_1) 
	AS source (NexusId, CompanyId, State, JurisTypeId, JurisCode, JurisName, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, ShortName, SignatureCode, StateAssignedNo, NexusTypeId, Country, Sourcing, AccountingMethodId, HasLocalNexus, LocalNexusTypeId, HasPermanentEstablishment, TaxId, NexusTaxTypeGroupIdSK, VATNumberTypeId, VATOptionsId, MOSSId, IsSellerImporterOfRecord)

ON (target.NexusId = source.NexusId)
WHEN MATCHED THEN
UPDATE SET CompanyId = source.CompanyId,
	State = source.State,
	JurisTypeId = source.JurisTypeId,
	JurisCode = source.JurisCode,
	JurisName = source.JurisName,
	EffDate = source.EffDate,
	EndDate = source.EndDate,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	ShortName = source.ShortName,
	SignatureCode = source.SignatureCode,
	StateAssignedNo = source.StateAssignedNo,
	NexusTypeId = source.NexusTypeId,
	Country = source.Country,
	Sourcing = source.Sourcing,
	AccountingMethodId = source.AccountingMethodId,
	HasLocalNexus = source.HasLocalNexus,
	LocalNexusTypeId = source.LocalNexusTypeId,
	HasPermanentEstablishment = source.HasPermanentEstablishment,
	TaxId = source.TaxId,
	NexusTaxTypeGroupIdSK = source.NexusTaxTypeGroupIdSK,
	VATNumberTypeId = source.VATNumberTypeId,
	VATOptionsId = source.VATOptionsId,
	MOSSId = source.MOSSId,
	IsSellerImporterOfRecord = source.IsSellerImporterOfRecord
WHEN NOT MATCHED THEN
INSERT (NexusId, CompanyId, State, JurisTypeId, JurisCode, JurisName, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, ShortName, SignatureCode, StateAssignedNo, NexusTypeId, Country, Sourcing, AccountingMethodId, HasLocalNexus, LocalNexusTypeId, HasPermanentEstablishment, TaxId, NexusTaxTypeGroupIdSK, VATNumberTypeId, VATOptionsId, MOSSId, IsSellerImporterOfRecord)
VALUES (source.NexusId, source.CompanyId, source.State, source.JurisTypeId, source.JurisCode, source.JurisName, source.EffDate, source.EndDate, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.ShortName, source.SignatureCode, source.StateAssignedNo, source.NexusTypeId, source.Country, source.Sourcing, source.AccountingMethodId, source.HasLocalNexus, source.LocalNexusTypeId, source.HasPermanentEstablishment, source.TaxId, source.NexusTaxTypeGroupIdSK, source.VATNumberTypeId, source.VATOptionsId, source.MOSSId, source.IsSellerImporterOfRecord)
;

SET IDENTITY_INSERT dbo.Nexus OFF

END
GO



CREATE OR ALTER PROCEDURE dbo.sp_Merge_TaxCode_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.TaxCode ON

MERGE dbo.TaxCode AS target
USING (SELECT TaxCodeId, TaxCode, TaxCodeTypeId, CompanyId, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, ParentTaxCode, IsPhysical, GoodsServiceCode, EntityUseCode, IsActive, IsSSTCertified FROM dbo.TaxCode_Temp_Shard_1) 
	AS source (TaxCodeId, TaxCode, TaxCodeTypeId, CompanyId, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, ParentTaxCode, IsPhysical, GoodsServiceCode, EntityUseCode, IsActive, IsSSTCertified)
ON (target.TaxCodeId = source.TaxCodeId)
WHEN MATCHED THEN
UPDATE SET TaxCode = source.TaxCode,
	TaxCodeTypeId = source.TaxCodeTypeId,
	CompanyId = source.CompanyId,
	Description = source.Description,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	ParentTaxCode = source.ParentTaxCode,
	IsPhysical = source.IsPhysical,
	GoodsServiceCode = source.GoodsServiceCode,
	EntityUseCode = source.EntityUseCode,
	IsActive = source.IsActive,
	IsSSTCertified = source.IsSSTCertified
WHEN NOT MATCHED THEN
INSERT (TaxCodeId, TaxCode, TaxCodeTypeId, CompanyId, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, ParentTaxCode, IsPhysical, GoodsServiceCode, EntityUseCode, IsActive, IsSSTCertified)
VALUES (source.TaxCodeId, source.TaxCode, source.TaxCodeTypeId, source.CompanyId, source.Description, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.ParentTaxCode, source.IsPhysical, source.GoodsServiceCode, source.EntityUseCode, source.IsActive, source.IsSSTCertified)
;

SET IDENTITY_INSERT dbo.TaxCode OFF

END
GO






CREATE OR ALTER PROCEDURE dbo.sp_Merge_UPCCodeLookup_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.UPCCodeLookup ON

MERGE dbo.UPCCodeLookup AS target
USING (SELECT UPCCodeLookupId, UPCCode, LegacyTaxCode, CompanyId, UPCDescription, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EffDate, EndDate, Usage, IsSystem FROM dbo.UPCCodeLookup_Temp_Shard_1) 
	AS source (UPCCodeLookupId, UPCCode, LegacyTaxCode, CompanyId, UPCDescription, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EffDate, EndDate, Usage, IsSystem)
ON (target.UPCCodeLookupId = source.UPCCodeLookupId)
WHEN MATCHED THEN
UPDATE SET UPCCode = source.UPCCode,
	LegacyTaxCode = source.LegacyTaxCode,
	CompanyId = source.CompanyId,
	UPCDescription = source.UPCDescription,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	EffDate = source.EffDate,
	EndDate = source.EndDate,
	Usage = source.Usage,
	IsSystem = source.IsSystem
WHEN NOT MATCHED THEN
INSERT (UPCCodeLookupId, UPCCode, LegacyTaxCode, CompanyId, UPCDescription, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, EffDate, EndDate, Usage, IsSystem)
VALUES (source.UPCCodeLookupId, source.UPCCode, source.LegacyTaxCode, source.CompanyId, source.UPCDescription, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.EffDate, source.EndDate, source.Usage, source.IsSystem)
;

SET IDENTITY_INSERT dbo.UPCCodeLookup OFF

END
GO



CREATE OR ALTER PROCEDURE dbo.sp_Merge_AvaCertServiceConfig_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.AvaCertServiceConfig ON

MERGE dbo.AvaCertServiceConfig AS target
USING (SELECT AvaCertServiceConfigId, CompanyId, AvaCertServiceStatusId, IsUpdateEnabled, ClientCode, OrgCode, AllowPending, LastUpdate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate FROM dbo.AvaCertServiceConfig_Temp_Shard_1) 
	AS source (AvaCertServiceConfigId, CompanyId, AvaCertServiceStatusId, IsUpdateEnabled, ClientCode, OrgCode, AllowPending, LastUpdate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
ON (target.AvaCertServiceConfigId = source.AvaCertServiceConfigId)
WHEN MATCHED THEN
UPDATE SET CompanyId = source.CompanyId,
	AvaCertServiceStatusId = source.AvaCertServiceStatusId,
	IsUpdateEnabled = source.IsUpdateEnabled,
	ClientCode = source.ClientCode,
	OrgCode = source.OrgCode,
	AllowPending = source.AllowPending,
	LastUpdate = source.LastUpdate,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate
WHEN NOT MATCHED THEN
INSERT (AvaCertServiceConfigId, CompanyId, AvaCertServiceStatusId, IsUpdateEnabled, ClientCode, OrgCode, AllowPending, LastUpdate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (source.AvaCertServiceConfigId, source.CompanyId, source.AvaCertServiceStatusId, source.IsUpdateEnabled, source.ClientCode, source.OrgCode, source.AllowPending, source.LastUpdate, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate)
;

SET IDENTITY_INSERT dbo.AvaCertServiceConfig OFF

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_AvaCommsServiceConfig_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.AvaCommsServiceConfig ON

MERGE dbo.AvaCommsServiceConfig AS target
USING (SELECT AvaCommsConfigId, CompanyId, ClientId, ClientProfileId, Parameters, LastUpdate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate FROM dbo.AvaCommsServiceConfig_Temp_Shard_1) 
	AS source (AvaCommsConfigId, CompanyId, ClientId, ClientProfileId, Parameters, LastUpdate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
ON (target.AvaCommsConfigId = source.AvaCommsConfigId)
WHEN MATCHED THEN
UPDATE SET CompanyId = source.CompanyId,
	ClientId = source.ClientId,
	ClientProfileId = source.ClientProfileId,
	Parameters = source.Parameters,
	LastUpdate = source.LastUpdate,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate
WHEN NOT MATCHED THEN
INSERT (AvaCommsConfigId, CompanyId, ClientId, ClientProfileId, Parameters, LastUpdate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (source.AvaCommsConfigId, source.CompanyId, source.ClientId, source.ClientProfileId, source.Parameters, source.LastUpdate, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate)
;

SET IDENTITY_INSERT dbo.AvaCommsServiceConfig OFF

END
GO






CREATE OR ALTER PROCEDURE dbo.sp_Merge_BRCompanySecurityCertificate_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.BRCompanySecurityCertificate ON

MERGE dbo.BRCompanySecurityCertificate AS target
USING (SELECT CompanySecurityCertificateId, CompanyId, Password, Certificate, ExpirationDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate FROM dbo.BRCompanySecurityCertificate_Temp_Shard_1) 
	AS source (CompanySecurityCertificateId, CompanyId, Password, Certificate, ExpirationDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
ON (target.CompanySecurityCertificateId = source.CompanySecurityCertificateId)
WHEN MATCHED THEN
UPDATE SET CompanyId = source.CompanyId,
	Password = source.Password,
	Certificate = source.Certificate,
	ExpirationDate = source.ExpirationDate,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate
WHEN NOT MATCHED THEN
INSERT (CompanySecurityCertificateId, CompanyId, Password, Certificate, ExpirationDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (source.CompanySecurityCertificateId, source.CompanyId, source.Password, source.Certificate, source.ExpirationDate, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate)
;

SET IDENTITY_INSERT dbo.BRCompanySecurityCertificate OFF

END
GO


CREATE OR ALTER PROCEDURE dbo.sp_Merge_BRTaxRegimeBuyerTypeConfig_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.BRTaxRegimeBuyerTypeConfig ON

MERGE dbo.BRTaxRegimeBuyerTypeConfig AS target
USING (SELECT BRTaxRegimeBuyerTypeConfigId, CompanyId, Country, JurisName, JurisCode, JurisTypeId, TaxRuleTypeId, TaxCodeId, TaxRegime, IsRateCumulative, BuyerType, RateTypeId, Value, EffDate, EndDate, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate FROM dbo.BRTaxRegimeBuyerTypeConfig_Temp_Shard_1) 
	AS source (BRTaxRegimeBuyerTypeConfigId, CompanyId, Country, JurisName, JurisCode, JurisTypeId, TaxRuleTypeId, TaxCodeId, TaxRegime, IsRateCumulative, BuyerType, RateTypeId, Value, EffDate, EndDate, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
ON (target.BRTaxRegimeBuyerTypeConfigId = source.BRTaxRegimeBuyerTypeConfigId)
WHEN MATCHED THEN
UPDATE SET CompanyId = source.CompanyId,
	Country = source.Country,
	JurisName = source.JurisName,
	JurisCode = source.JurisCode,
	JurisTypeId = source.JurisTypeId,
	TaxRuleTypeId = source.TaxRuleTypeId,
	TaxCodeId = source.TaxCodeId,
	TaxRegime = source.TaxRegime,
	IsRateCumulative = source.IsRateCumulative,
	BuyerType = source.BuyerType,
	RateTypeId = source.RateTypeId,
	Value = source.Value,
	EffDate = source.EffDate,
	EndDate = source.EndDate,
	Description = source.Description,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate
WHEN NOT MATCHED THEN
INSERT (BRTaxRegimeBuyerTypeConfigId, CompanyId, Country, JurisName, JurisCode, JurisTypeId, TaxRuleTypeId, TaxCodeId, TaxRegime, IsRateCumulative, BuyerType, RateTypeId, Value, EffDate, EndDate, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (source.BRTaxRegimeBuyerTypeConfigId, source.CompanyId, source.Country, source.JurisName, source.JurisCode, source.JurisTypeId, source.TaxRuleTypeId, source.TaxCodeId, source.TaxRegime, source.IsRateCumulative, source.BuyerType, source.RateTypeId, source.Value, source.EffDate, source.EndDate, source.Description, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate)
;

SET IDENTITY_INSERT dbo.BRTaxRegimeBuyerTypeConfig OFF

END
GO






CREATE OR ALTER PROCEDURE dbo.sp_Merge_CompanyTaxForm_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.CompanyTaxForm ON

MERGE dbo.CompanyTaxForm AS target
USING (SELECT CompanyTaxFormId, CompanyId, LibraryTaxFormId, FilingStatusId, CloseDate, FinalDate, FilingFrequencyId, ModifiedUserId, TaxFormPDF, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedDate FROM dbo.CompanyTaxForm_Temp_Shard_1) 
	AS source (CompanyTaxFormId, CompanyId, LibraryTaxFormId, FilingStatusId, CloseDate, FinalDate, FilingFrequencyId, ModifiedUserId, TaxFormPDF, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedDate)
ON (target.CompanyTaxFormId = source.CompanyTaxFormId)
WHEN MATCHED THEN
UPDATE SET CompanyId = source.CompanyId,
	LibraryTaxFormId = source.LibraryTaxFormId,
	FilingStatusId = source.FilingStatusId,
	CloseDate = source.CloseDate,
	FinalDate = source.FinalDate,
	FilingFrequencyId = source.FilingFrequencyId,
	ModifiedUserId = source.ModifiedUserId,
	TaxFormPDF = source.TaxFormPDF,
	EffDate = source.EffDate,
	EndDate = source.EndDate,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedDate = source.ModifiedDate
WHEN NOT MATCHED THEN
INSERT (CompanyTaxFormId, CompanyId, LibraryTaxFormId, FilingStatusId, CloseDate, FinalDate, FilingFrequencyId, ModifiedUserId, TaxFormPDF, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedDate)
VALUES (source.CompanyTaxFormId, source.CompanyId, source.LibraryTaxFormId, source.FilingStatusId, source.CloseDate, source.FinalDate, source.FilingFrequencyId, source.ModifiedUserId, source.TaxFormPDF, source.EffDate, source.EndDate, source.CreatedUserId, source.CreatedDate, source.ModifiedDate)
;

SET IDENTITY_INSERT dbo.CompanyTaxForm OFF

END
GO


CREATE OR ALTER PROCEDURE dbo.sp_Merge_Return_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.[Return] ON

MERGE dbo.[Return] AS target
USING (SELECT ReturnId, TransmissionId, CompanyId, ReturnTypeId, ReturnStatusId, BeginDate, EndDate, FiledDate, CreatedDate, ModifiedDate, JurisCode, JurisName, JurisTypeId, Region, ReturnName FROM dbo.Return_Temp_Shard_1) 
	AS source (ReturnId, TransmissionId, CompanyId, ReturnTypeId, ReturnStatusId, BeginDate, EndDate, FiledDate, CreatedDate, ModifiedDate, JurisCode, JurisName, JurisTypeId, Region, ReturnName)
ON (target.ReturnId = source.ReturnId)
WHEN MATCHED THEN
UPDATE SET TransmissionId = source.TransmissionId,
	CompanyId = source.CompanyId,
	ReturnTypeId = source.ReturnTypeId,
	ReturnStatusId = source.ReturnStatusId,
	BeginDate = source.BeginDate,
	EndDate = source.EndDate,
	FiledDate = source.FiledDate,
	CreatedDate = source.CreatedDate,
	ModifiedDate = source.ModifiedDate,
	JurisCode = source.JurisCode,
	JurisName = source.JurisName,
	JurisTypeId = source.JurisTypeId,
	Region = source.Region,
	ReturnName = source.ReturnName
WHEN NOT MATCHED THEN
INSERT (ReturnId, TransmissionId, CompanyId, ReturnTypeId, ReturnStatusId, BeginDate, EndDate, FiledDate, CreatedDate, ModifiedDate, JurisCode, JurisName, JurisTypeId, Region, ReturnName)
VALUES (source.ReturnId, source.TransmissionId, source.CompanyId, source.ReturnTypeId, source.ReturnStatusId, source.BeginDate, source.EndDate, source.FiledDate, source.CreatedDate, source.ModifiedDate, source.JurisCode, source.JurisName, source.JurisTypeId, source.Region, source.ReturnName)
;

SET IDENTITY_INSERT dbo.[Return] OFF

END
GO






CREATE OR ALTER PROCEDURE dbo.sp_Merge_TaxRuleProductDetail_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.TaxRuleProductDetail ON

MERGE dbo.TaxRuleProductDetail AS target
USING (SELECT TaxRuleProductDetailId, TaxRuleId, ProductId, ProductTypeId, EffDate, EndDate, ModifiedDate FROM dbo.TaxRuleProductDetail_Temp_Shard_1) 
	AS source (TaxRuleProductDetailId, TaxRuleId, ProductId, ProductTypeId, EffDate, EndDate, ModifiedDate)
ON (target.TaxRuleProductDetailId = source.TaxRuleProductDetailId)
WHEN MATCHED THEN
UPDATE SET TaxRuleId = source.TaxRuleId,
	ProductId = source.ProductId,
	ProductTypeId = source.ProductTypeId,
	EffDate = source.EffDate,
	EndDate = source.EndDate,
	ModifiedDate = source.ModifiedDate
WHEN NOT MATCHED THEN
INSERT (TaxRuleProductDetailId, TaxRuleId, ProductId, ProductTypeId, EffDate, EndDate, ModifiedDate)
VALUES (source.TaxRuleProductDetailId, source.TaxRuleId, source.ProductId, source.ProductTypeId, source.EffDate, source.EndDate, source.ModifiedDate)
;

SET IDENTITY_INSERT dbo.TaxRuleProductDetail OFF

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_TaxCodeAttribute_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.TaxCodeAttribute ON

MERGE dbo.TaxCodeAttribute AS target
USING (SELECT TaxCodeAttributeId, TaxCodeId, Name, Value, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, UOMId FROM dbo.TaxCodeAttribute_Temp_Shard_1) 
	AS source (TaxCodeAttributeId, TaxCodeId, Name, Value, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, UOMId)
ON (target.TaxCodeAttributeId = source.TaxCodeAttributeId)
WHEN MATCHED THEN
UPDATE SET TaxCodeId = source.TaxCodeId,
	Name = source.Name,
	Value = source.Value,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	UOMId = source.UOMId
WHEN NOT MATCHED THEN
INSERT (TaxCodeAttributeId, TaxCodeId, Name, Value, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, UOMId)
VALUES (source.TaxCodeAttributeId, source.TaxCodeId, source.Name, source.Value, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.UOMId)
;

SET IDENTITY_INSERT dbo.TaxCodeAttribute OFF

END
GO



CREATE OR ALTER PROCEDURE dbo.sp_Merge_TaxCodeCategorization_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.TaxCodeCategorization ON

MERGE dbo.TaxCodeCategorization AS target
USING (SELECT TaxCodeCategorizationId, TaxCodeId, Country, TaxCodeTypeId, IsPhysical, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate FROM dbo.TaxCodeCategorization_Temp_Shard_1) 
	AS source (TaxCodeCategorizationId, TaxCodeId, Country, TaxCodeTypeId, IsPhysical, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
ON (target.TaxCodeCategorizationId = source.TaxCodeCategorizationId)
WHEN MATCHED THEN
UPDATE SET TaxCodeId = source.TaxCodeId,
	Country = source.Country,
	TaxCodeTypeId = source.TaxCodeTypeId,
	IsPhysical = source.IsPhysical,
	EffDate = source.EffDate,
	EndDate = source.EndDate,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate
WHEN NOT MATCHED THEN
INSERT (TaxCodeCategorizationId, TaxCodeId, Country, TaxCodeTypeId, IsPhysical, EffDate, EndDate, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (source.TaxCodeCategorizationId, source.TaxCodeId, source.Country, source.TaxCodeTypeId, source.IsPhysical, source.EffDate, source.EndDate, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate)
;

SET IDENTITY_INSERT dbo.TaxCodeCategorization OFF

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_Item_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.Item ON

MERGE dbo.Item AS target
USING (SELECT ItemId, ItemCode, CompanyId, TaxCodeId, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate FROM dbo.Item_Temp_Shard_1) 
	AS source (ItemId, ItemCode, CompanyId, TaxCodeId, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
ON (target.ItemId = source.ItemId)
WHEN MATCHED THEN
UPDATE SET ItemCode = source.ItemCode,
	CompanyId = source.CompanyId,
	TaxCodeId = source.TaxCodeId,
	Description = source.Description,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate
WHEN NOT MATCHED THEN
INSERT (ItemId, ItemCode, CompanyId, TaxCodeId, Description, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (source.ItemId, source.ItemCode, source.CompanyId, source.TaxCodeId, source.Description, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate)
;

SET IDENTITY_INSERT dbo.Item OFF

END
GO


CREATE OR ALTER PROCEDURE dbo.sp_Merge_ItemAttribute_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.ItemAttribute ON

MERGE dbo.ItemAttribute AS target
USING (SELECT ItemAttributeId, ItemId, Name, Value, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, UOMId FROM dbo.ItemAttribute_Temp_Shard_1) 
	AS source (ItemAttributeId, ItemId, Name, Value, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, UOMId)
ON (target.ItemAttributeId = source.ItemAttributeId)
WHEN MATCHED THEN
UPDATE SET ItemId = source.ItemId,
	Name = source.Name,
	Value = source.Value,
	CreatedUserId = source.CreatedUserId,
	CreatedDate = source.CreatedDate,
	ModifiedUserId = source.ModifiedUserId,
	ModifiedDate = source.ModifiedDate,
	UOMId = source.UOMId
WHEN NOT MATCHED THEN
INSERT (ItemAttributeId, ItemId, Name, Value, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate, UOMId)
VALUES (source.ItemAttributeId, source.ItemId, source.Name, source.Value, source.CreatedUserId, source.CreatedDate, source.ModifiedUserId, source.ModifiedDate, source.UOMId)
;

SET IDENTITY_INSERT dbo.ItemAttribute OFF

END
GO


CREATE OR ALTER PROCEDURE dbo.sp_Merge_DocumentAddress_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.DocumentAddress ON

MERGE dbo.DocumentAddress AS target
USING (SELECT DocumentAddressId, DocumentId, Line1, City, Country, Region, PostalCode, BoundaryLevelId, JurisCode, County, CitySignature, TaxRegionId, GeoCode, GeocodeTypeId, ValidateStatusId, Latitude, Longitude, DocumentQueueAddressId, AddressLine1, AddressLine2, AddressLine3 FROM dbo.DocumentAddress_Temp_Shard_1) 
	AS source (DocumentAddressId, DocumentId, Line1, City, Country, Region, PostalCode, BoundaryLevelId, JurisCode, County, CitySignature, TaxRegionId, GeoCode, GeocodeTypeId, ValidateStatusId, Latitude, Longitude, DocumentQueueAddressId, AddressLine1, AddressLine2, AddressLine3)
ON (target.DocumentAddressId = source.DocumentAddressId)
WHEN MATCHED THEN
UPDATE SET DocumentId = source.DocumentId,
	Line1 = source.Line1,
	City = source.City,
	Country = source.Country,
	Region = source.Region,
	PostalCode = source.PostalCode,
	BoundaryLevelId = source.BoundaryLevelId,
	JurisCode = source.JurisCode,
	County = source.County,
	CitySignature = source.CitySignature,
	TaxRegionId = source.TaxRegionId,
	GeoCode = source.GeoCode,
	GeocodeTypeId = source.GeocodeTypeId,
	ValidateStatusId = source.ValidateStatusId,
	Latitude = source.Latitude,
	Longitude = source.Longitude,
	DocumentQueueAddressId = source.DocumentQueueAddressId,
	AddressLine1 = source.AddressLine1,
	AddressLine2 = source.AddressLine2,
	AddressLine3 = source.AddressLine3
WHEN NOT MATCHED THEN
INSERT (DocumentAddressId, DocumentId, Line1, City, Country, Region, PostalCode, BoundaryLevelId, JurisCode, County, CitySignature, TaxRegionId, GeoCode, GeocodeTypeId, ValidateStatusId, Latitude, Longitude, DocumentQueueAddressId, AddressLine1, AddressLine2, AddressLine3)
VALUES (source.DocumentAddressId, source.DocumentId, source.Line1, source.City, source.Country, source.Region, source.PostalCode, source.BoundaryLevelId, source.JurisCode, source.County, source.CitySignature, source.TaxRegionId, source.GeoCode, source.GeocodeTypeId, source.ValidateStatusId, source.Latitude, source.Longitude, source.DocumentQueueAddressId, source.AddressLine1, source.AddressLine2, source.AddressLine3)
;

SET IDENTITY_INSERT dbo.DocumentAddress OFF

END
GO


CREATE OR ALTER PROCEDURE dbo.sp_Merge_DocumentProperty_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.DocumentProperty ON

MERGE dbo.DocumentProperty AS target
USING (SELECT DocumentPropertyId, DocumentId, ReferenceCode, VATNumberTypeId FROM dbo.DocumentProperty_Temp_Shard_1) 
	AS source (DocumentPropertyId, DocumentId, ReferenceCode, VATNumberTypeId)
ON (target.DocumentPropertyId = source.DocumentPropertyId)
WHEN MATCHED THEN
UPDATE SET DocumentId = source.DocumentId,
	ReferenceCode = source.ReferenceCode,
	VATNumberTypeId = source.VATNumberTypeId
WHEN NOT MATCHED THEN
INSERT (DocumentPropertyId, DocumentId, ReferenceCode, VATNumberTypeId)
VALUES (source.DocumentPropertyId, source.DocumentId, source.ReferenceCode, source.VATNumberTypeId)
;

SET IDENTITY_INSERT dbo.DocumentProperty OFF

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_DocumentLineParameterBag_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.DocumentLineParameterBag ON

MERGE dbo.DocumentLineParameterBag AS target
USING (SELECT DocumentLineParameterBagId, DocumentLineId, Name, Value, UOMId, UOMIdSystem, ValueSystem FROM dbo.DocumentLineParameterBag_Temp_Shard_1) 
	AS source (DocumentLineParameterBagId, DocumentLineId, Name, Value, UOMId, UOMIdSystem, ValueSystem)
ON (target.DocumentLineParameterBagId = source.DocumentLineParameterBagId)
WHEN MATCHED THEN
UPDATE SET DocumentLineId = source.DocumentLineId,
	Name = source.Name,
	Value = source.Value,
	UOMId = source.UOMId,
	UOMIdSystem = source.UOMIdSystem,
	ValueSystem = source.ValueSystem
WHEN NOT MATCHED THEN
INSERT (DocumentLineParameterBagId, DocumentLineId, Name, Value, UOMId, UOMIdSystem, ValueSystem)
VALUES (source.DocumentLineParameterBagId, source.DocumentLineId, source.Name, source.Value, source.UOMId, source.UOMIdSystem, source.ValueSystem)
;

SET IDENTITY_INSERT dbo.DocumentLineParameterBag OFF

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_DocumentLineProperty_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.DocumentLineProperty ON

MERGE dbo.DocumentLineProperty AS target
USING (SELECT DocumentLinePropertyId, DocumentLineId, VATCode, VATNumberTypeId, HSCodeId, HSCode, HSCodeUsed, CIF FROM dbo.DocumentLineProperty_Temp_Shard_1) 
	AS source (DocumentLinePropertyId, DocumentLineId, VATCode, VATNumberTypeId, HSCodeId, HSCode, HSCodeUsed, CIF)
ON (target.DocumentLinePropertyId = source.DocumentLinePropertyId)
WHEN MATCHED THEN
UPDATE SET DocumentLineId = source.DocumentLineId,
	VATCode = source.VATCode,
	VATNumberTypeId = source.VATNumberTypeId,
	HSCodeId = source.HSCodeId,
	HSCode = source.HSCode,
	HSCodeUsed = source.HSCodeUsed,
	CIF = source.CIF
WHEN NOT MATCHED THEN
INSERT (DocumentLinePropertyId, DocumentLineId, VATCode, VATNumberTypeId, HSCodeId, HSCode, HSCodeUsed, CIF)
VALUES (source.DocumentLinePropertyId, source.DocumentLineId, source.VATCode, source.VATNumberTypeId, source.HSCodeId, source.HSCode, source.HSCodeUsed, source.CIF)
;

SET IDENTITY_INSERT dbo.DocumentLineProperty OFF

END
GO



CREATE OR ALTER PROCEDURE dbo.sp_Merge_DocumentLineDetailProperty_From_Temp_Shard_1
AS
BEGIN

SET IDENTITY_INSERT dbo.DocumentLineDetailProperty ON

MERGE dbo.DocumentLineDetailProperty AS target
USING (SELECT DocumentLineDetailId, TaxTypeMappingId, RateTypeTaxTypeMappingId, IsFee, TaxAuthorityId, ReportLevel FROM dbo.DocumentLineDetailProperty_Temp_Shard_1) 
	AS source (DocumentLineDetailId, TaxTypeMappingId, RateTypeTaxTypeMappingId, IsFee, TaxAuthorityId, ReportLevel)
ON (target.DocumentLineDetailId = source.DocumentLineDetailId)
WHEN MATCHED THEN
UPDATE SET TaxTypeMappingId = source.TaxTypeMappingId,
	RateTypeTaxTypeMappingId = source.RateTypeTaxTypeMappingId,
	IsFee = source.IsFee,
	TaxAuthorityId = source.TaxAuthorityId,
	ReportLevel = source.ReportLevel
WHEN NOT MATCHED THEN
INSERT (DocumentLineDetailId, TaxTypeMappingId, RateTypeTaxTypeMappingId, IsFee, TaxAuthorityId, ReportLevel)
VALUES (source.DocumentLineDetailId, source.TaxTypeMappingId, source.RateTypeTaxTypeMappingId, source.IsFee, source.TaxAuthorityId, source.ReportLevel)
;

SET IDENTITY_INSERT dbo.DocumentLineDetailProperty OFF

END
GO

