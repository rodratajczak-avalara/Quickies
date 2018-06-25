

IF OBJECT_ID ('dbo.DocumentLineDetail_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.DocumentLineDetail_Temp_Shard_3
GO

CREATE TABLE [dbo].[DocumentLineDetail_Temp_Shard_3](
	[DocumentLineDetailId] [bigint] NOT NULL,
	[DocumentLineId] [bigint] NOT NULL,
	[JurisTypeId] [varchar](3) NULL,
	[SERCode] [varchar](10) NULL,
	[JurisCode] [varchar](10) NULL,
	[StateFIPS] [varchar](2) NOT NULL,
	[TaxableAmount] [money] NOT NULL,
	[Rate_Old] [decimal](8, 6) NOT NULL,
	[Tax] [money] NOT NULL,
	[Sourcing] [varchar](2) NULL,
	[TaxTypeId] [varchar](1) NOT NULL,
	[ExemptAmount] [money] NOT NULL,
	[ExemptReasonId] [int] NOT NULL,
	[Region] [varchar](3) NULL,
	[InState] [bit] NOT NULL,
	[NonTaxableAmount] [money] NOT NULL,
	[RateSourceId] [int] NOT NULL,
	[RateRuleId] [int] NOT NULL,
	[NonTaxableTypeId] [int] NOT NULL,
	[NonTaxableRuleId] [int] NOT NULL,
	[CountyFIPS] [varchar](3) NULL,
	[AddressId] [bigint] NULL,
	[JurisName] [varchar](200) NULL,
	[StateAssignedNo] [varchar](50) NULL,
	[Country] [varchar](2) NOT NULL,
	[JurisdictionId] [int] NOT NULL,
	[TaxName] [varchar](75) NOT NULL,
	[TaxAuthorityTypeId] [int] NOT NULL,
	[TaxRegionId] [int] NULL,
	[TaxCalculated] [money] NULL,
	[TaxOverride] [money] NULL,
	[SignatureCode] [char](4) NULL,
	[RateTypeId] [char](1) NULL,
	[DocumentId] [bigint] NULL,
	[Rate] [decimal](18, 6) NULL,
	[TaxableUnits] [decimal](19, 4) NULL,
	[NonTaxableUnits] [decimal](19, 4) NULL,
	[ExemptUnits] [decimal](19, 4) NULL,
	[UnitOfBasisId] [bigint] NULL,
	[ReturnsRateID] [int] NULL,
	[ReturnsDeductionID] [int] NULL,
	[ReturnsTaxTypeID] [int] NULL,
	[IsNonPassThru] [bit] NULL,
 CONSTRAINT [PK_DocumentLineDetail_Temp_Shard_3] PRIMARY KEY ( [DocumentLineDetailId] ASC )
 )
GO


 
 