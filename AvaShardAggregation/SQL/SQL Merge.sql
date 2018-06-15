CREATE OR ALTER PROCEDURE dbo.sp_Merge_Document_From_Temp_Shard_1
AS
BEGIN

CREATE TABLE #Document_Temp_Shard_1_Results
    (ExistingDocumentId BIGINT,  
     ExistingDocumentTypeId  BIGINT,  
     ExistingCompanyId BIGINT,  
     ActionTaken nvarchar(10),  
     NewDocuumentId BIGINT,  
     NewDocumentTypeId BIGINT,  
     NewCompanyId BIGINT  
    )

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
OUTPUT deleted.DocumentId, deleted.DocumentTypeId, deleted.CompanyId, $action, inserted.DocumentId, inserted.DocumentTypeId, inserted.CompanyId INTO #Document_Temp_Shard_1_Results;

SET IDENTITY_INSERT dbo.Document OFF

SELECT * FROM #Document_Temp_Shard_1_Results

DROP TABLE #Document_Temp_Shard_1_Results

END
GO




CREATE OR ALTER PROCEDURE dbo.sp_Merge_Document_From_Temp_Shard_2
AS
BEGIN

CREATE TABLE #Document_Temp_Shard_2_Results
    (ExistingDocumentId BIGINT,  
     ExistingDocumentTypeId  BIGINT,  
     ExistingCompanyId BIGINT,  
     ActionTaken nvarchar(10),  
     NewDocuumentId BIGINT,  
     NewDocumentTypeId BIGINT,  
     NewCompanyId BIGINT  
    )

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
OUTPUT deleted.DocumentId, deleted.DocumentTypeId, deleted.CompanyId, $action, inserted.DocumentId, inserted.DocumentTypeId, inserted.CompanyId INTO #Document_Temp_Shard_2_Results;

SET IDENTITY_INSERT dbo.Document OFF

SELECT * FROM #Document_Temp_Shard_2_Results

DROP TABLE #Document_Temp_Shard_2_Results

END
GO





CREATE OR ALTER PROCEDURE dbo.sp_Merge_Document_From_Temp_Shard_3
AS
BEGIN

/*
CREATE TABLE #Document_Temp_Shard_3_Results
    (ExistingDocumentId BIGINT,  
     ExistingDocumentTypeId  BIGINT,  
     ExistingCompanyId BIGINT,  
     ActionTaken nvarchar(10),  
     NewDocuumentId BIGINT,  
     NewDocumentTypeId BIGINT,  
     NewCompanyId BIGINT  
    )
*/

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
--OUTPUT deleted.DocumentId, deleted.DocumentTypeId, deleted.CompanyId, $action, inserted.DocumentId, inserted.DocumentTypeId, inserted.CompanyId INTO #Document_Temp_Shard_1_Results
;

SET IDENTITY_INSERT dbo.Document OFF

--SELECT * FROM #Document_Temp_Shard_3_Results

--DROP TABLE #Document_Temp_Shard_3_Results

END
GO