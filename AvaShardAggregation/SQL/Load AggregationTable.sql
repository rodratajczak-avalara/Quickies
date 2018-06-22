SET IDENTITY_INSERT dbo.AggregationTable ON
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (1, 'AvaTaxAccount', 'dbo', 'Account', null, 1, 0, 0, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (2, 'AvaTaxAccount', 'dbo', 'TaxServiceConfig', 1, 1, 0, 2, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (3, 'AvaTaxAccount', 'dbo', 'FormServiceConfig', 1, 0, 1, 2, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (4, 'AvaTaxAccount', 'dbo', 'AccountFeatureList', 1, 0, 0, 2, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (5, 'AvaTaxAccount', 'dbo', 'AddressServiceConfig', 1, 1, 0, 2, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (6, 'AvaTaxAccount', 'dbo', 'DocumentCodeChangeList', 1, 1, 0, 2, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (7, 'AvaTaxAccount', 'dbo', 'BoundaryOverride', 1, 1, 0, 2, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (8, 'AvaTaxAccount', 'dbo', 'JurisdictionOverride', 1, 1, 0, 2, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (9, 'AvaTaxAccount', 'dbo', 'Service', 1, 1, 0, 2, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (10, 'AvaTaxAudit', 'dbo', 'AuditServiceConfig', 1, 1, 0, 2, 0, 0, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (11, 'AvaTaxAccount', 'dbo', 'Subscription', 1, 1, 0, 2, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (12, 'AvaTaxAccount', 'dbo', 'User', 1, 1, 0, 1, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (13, 'AvaTaxAccount', 'dbo', 'CombinedHSTConfig', 1, 0, 0, 2, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (14, 'AvaTaxAccount', 'dbo', 'MaxLine', 1, 0, 0, 2, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (15, 'AvaTaxAccount', 'dbo', 'Company', 1, 1, 0, 2, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (16, 'AvaTaxAccount', 'dbo', 'TaxRule', 15, 1, 0, 3, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (17, 'AvaTaxAccount', 'dbo', 'CompanyContact', 15, 1, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (18, 'AvaTaxAccount', 'dbo', 'CompanyLocation', 15, 1, 0, 3, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (19, 'AvaTaxAccount', 'dbo', 'CompanySetting', 15, 0, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (20, 'AvaTaxAccount', 'dbo', 'ExemptCert', 15, 1, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (21, 'AvaTaxAccount', 'dbo', 'Nexus', 15, 1, 0, 3, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (22, 'AvaTaxAccount', 'dbo', 'TaxCode', 15, 1, 0, 3, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (23, 'AvaTaxAccount', 'dbo', 'UPCCodeLookup', 15, 1, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (24, 'AvaTaxAccount', 'dbo', 'CompanyDistanceThreshold', 15, 0, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (25, 'AvaTaxAccount', 'dbo', 'AvaCertServiceConfig', 15, 1, 0, 3, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (26, 'AvaTaxAccount', 'dbo', 'AvaCommsServiceConfig', 15, 1, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (27, 'AvaTaxAccount', 'dbo', 'BRCompanySecurityCertificate', 15, 1, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (28, 'AvaTaxAccount', 'dbo', 'BRTaxRegimeBuyerTypeConfig', 15, 1, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (29, 'AvaTaxAccount', 'dbo', 'CompanyRPSSeries', 15, 0, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (30, 'AvaTaxAccount', 'dbo', 'CompanyReturn', 15, 1, 0, 3, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (31, 'AvaTaxAccount', 'dbo', 'CompanyTaxForm', 15, 1, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (32, 'AvaTaxAccount', 'dbo', 'Return', 15, 1, 0, 3, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (33, 'AvaTaxBatch', 'dbo', 'Batch', 15, 1, 0, 3, 0, 0, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (34, 'AvaTaxAccount', 'dbo', 'Document', 15, 1, 0, 3, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (35, 'AvaTaxAccount', 'dbo', 'SpecialTaxDistrictOverride', 7, 0, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (36, 'AvaTaxAccount', 'dbo', 'SubscriptionSetting', 11, 0, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (37, 'AvaTaxAccount', 'dbo', 'TaxRuleProductDetail', 16, 1, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (38, 'AvaTaxAccount', 'dbo', 'CompanyLocationSetting', 18, 0, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (39, 'AvaTaxAccount', 'dbo', 'ExemptCertDetail', 20, 0, 0, 4, 0, 0, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (40, 'AvaTaxAccount', 'dbo', 'ExemptCertDetailTaxCode', 20, 0, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (41, 'AvaTaxAccount', 'dbo', 'TaxCodeAttribute', 22, 1, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (42, 'AvaTaxAccount', 'dbo', 'TaxCodeCategorization', 22, 1, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (43, 'AvaTaxAccount', 'dbo', 'Item', 22, 1, 0, 4, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (44, 'AvaTaxAccount', 'dbo', 'CompanySupportingReturn', 30, 0, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (45, 'AvaTaxAccount', 'dbo', 'ReturnLine', 32, 0, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (46, 'AvaTaxAccount', 'dbo', 'BatchFile', 33, 0, 0, 3, 0, 0, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (47, 'AvaTaxAccount', 'dbo', 'BatchError', 33, 0, 0, 4, 0, 0, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (48, 'AvaTaxAccount', 'dbo', 'DocumentAddress', 34, 0, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (49, 'AvaTaxAccount', 'dbo', 'DocumentProperty', 34, 0, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (50, 'AvaTaxAccount', 'dbo', 'DocumentParameterBag', 34, 0, 0, 4, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (51, 'AvaTaxAccount', 'dbo', 'DocumentAddressLocationTypes', 34, 0, 0, 4, 0, 0, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (52, 'AvaTaxAccount', 'dbo', 'DocumentLine', 34, 0, 0, 4, 1, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (53, 'AvaTaxAccount', 'dbo', 'ItemAttribute', 43, 1, 0, 5, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (54, 'AvaTaxAccount', 'dbo', 'DocumentLineProperty', 52, 0, 0, 5, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (55, 'AvaTaxAccount', 'dbo', 'DocumentLineParameterBag', 52, 0, 0, 5, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (56, 'AvataxAccount', 'dbo', 'DocumentLineAddressLocationTypes', 52, 0, 0, 5, 0, 0, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (57, 'AvaTaxAccount', 'dbo', 'DocumentLineDetail', 56, 0, 0, 6, 0, 1, 0, getdate(), 0, getdate())
GO

INSERT INTO dbo.AggregationTable (AggregationTableId, [Database], [Schema], TableName, ParentTableId, ModifiedDateExists, FullTable, ExecutionGroup, RemoveDuplicate, Enabled, CreatedUserId, CreatedDate, ModifiedUserId, ModifiedDate)
VALUES (58, 'AvaTaxAccount', 'dbo', 'DocumentLineDetailProperty', 56, 0, 0, 6, 0, 1, 0, getdate(), 0, getdate())
GO

SET IDENTITY_INSERT dbo.AggregationTable OFF
GO