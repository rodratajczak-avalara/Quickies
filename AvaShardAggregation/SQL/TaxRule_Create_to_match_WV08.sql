

IF OBJECT_ID ('dbo.TaxRule') IS NOT NULL
	DROP TABLE dbo.TaxRule
GO

CREATE TABLE [dbo].[TaxRule](
	[TaxRuleId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[StateFIPS] [varchar](2) NOT NULL,
	[JurisName] [varchar](200) NOT NULL,
	[JurisCode] [varchar](10) NOT NULL,
	[JurisTypeId] [varchar](3) NOT NULL,
	[TaxCodeId] [int] NULL,
	[CustomerUsageType] [varchar](25) NULL,
	[TaxTypeId] [varchar](1) NOT NULL,
	[RateTypeId] [varchar](1) NULL,
	[TaxRuleTypeId] [smallint] NOT NULL,
	[IsAllJuris] [bit] NOT NULL,
	[Value] [decimal](18, 6) NOT NULL,
	[Cap] [decimal](18, 6) NOT NULL,
	[Threshold] [decimal](18, 6) NOT NULL,
	[Options_Old] [varchar](100) NULL,
	[EffDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUserId] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[State] [varchar](3) NOT NULL,
	[CountyFIPS] [varchar](3) NULL,
	[IsSTPro] [bit] NOT NULL,
	[Country] [char](2) NULL,
	[Sourcing] [char](1) NULL,
	[Options] [varchar](510) NULL,
	[UnitOfBasisId] [bigint] NULL,
	[AttributeOptions] [varchar](510) NULL,
	[ReturnsRateID] [int] NULL,
	[ReturnsDeductionID] [int] NULL,
	[ReturnsTaxTypeID] [int] NULL,
	[AttributeApplicability] [varchar](510) NULL,
	[TaxTypeMappingId] [int] NULL,
	[RateTypeTaxTypeMappingId] [int] NULL,
	[NonPassThruExpression] [varchar](500) NULL,
	[CurrencyCode] [char](3) NULL,
	[PreferredProgramId] [int] NULL,
	[UOMId] [int] NULL,
	[HashKey] [varbinary](900) NULL,
 CONSTRAINT [PK_TaxRule] PRIMARY KEY CLUSTERED ([TaxRuleId] ASC) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX IX_TaxRule_HashKey
	ON dbo.TaxRule (HashKey)
GO
