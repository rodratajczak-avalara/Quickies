
IF OBJECT_ID ('dbo.Document_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.Document_Temp_Shard_3
GO

CREATE TABLE dbo.Document_Temp_Shard_3
	(
	DocumentId                          BIGINT NOT NULL,
	DocumentTypeId                      TINYINT NOT NULL,
	CompanyId                           INT NOT NULL,
	DocumentDate                        DATETIME NOT NULL,
	DocumentCode                        VARCHAR (50),
	DocumentStatusId                    TINYINT NOT NULL,
	PurchaseOrderNo                     VARCHAR (50),
	CustomerVendorCode                  VARCHAR (50),
	SalespersonCode                     VARCHAR (25),
	CustomerUsageType                   VARCHAR (25),
	ReferenceCode                       VARCHAR (50),
	IsReconciled                        BIT  NOT NULL,
	TotalAmount                         MONEY  NOT NULL,
	TotalTax                            MONEY  NOT NULL,
	ExemptNo                            VARCHAR (25),
	ModifiedDate                        DATETIME NOT NULL,
	ModifiedUserId                      INT NOT NULL,
	TaxDate                             DATETIME NOT NULL,
	DocumentLineCount                   INT NOT NULL,
	HashCode                            INT,
	TotalTaxable                        MONEY ,
	TotalExempt                         MONEY ,
	BatchCode                           VARCHAR (25) ,
	SoftwareVersion                     VARCHAR (25),
	LocationCode                        VARCHAR (50),
	AdjustmentReasonId                  TINYINT ,
	AdjustmentDescription               VARCHAR (255),
	IsLocked                            BIT NOT NULL,
	Version                             INT NOT NULL,
	TotalTaxCalculated                  MONEY ,
	TaxOverrideAmount                   MONEY ,
	TaxOverrideTypeId                   TINYINT ,
	TaxOverrideReason                   VARCHAR (255),
	CurrencyCode                        CHAR (3),
	PaymentDate                         DATETIME,
	OriginAddressId                     BIGINT,
	DestinationAddressId                BIGINT,
	ExchangeRate                        MONEY,
	ExchangeRateEffDate                 DATETIME,
	AdjustedStatusId                    TINYINT,
	Region                              VARCHAR (3),
	Country                             VARCHAR (50),
	PosLaneCode                         VARCHAR (50),
	BusinessIdentificationNo            VARCHAR (25),
	IsSellerImporterOfRecord            BIT,
	BRBuyerType                         VARCHAR (5),
	BRBuyer_IsExemptOrCannotWH_IRRF     BIT,
	BRBuyer_IsExemptOrCannotWH_PISRF    BIT,
	BRBuyer_IsExemptOrCannotWH_COFINSRF BIT,
	BRBuyer_IsExemptOrCannotWH_CSLLRF   BIT,
	BRBuyer_IsExempt_PIS                BIT,
	BRBuyer_IsExempt_COFINS             BIT,
	BRBuyer_IsExempt_CSLL               BIT,
	Description                         NVARCHAR (2048),
	Email                               VARCHAR (50),
	CONSTRAINT PK_Document_Temp_Shard_3 PRIMARY KEY (DocumentId),
	CONSTRAINT IX_Document_Temp_Shard_3_Unique UNIQUE (CompanyId,DocumentCode,DocumentTypeId,Version)
	)
GO





IF OBJECT_ID ('dbo.DocumentLine_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.DocumentLine_Temp_Shard_3
GO

CREATE TABLE dbo.DocumentLine_Temp_Shard_3
	(
	DocumentLineId           BIGINT NOT NULL,
	DocumentId               BIGINT NOT NULL,
	[LineNo]                 VARCHAR (50),
	ItemCode                 VARCHAR (50),
	TaxCode                  VARCHAR (25),
	OriginAddressId          BIGINT,
	DestinationAddressId     BIGINT,
	Quantity                 MONEY NOT NULL,
	LineAmount               MONEY,
	ExemptAmount             MONEY,
	DiscountAmount           MONEY,
	DiscountTypeId           TINYINT,
	TaxableAmount            MONEY,
	ExemptNo                 VARCHAR (25),
	RevAccount               VARCHAR (50),
	Ref1                     VARCHAR (250),
	Ref2                     VARCHAR (250),
	IsSSTP                   BIT NOT NULL,
	IsItemTaxable            BIT NOT NULL,
	CustomerUsageType        VARCHAR (25),
	Description              NVARCHAR (2096),
	Sourcing                 VARCHAR (2),
	GoodsServiceCode         BIGINT NOT NULL,
	TaxEngine                VARCHAR (10) NOT NULL,
	BoundaryOverrideId       INT,
	TweEntityUseCode         VARCHAR (40),
	Tax                      MONEY,
	ExemptCertId             INT,
	TaxCodeId                INT,
	TaxCalculated            MONEY,
	TaxOverrideAmount        MONEY,
	TaxOverrideTypeId        TINYINT,
	TaxOverrideReason        VARCHAR (255),
	TaxDate                  DATETIME,
	ReportingDate            DATETIME,
	AccountingMethodId       TINYINT,
	TaxIncluded              BIT,
	IsExempt                 BIT,
	BusinessIdentificationNo VARCHAR (25),
	UnitOfMeasurement        VARCHAR (25),
	StateSstNexusTypeId      INT,
	CONSTRAINT PK_DocumentLine_Temp_Shard_3 PRIMARY KEY (DocumentLineId),
	CONSTRAINT IX_DocumentLine_Temp_Shard_3 UNIQUE (DocumentId,[LineNo])
	)
GO







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


 

IF OBJECT_ID ('dbo.DocumentParameterBag_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.DocumentParameterBag_Temp_Shard_3
GO

CREATE TABLE dbo.DocumentParameterBag_Temp_Shard_3
	(
	DocumentParameterBagId BIGINT NOT NULL,
	DocumentId             BIGINT NOT NULL,
	Name                   VARCHAR (50) NOT NULL,
	Value                  VARCHAR (50),
	UOMId                  INT,
	UOMIdSystem            INT,
	ValueSystem            VARCHAR (50),
	CONSTRAINT PK_DocumentParameterBag_Temp_Shard_3 PRIMARY KEY (DocumentParameterBagId)
	)
GO




IF OBJECT_ID ('dbo.Account_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.Account_Temp_Shard_3
GO

CREATE TABLE dbo.Account_Temp_Shard_3
	(
	AccountId       INT NOT NULL,
	LicenseKey      VARCHAR (50),
	Salt            VARCHAR (50),
	AccountName     VARCHAR (50),
	SiteId          INT,
	EffDate         DATETIME,
	EndDate         DATETIME,
	CreatedUserId   INT,
	CreatedDate     DATETIME,
	ModifiedUserId  INT,
	ModifiedDate    DATETIME,
	AccountStatusId INT,
	CRMId           INT,
	CRMIdSTR        VARCHAR (100),
	CONSTRAINT PK_Account_Temp_Shard_3 PRIMARY KEY (AccountId)
	)
GO


IF OBJECT_ID ('dbo.TaxServiceConfig_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.TaxServiceConfig_Temp_Shard_3
GO

CREATE TABLE dbo.TaxServiceConfig_Temp_Shard_3
	(
	AccountId                            INT NOT NULL,
	RequireOriginAddress                 BIT NOT NULL,
	RequireMappedItemCode                BIT NOT NULL,
	CreatedUserId                        INT NOT NULL,
	CreatedDate                          DATETIME NOT NULL,
	ModifiedUserId                       INT NOT NULL,
	ModifiedDate                         DATETIME NOT NULL,
	EcmsEnabled                          BIT NOT NULL,
	EcmsCertUse                          SMALLINT NOT NULL,
	EcmsCompleteCertsRequired            BIT NOT NULL,
	EcmsOverrideCode                     VARCHAR (25),
	EcmsSstCertsRequired                 BIT,
	MaxLines                             INT,
	EcmsCertUseCa                        SMALLINT,
	IsJaasDisabled                       BIT,
	SSTPolicyOverrideDate                DATETIME,
	ItemDescPolicyOverrideDate           DATETIME,
	UseIsSellerImporterOfRecordFromNexus BIT,
	CONSTRAINT PK_TaxServiceConfig_Temp_Shard_3 PRIMARY KEY (AccountId),
	CONSTRAINT IX_TaxServiceConfig_Temp_Shard_3 UNIQUE (AccountId,CreatedUserId)
	)
GO




IF OBJECT_ID ('dbo.AddressServiceConfig_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.AddressServiceConfig_Temp_Shard_3
GO

CREATE TABLE dbo.AddressServiceConfig_Temp_Shard_3
	(
	AccountId      INT NOT NULL,
	IsUpperCase    BIT NOT NULL,
	CreatedUserId  INT NOT NULL,
	CreatedDate    DATETIME NOT NULL,
	ModifiedUserId INT NOT NULL,
	ModifiedDate   DATETIME NOT NULL,
	IsJaasDisabled BIT,
	CONSTRAINT PK_AddressServiceConfig_Temp_Shard_3 PRIMARY KEY (AccountId)
	)
GO


IF OBJECT_ID ('dbo.DocumentCodeChangeList_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.DocumentCodeChangeList_Temp_Shard_3
GO

CREATE TABLE dbo.DocumentCodeChangeList_Temp_Shard_3
	(
	AccountId    INT NOT NULL,
	DocumentCode VARCHAR (50) NOT NULL,
	Committed    BIT NOT NULL,
	ModifiedDate DATETIME NOT NULL,
	CONSTRAINT PK_DocumentCodeChangeList_Temp_Shard_3 PRIMARY KEY (AccountId,DocumentCode,Committed)
	)
GO


IF OBJECT_ID ('dbo.BoundaryOverride_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.BoundaryOverride_Temp_Shard_3
GO

CREATE TABLE dbo.BoundaryOverride_Temp_Shard_3
	(
	BoundaryOverrideId INT NOT NULL,
	AccountId          INT NOT NULL,
	AddrLo             VARCHAR (10),
	AddrHi             VARCHAR (10),
	OddEven            VARCHAR (1),
	StreetPre          VARCHAR (2),
	StreetName         VARCHAR (25),
	StreetSuffix       VARCHAR (4),
	StreetPost         VARCHAR (2),
	City               VARCHAR (50),
	County             VARCHAR (50),
	ZIP5Lo             VARCHAR (5),
	ZIP5Hi             VARCHAR (5),
	ZIP4Lo             VARCHAR (4),
	ZIP4Hi             VARCHAR (4),
	StateFIPS          VARCHAR (2),
	CountyFIPS         VARCHAR (10) NOT NULL,
	PlaceFIPS          VARCHAR (10),
	PlaceClassCode     VARCHAR (2),
	StateAssignedCode  VARCHAR (50),
	Longitude          VARCHAR (7),
	Latitude           VARCHAR (7),
	EffDate            DATETIME NOT NULL,
	EndDate            DATETIME NOT NULL,
	CreatedUserId      INT NOT NULL,
	CreatedDate        DATETIME NOT NULL,
	ModifiedUserId     INT NOT NULL,
	ModifiedDate       DATETIME NOT NULL,
	RateId             INT,
	HasSTJ             BIT NOT NULL,
	BoundaryLevel      INT NOT NULL,
	CountyJurisName    VARCHAR (50),
	CityJurisName      VARCHAR (50),
	TaxRegionId        INT NOT NULL,
	State              CHAR (2),
	CONSTRAINT PK_BoundaryOverride_Temp_Shard_3 PRIMARY KEY NONCLUSTERED (BoundaryOverrideId),
	CONSTRAINT CX_BoundaryOverride_Temp_Shard_3 UNIQUE CLUSTERED (AccountId,State,City,StreetName,StreetPre,StreetPost,AddrLo,OddEven,ZIP5Lo,ZIP4Lo,EffDate,StreetSuffix,TaxRegionId)
	)
GO



IF OBJECT_ID ('dbo.JurisdictionOverride_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.JurisdictionOverride_Temp_Shard_3
GO

CREATE TABLE dbo.JurisdictionOverride_Temp_Shard_3
	(
	JurisdictionOverrideId INT NOT NULL,
	BoundaryOverrideId     INT,
	AccountId              INT NOT NULL,
	Address                VARCHAR (50),
	City                   VARCHAR (50),
	Region                 VARCHAR (3) NOT NULL,
	PostalCode             VARCHAR (11) NOT NULL,
	Description            NVARCHAR (50),
	CreatedUserId          INT,
	CreatedDate            DATETIME NOT NULL,
	ModifiedUserId         INT NOT NULL,
	ModifiedDate           DATETIME NOT NULL,
	EffDate                DATETIME,
	EndDate                DATETIME,
	CONSTRAINT PK_JurisdictionOverride_Temp_Shard_3 PRIMARY KEY NONCLUSTERED (JurisdictionOverrideId),
	CONSTRAINT IX_JurisdictionOverride_Temp_Shard_3 UNIQUE (AccountId,Region,City,PostalCode,Address,EffDate)
	)
GO





IF OBJECT_ID ('dbo.Service_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.Service_Temp_Shard_3
GO

CREATE TABLE dbo.Service_Temp_Shard_3
	(
	ServiceId      INT NOT NULL,
	AccountId      INT NOT NULL,
	ServiceTypeId  INT NOT NULL,
	Quantity       INT NOT NULL,
	CompanyId      INT,
	CreatedUserId  INT NOT NULL,
	CreatedDate    DATETIME NOT NULL,
	ModifiedUserId INT NOT NULL,
	ModifiedDate   DATETIME NOT NULL,
	EffDate        DATETIME NOT NULL,
	EndDate        DATETIME NOT NULL,
	CONSTRAINT PK_Service_Temp_Shard_3 PRIMARY KEY (ServiceId),
	CONSTRAINT IX_Service_Temp_Shard_3 UNIQUE (AccountId,ServiceTypeId,EffDate)
	)
GO




IF OBJECT_ID ('dbo.Subscription_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.Subscription_Temp_Shard_3
GO

CREATE TABLE dbo.Subscription_Temp_Shard_3
	(
	SubscriptionId     INT NOT NULL,
	AccountId          INT NOT NULL,
	PartnerId          VARCHAR (50),
	ReferenceCode      VARCHAR (100),
	SubscriptionTypeId INT NOT NULL,
	CountryCode        CHAR (3),
	RegionCode         VARCHAR (3),
	EffDate            DATETIME NOT NULL,
	EndDate            DATETIME NOT NULL,
	CancelDate         DATETIME,
	IsActive           BIT NOT NULL,
	CreatedDate        DATETIME NOT NULL,
	ModifiedDate       DATETIME NOT NULL,
	CreatedUserId      INT NOT NULL,
	ModifiedUserId     INT NOT NULL,
	CONSTRAINT PK_Subscription_SubscriptionId_Temp_Shard_3 PRIMARY KEY (SubscriptionId),
	CONSTRAINT IX_UNIQUE_Subscription_AccountId_ReferenceCode_SubscriptionTypeId_RegionCode_EffDate_Temp_Shard_3 UNIQUE (AccountId,ReferenceCode,SubscriptionTypeId,RegionCode,EffDate)
	)
GO





IF OBJECT_ID ('dbo.User_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.[User_Temp_Shard_3]
GO

CREATE TABLE dbo.[User_Temp_Shard_3]
	(
	UserId              INT NOT NULL,
	AccountId           INT NOT NULL,
	UserName            VARCHAR (50),
	FirstName           NVARCHAR (50),
	LastName            NVARCHAR (50),
	Password            NVARCHAR (50),
	PasswordStatusId    INT NOT NULL,
	Email               VARCHAR (50),
	PostalCode          VARCHAR (10),
	SecurityRoleId      TINYINT NOT NULL,
	IsActive            BIT NOT NULL,
	CreatedUserId       INT NOT NULL,
	CreatedDate         DATETIME NOT NULL,
	ModifiedUserId      INT NOT NULL,
	ModifiedDate        DATETIME NOT NULL,
	FailedLoginAttempts INT NOT NULL,
	CompanyId           INT,
	SubjectId           NVARCHAR (32),
	CONSTRAINT PK_User_Temp_Shard_3 PRIMARY KEY (UserId),
	CONSTRAINT IX_User_Temp_Shard_3 UNIQUE (UserName)
	)
GO


IF OBJECT_ID ('dbo.Company_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.Company_Temp_Shard_3
GO

CREATE TABLE dbo.Company_Temp_Shard_3
	(
	CompanyId                  INT NOT NULL,
	AccountId                  INT NOT NULL,
	ParentId                   INT,
	SSTPID                     VARCHAR (9),
	CompanyCode                VARCHAR (25),
	CompanyName                NVARCHAR (50),
	IsDefault                  BIT NOT NULL,
	DefaultLocationId          INT,
	IsActive                   BIT NOT NULL,
	CreatedUserId              INT NOT NULL,
	CreatedDate                DATETIME NOT NULL,
	ModifiedUserId             INT NOT NULL,
	ModifiedDate               DATETIME NOT NULL,
	TIN                        VARCHAR (11),
	HasProfile                 BIT NOT NULL,
	IsReportingEntity          BIT NOT NULL,
	SSTEffDate                 DATETIME,
	RegalBankId                VARCHAR (20),
	EntityNo                   INT NOT NULL,
	DefaultCountry             CHAR (2),
	BaseCurrencyCode           CHAR (3),
	RoundingLevelId            TINYINT,
	CashBasisAccountingEnabled BIT,
	WarningsEnabled            BIT,
	IsTest                     BIT,
	TaxDependancyLevelId       TINYINT,
	InProgress                 BIT NOT NULL,
	BusinessIdentificationNo   VARCHAR (25),
	VATDeductionRightId        INT,
	MOSSId                     VARCHAR (25),
	MOSSCountry                VARCHAR (2),
	CONSTRAINT PK_Company_Temp_Shard_3 PRIMARY KEY (CompanyId),
	CONSTRAINT IX_Company_Temp_Shard_3 UNIQUE (AccountId,CompanyCode),
	CONSTRAINT IX_Company_Temp_Shard_3_AccountId_EntityNo UNIQUE (AccountId,EntityNo)
	)
GO



IF OBJECT_ID ('dbo.TaxRule_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.TaxRule_Temp_Shard_3
GO

CREATE TABLE dbo.TaxRule_Temp_Shard_3
	(
	TaxRuleId                INT NOT NULL,
	CompanyId                INT NOT NULL,
	StateFIPS                VARCHAR (2) NOT NULL,
	JurisName                VARCHAR (200) NOT NULL,
	JurisCode                VARCHAR (10) NOT NULL,
	JurisTypeId              VARCHAR (3) NOT NULL,
	TaxCodeId                INT,
	CustomerUsageType        VARCHAR (25),
	TaxTypeId                VARCHAR (1) NOT NULL,
	RateTypeId               VARCHAR (1),
	TaxRuleTypeId            SMALLINT NOT NULL,
	IsAllJuris               BIT,
	Value                    DECIMAL (18, 6) NOT NULL,
	Cap                      DECIMAL (18, 6) NOT NULL,
	Threshold                DECIMAL (18, 6) NOT NULL,
	Options_Old              VARCHAR (510),
	EffDate                  DATETIME NOT NULL,
	EndDate                  DATETIME NOT NULL,
	Description              NVARCHAR (255),
	CreatedUserId            INT NOT NULL,
	CreatedDate              DATETIME NOT NULL,
	ModifiedUserId           INT NOT NULL,
	ModifiedDate             DATETIME NOT NULL,
	State                    VARCHAR (3) NOT NULL,
	CountyFIPS               VARCHAR (3),
	IsSTPro                  BIT  NOT NULL,
	Country                  CHAR (2),
	Sourcing                 CHAR (1),
	Options 				 VARCHAR (510),
	UnitOfBasisId            BIGINT,
	AttributeOptions         VARCHAR (510),
	ReturnsRateID            INT,
	ReturnsDeductionID       INT,
	ReturnsTaxTypeID         INT,
	AttributeApplicability   VARCHAR (510),
	TaxTypeMappingId         INT,
	RateTypeTaxTypeMappingId INT,
	NonPassThruExpression    VARCHAR (500),
	CurrencyCode             CHAR (3) NOT NULL,
	PreferredProgramId       INT,
	UOMId                    INT,
	HashKey                  VARBINARY (900),
	CONSTRAINT PK_TaxRule_Temp_Shard_3 PRIMARY KEY (TaxRuleId),
	CONSTRAINT IX_TaxRule_Temp_Shard_3_HashKey UNIQUE (HashKey)
	)
GO


IF OBJECT_ID ('dbo.CompanyContact_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.CompanyContact_Temp_Shard_3
GO

CREATE TABLE dbo.CompanyContact_Temp_Shard_3
	(
	CompanyContactId   INT NOT NULL,
	CompanyId          INT,
	CompanyContactCode VARCHAR (25),
	ContactRoleTypeId  TINYINT,
	FirstName          NVARCHAR (50),
	MiddleName         NVARCHAR (50),
	LastName           NVARCHAR (50),
	Title              NVARCHAR (50),
	Line1              NVARCHAR (50),
	Line2              NVARCHAR (50),
	Line3              NVARCHAR (50),
	City               NVARCHAR (50),
	Region             VARCHAR (3),
	PostalCode         VARCHAR (10),
	Country            NVARCHAR (50),
	Email              VARCHAR (50),
	Phone              VARCHAR (25),
	Phone2             VARCHAR (25),
	Fax                VARCHAR (25),
	CreatedUserId      INT NOT NULL,
	CreatedDate        DATETIME NOT NULL,
	ModifiedUserId     INT NOT NULL,
	ModifiedDate       DATETIME NOT NULL,
	CONSTRAINT PK_CompanyContact_Temp_Shard_3 PRIMARY KEY (CompanyContactId)
	)
GO





IF OBJECT_ID ('dbo.CompanyLocation_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.CompanyLocation_Temp_Shard_3
GO

CREATE TABLE dbo.CompanyLocation_Temp_Shard_3
	(
	CompanyLocationId   INT NOT NULL,
	CompanyId           INT NOT NULL,
	LocationCode        VARCHAR (50) NOT NULL,
	Description         NVARCHAR (255),
	AddressTypeId       TINYINT NOT NULL,
	AddressCategoryId   TINYINT NOT NULL,
	Line1               NVARCHAR (50) NOT NULL,
	Line2               NVARCHAR (50),
	Line3               NVARCHAR (50),
	City                NVARCHAR (50),
	Region              VARCHAR (3),
	PostalCode          VARCHAR (10) NOT NULL,
	Country             VARCHAR (2) NOT NULL,
	StateAssignedCode   VARCHAR (13),
	CreatedUserId       INT NOT NULL,
	CreatedDate         DATETIME NOT NULL,
	ModifiedUserId      INT NOT NULL,
	ModifiedDate        DATETIME NOT NULL,
	IsDefault           BIT NOT NULL,
	County              NVARCHAR (50),
	IsRegistered        BIT,
	DBAName             NVARCHAR (100),
	OutletName          NVARCHAR (100),
	StartDate           DATETIME,
	EndDate             DATETIME,
	LastTransactionDate DATETIME,
	RegisteredDate      DATETIME,
	CONSTRAINT PK_CompanyLocation_Temp_Shard_3 PRIMARY KEY (CompanyLocationId),
	CONSTRAINT IX_CompanyLocation_Temp_Shard_3 UNIQUE (CompanyId,LocationCode,AddressTypeId,StartDate)
	)
GO



IF OBJECT_ID ('dbo.ExemptCert_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.ExemptCert_Temp_Shard_3
GO

CREATE TABLE dbo.ExemptCert_Temp_Shard_3
	(
	ExemptCertId                 INT NOT NULL,
	CompanyId                    INT NOT NULL,
	CustomerCode                 NVARCHAR (50) NOT NULL,
	CustomerName                 NVARCHAR (100),
	Address1                     NVARCHAR (50),
	Address2                     NVARCHAR (50),
	Address3                     NVARCHAR (50),
	City                         NVARCHAR (50),
	Region                       VARCHAR (3),
	PostalCode                   VARCHAR (10),
	Country                      NVARCHAR (50),
	ExemptCertTypeId             TINYINT NOT NULL,
	DocumentRefNo                NVARCHAR (50),
	BusinessTypeId               TINYINT NOT NULL,
	BusinessTypeOtherDescription NVARCHAR (255),
	ExemptReasonId               CHAR (1) NOT NULL,
	ExemptReasonOtherDescription NVARCHAR (255),
	EffDate                      DATETIME,
	RegionsApplicable            VARCHAR (200),
	ExemptCertStatusId           TINYINT NOT NULL,
	CreatedDate                  DATETIME,
	LastTransactionDate          DATETIME,
	ExpiryDate                   DATETIME,
	CreatedUserId                INT NOT NULL,
	ModifiedDate                 DATETIME NOT NULL,
	ModifiedUserId               INT NOT NULL,
	CountryIssued                VARCHAR (2),
	AvaCertId                    VARCHAR (10),
	ExemptCertReviewStatusId     TINYINT,
	CONSTRAINT PK_ExemptCert_Temp_Shard_3 PRIMARY KEY (ExemptCertId)
	)
GO



IF OBJECT_ID ('dbo.Nexus_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.Nexus_Temp_Shard_3
GO

CREATE TABLE dbo.Nexus_Temp_Shard_3
	(
	NexusId                   INT NOT NULL,
	CompanyId                 INT NOT NULL,
	State                     VARCHAR (3) NOT NULL,
	JurisTypeId               VARCHAR (3),
	JurisCode                 VARCHAR (10) NOT NULL,
	JurisName                 VARCHAR (200) NOT NULL,
	EffDate                   DATETIME NOT NULL,
	EndDate                   DATETIME NOT NULL,
	CreatedUserId             INT NOT NULL,
	CreatedDate               DATETIME NOT NULL,
	ModifiedUserId            INT NOT NULL,
	ModifiedDate              DATETIME NOT NULL,
	ShortName                 VARCHAR (15),
	SignatureCode             VARCHAR (4),
	StateAssignedNo           VARCHAR (50),
	NexusTypeId               SMALLINT NOT NULL,
	Country                   CHAR (2),
	Sourcing                  CHAR (1),
	AccountingMethodId        TINYINT,
	HasLocalNexus             BIT,
	LocalNexusTypeId          SMALLINT,
	HasPermanentEstablishment BIT NOT NULL,
	TaxId                     VARCHAR (25),
	NexusTaxTypeGroupIdSK     INT,
	VATNumberTypeId           INT,
	VATOptionsId              INT,
	MOSSId                    VARCHAR (25),
	IsSellerImporterOfRecord  BIT,
	CONSTRAINT PK_Nexus_Temp_Shard_3 PRIMARY KEY NONCLUSTERED (NexusId),
	CONSTRAINT IX_Nexus_Temp_Shard_3 UNIQUE (CompanyId,State,JurisCode,JurisTypeId,JurisName,EffDate,NexusTypeId,NexusTaxTypeGroupIdSK)
	)
GO



IF OBJECT_ID ('dbo.TaxCode_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.TaxCode_Temp_Shard_3
GO

CREATE TABLE dbo.TaxCode_Temp_Shard_3
	(
	TaxCodeId        INT NOT NULL,
	TaxCode          VARCHAR (25),
	TaxCodeTypeId    VARCHAR (2) NOT NULL,
	CompanyId        INT,
	Description      NVARCHAR (500),
	CreatedUserId    INT NOT NULL,
	CreatedDate      DATETIME NOT NULL,
	ModifiedUserId   INT NOT NULL,
	ModifiedDate     DATETIME NOT NULL,
	ParentTaxCode    VARCHAR (25),
	IsPhysical       BIT NOT NULL,
	GoodsServiceCode BIGINT NOT NULL,
	EntityUseCode    VARCHAR (40),
	IsActive         BIT NOT NULL,
	IsSSTCertified   BIT,
	CONSTRAINT PK_TaxCode_Temp_Shard_3 PRIMARY KEY (TaxCodeId),
	CONSTRAINT IX_TaxCode_Temp_Shard_3 UNIQUE (TaxCode,CompanyId)
	)
GO



IF OBJECT_ID ('dbo.UPCCodeLookup_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.UPCCodeLookup_Temp_Shard_3
GO

CREATE TABLE dbo.UPCCodeLookup_Temp_Shard_3
	(
	UPCCodeLookupId INT NOT NULL,
	UPCCode         VARCHAR (50) NOT NULL,
	LegacyTaxCode   VARCHAR (50) NOT NULL,
	CompanyId       INT NOT NULL,
	UPCDescription  NVARCHAR (255) NOT NULL,
	CreatedUserId   INT NOT NULL,
	CreatedDate     DATETIME NOT NULL,
	ModifiedUserId  INT NOT NULL,
	ModifiedDate    DATETIME NOT NULL,
	EffDate         DATETIME NOT NULL,
	EndDate         DATETIME NOT NULL,
	Usage           INT NOT NULL,
	IsSystem        INT NOT NULL,
	CONSTRAINT PK_UPCCodeLookup_Temp_Shard_3 PRIMARY KEY (UPCCodeLookupId)
	)
GO





IF OBJECT_ID ('dbo.AvaCertServiceConfig_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.AvaCertServiceConfig_Temp_Shard_3
GO

CREATE TABLE dbo.AvaCertServiceConfig_Temp_Shard_3
	(
	AvaCertServiceConfigId INT NOT NULL,
	CompanyId              INT NOT NULL,
	AvaCertServiceStatusId TINYINT NOT NULL,
	IsUpdateEnabled        BIT NOT NULL,
	ClientCode             VARCHAR (50),
	OrgCode                VARCHAR (50),
	AllowPending           BIT,
	LastUpdate             DATETIME,
	CreatedUserId          INT NOT NULL,
	CreatedDate            DATETIME NOT NULL,
	ModifiedUserId         INT NOT NULL,
	ModifiedDate           DATETIME NOT NULL,
	CONSTRAINT PK_AvaCertServiceConfig_Temp_Shard_3 PRIMARY KEY (AvaCertServiceConfigId),
	CONSTRAINT IX_AvaCertServiceConfig_Temp_Shard_3 UNIQUE (CompanyId)
	)
GO





IF OBJECT_ID ('dbo.AvaCommsServiceConfig_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.AvaCommsServiceConfig_Temp_Shard_3
GO

CREATE TABLE dbo.AvaCommsServiceConfig_Temp_Shard_3
	(
	AvaCommsConfigId INT NOT NULL,
	CompanyId        INT NOT NULL,
	ClientId         INT NOT NULL,
	ClientProfileId  INT,
	Parameters       VARCHAR (1000),
	LastUpdate       DATETIME NOT NULL,
	CreatedUserId    INT NOT NULL,
	CreatedDate      DATETIME NOT NULL,
	ModifiedUserId   INT NOT NULL,
	ModifiedDate     DATETIME NOT NULL,
	CONSTRAINT PK_AvaCommsServiceConfig_Temp_Shard_3 PRIMARY KEY (AvaCommsConfigId),
	CONSTRAINT IX_AvaCommsServiceConfig_Temp_Shard_3 UNIQUE (CompanyId)
	)
GO





IF OBJECT_ID ('dbo.BRCompanySecurityCertificate_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.BRCompanySecurityCertificate_Temp_Shard_3
GO

CREATE TABLE dbo.BRCompanySecurityCertificate_Temp_Shard_3
	(
	CompanySecurityCertificateId INT NOT NULL,
	CompanyId                    INT NOT NULL,
	Password                     VARCHAR (256) NOT NULL,
	Certificate                  VARBINARY (max) NOT NULL,
	ExpirationDate               DATETIME NOT NULL,
	CreatedUserId                INT NOT NULL,
	CreatedDate                  DATETIME NOT NULL,
	ModifiedUserId               INT NOT NULL,
	ModifiedDate                 DATETIME NOT NULL,
	CONSTRAINT PK_BRCompanySecurityCertificate_Temp_Shard_3 PRIMARY KEY (CompanySecurityCertificateId)
	)
GO


IF OBJECT_ID ('dbo.BRTaxRegimeBuyerTypeConfig_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.BRTaxRegimeBuyerTypeConfig_Temp_Shard_3
GO

CREATE TABLE dbo.BRTaxRegimeBuyerTypeConfig_Temp_Shard_3
	(
	BRTaxRegimeBuyerTypeConfigId INT NOT NULL,
	CompanyId                    INT NOT NULL,
	Country                      VARCHAR (2) NOT NULL,
	JurisName                    VARCHAR (200) NOT NULL,
	JurisCode                    VARCHAR (10) NOT NULL,
	JurisTypeId                  VARCHAR (3) NOT NULL,
	TaxRuleTypeId                SMALLINT NOT NULL,
	TaxCodeId                    INT,
	TaxRegime                    VARCHAR (15),
	IsRateCumulative             BIT DEFAULT ((0)),
	BuyerType                    VARCHAR (5),
	RateTypeId                   VARCHAR (1) NOT NULL,
	Value                        DECIMAL (18, 6) NOT NULL,
	EffDate                      DATETIME NOT NULL,
	EndDate                      DATETIME NOT NULL,
	Description                  VARCHAR (255),
	CreatedUserId                INT NOT NULL,
	CreatedDate                  DATETIME NOT NULL,
	ModifiedUserId               INT NOT NULL,
	ModifiedDate                 DATETIME NOT NULL,
	CONSTRAINT PK_BRTaxRegimeBuyerTypeConfig_Temp_Shard_3 PRIMARY KEY (BRTaxRegimeBuyerTypeConfigId)
	)
GO




IF OBJECT_ID ('dbo.CompanyReturn_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.CompanyReturn_Temp_Shard_3
GO

CREATE TABLE dbo.CompanyReturn_Temp_Shard_3
	(
	CompanyReturnId       BIGINT NOT NULL,
	CompanyId             INT NOT NULL,
	ReturnName            VARCHAR (50) NOT NULL,
	FilingFrequencyId     TINYINT NOT NULL,
	Months                SMALLINT NOT NULL,
	RegistrationId        VARCHAR (50) NOT NULL,
	EIN                   VARCHAR (50) NOT NULL,
	Line1                 VARCHAR (50) NOT NULL,
	Line2                 VARCHAR (50) NOT NULL,
	City                  VARCHAR (50) NOT NULL,
	Region                VARCHAR (3) NOT NULL,
	PostalCode            VARCHAR (25) NOT NULL,
	Country               VARCHAR (2) NOT NULL,
	Phone                 VARCHAR (50) NOT NULL,
	Description           VARCHAR (1000),
	LegalEntityName       VARCHAR (75) NOT NULL,
	EffDate               DATETIME NOT NULL,
	EndDate               DATETIME NOT NULL,
	CreatedUserId         INT NOT NULL,
	CreatedDate           DATETIME NOT NULL,
	ModifiedUserId        INT NOT NULL,
	ModifiedDate          DATETIME NOT NULL,
	FilingCalendarID      INT NOT NULL,
	FilingTypeId          TINYINT NOT NULL,
	EFilePassword         VARCHAR (40),
	PrepayPercentage      TINYINT NOT NULL,
	TaxTypeId             VARCHAR (1) NOT NULL,
	Note                  VARCHAR (1000),
	AlSignOn              VARCHAR (25),
	AlAccessCode          VARCHAR (25),
	MeBusinessCode        VARCHAR (25),
	IaBen                 VARCHAR (25),
	CtReg                 VARCHAR (25),
	Other1Name            VARCHAR (50),
	Other1Value           VARCHAR (50),
	Other2Name            VARCHAR (50),
	Other2Value           VARCHAR (50),
	Other3Name            VARCHAR (50),
	Other3Value           VARCHAR (50),
	LocationCode          VARCHAR (50),
	OutletTypeId          TINYINT NOT NULL,
	LocalRegistrationId   VARCHAR (50),
	EfileUsername         VARCHAR (254),
	PaymentCurrency       VARCHAR (3),
	FixedPrepaymentAmount DECIMAL (18, 4),
	CONSTRAINT PK_CompanyReturn_Temp_Shard_3 PRIMARY KEY (CompanyReturnId),
	CONSTRAINT IX_CompanyReturn_Temp_Shard_3 UNIQUE (CompanyId,ReturnName,EffDate,RegistrationId)
	)
GO


IF OBJECT_ID ('dbo.CompanyTaxForm_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.CompanyTaxForm_Temp_Shard_3
GO

CREATE TABLE dbo.CompanyTaxForm_Temp_Shard_3
	(
	CompanyTaxFormId  INT NOT NULL,
	CompanyId         INT NOT NULL,
	LibraryTaxFormId  INT NOT NULL,
	FilingStatusId    TINYINT NOT NULL,
	CloseDate         DATETIME NOT NULL,
	FinalDate         DATETIME,
	FilingFrequencyId TINYINT NOT NULL,
	ModifiedUserId    INT NOT NULL,
	TaxFormPDF        NVARCHAR (100),
	EffDate           DATETIME NOT NULL,
	EndDate           DATETIME NOT NULL,
	CreatedUserId     INT NOT NULL,
	CreatedDate       DATETIME NOT NULL,
	ModifiedDate      DATETIME NOT NULL,
	CONSTRAINT PK_CompanyTaxForm_Temp_Shard_3 PRIMARY KEY (CompanyTaxFormId)
	)
GO




IF OBJECT_ID ('dbo.Return_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.[Return_Temp_Shard_3]
GO

CREATE TABLE dbo.[Return_Temp_Shard_3]
	(
	ReturnId       INT NOT NULL,
	TransmissionId INT NOT NULL,
	CompanyId      INT NOT NULL,
	ReturnTypeId   SMALLINT NOT NULL,
	ReturnStatusId SMALLINT NOT NULL,
	BeginDate      DATETIME,
	EndDate        DATETIME,
	FiledDate      DATETIME,
	CreatedDate    DATETIME,
	ModifiedDate   DATETIME,
	JurisCode      VARCHAR (10),
	JurisName      VARCHAR (200) NOT NULL,
	JurisTypeId    VARCHAR (3),
	Region         CHAR (2),
	ReturnName     VARCHAR (50),
	CONSTRAINT PK_Return_Temp_Shard_3 PRIMARY KEY (ReturnId)
	)
GO




IF OBJECT_ID ('dbo.TaxRuleProductDetail_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.TaxRuleProductDetail_Temp_Shard_3
GO

CREATE TABLE dbo.TaxRuleProductDetail_Temp_Shard_3
	(
	TaxRuleProductDetailId INT NOT NULL,
	TaxRuleId              INT NOT NULL,
	ProductId              BIGINT NOT NULL,
	ProductTypeId          INT NOT NULL,
	EffDate                DATETIME,
	EndDate                DATETIME,
	ModifiedDate           DATETIME,
	CONSTRAINT PK_TaxRuleProductDetailId_Temp_Shard_3 PRIMARY KEY (TaxRuleProductDetailId)
 	)
GO






IF OBJECT_ID ('dbo.TaxCodeAttribute_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.TaxCodeAttribute_Temp_Shard_3
GO

CREATE TABLE dbo.TaxCodeAttribute_Temp_Shard_3
	(
	TaxCodeAttributeId INT NOT NULL,
	TaxCodeId          INT NOT NULL,
	Name               VARCHAR (50) NOT NULL,
	Value              VARCHAR (50) NOT NULL,
	CreatedUserId      INT NOT NULL,
	CreatedDate        DATETIME NOT NULL,
	ModifiedUserId     INT NOT NULL,
	ModifiedDate       DATETIME NOT NULL,
	UOMId              INT,
	CONSTRAINT PK_TaxCodeAttribute_Temp_Shard_3 PRIMARY KEY (TaxCodeAttributeId)
	)
GO



IF OBJECT_ID ('dbo.TaxCodeCategorization_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.TaxCodeCategorization_Temp_Shard_3
GO

CREATE TABLE dbo.TaxCodeCategorization_Temp_Shard_3
	(
	TaxCodeCategorizationId INT NOT NULL,
	TaxCodeId               INT,
	Country                 VARCHAR (2) NOT NULL,
	TaxCodeTypeId           VARCHAR (2) NOT NULL,
	IsPhysical              BIT NOT NULL,
	EffDate                 DATETIME NOT NULL,
	EndDate                 DATETIME NOT NULL,
	CreatedUserId           INT NOT NULL,
	CreatedDate             DATETIME NOT NULL,
	ModifiedUserId          INT NOT NULL,
	ModifiedDate            DATETIME NOT NULL,
	CONSTRAINT PK_TaxCodeCategorization_Temp_Shard_3 PRIMARY KEY (TaxCodeCategorizationId)
	)
GO



IF OBJECT_ID ('dbo.Item_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.Item_Temp_Shard_3
GO

CREATE TABLE dbo.Item_Temp_Shard_3
	(
	ItemId         BIGINT NOT NULL,
	ItemCode       VARCHAR (50),
	CompanyId      INT NOT NULL,
	TaxCodeId      INT NOT NULL,
	Description    NVARCHAR (255),
	CreatedUserId  INT NOT NULL,
	CreatedDate    DATETIME NOT NULL,
	ModifiedUserId INT NOT NULL,
	ModifiedDate   DATETIME NOT NULL,
	CONSTRAINT PK_Item_Temp_Shard_3 PRIMARY KEY (ItemId),
	CONSTRAINT IX_Item_Temp_Shard_3 UNIQUE (CompanyId,ItemCode)
	)
GO






IF OBJECT_ID ('dbo.ItemAttribute_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.ItemAttribute_Temp_Shard_3
GO

CREATE TABLE dbo.ItemAttribute_Temp_Shard_3
	(
	ItemAttributeId BIGINT NOT NULL,
	ItemId          BIGINT NOT NULL,
	Name            VARCHAR (50) NOT NULL,
	Value           VARCHAR (50) NOT NULL,
	CreatedUserId   INT NOT NULL,
	CreatedDate     DATETIME NOT NULL,
	ModifiedUserId  INT NOT NULL,
	ModifiedDate    DATETIME NOT NULL,
	UOMId           INT,
	CONSTRAINT PK_ItemAttribute_Temp_Shard_3 PRIMARY KEY (ItemAttributeId)
	)
GO



IF OBJECT_ID ('dbo.DocumentAddress_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.DocumentAddress_Temp_Shard_3
GO

CREATE TABLE dbo.DocumentAddress_Temp_Shard_3
	(
	DocumentAddressId      BIGINT NOT NULL,
	DocumentId             BIGINT,
	Line1                  VARCHAR (50),
	City                   VARCHAR (50),
	Country                VARCHAR (50),
	Region                 VARCHAR (3),
	PostalCode             VARCHAR (11),
	BoundaryLevelId        TINYINT NOT NULL,
	JurisCode              VARCHAR (10),
	County                 VARCHAR (50),
	CitySignature          VARCHAR (4),
	TaxRegionId            INT NOT NULL,
	GeoCode                VARCHAR (10) NOT NULL,
	GeocodeTypeId          VARCHAR (20),
	ValidateStatusId       VARCHAR (20),
	Latitude               FLOAT,
	Longitude              FLOAT,
	DocumentQueueAddressId BIGINT,
	AddressLine1           VARCHAR (100),
	AddressLine2           VARCHAR (100),
	AddressLine3           VARCHAR (100),
	CONSTRAINT PK_DocumentAddress_Temp_Shard_3 PRIMARY KEY (DocumentAddressId)
	)
GO



IF OBJECT_ID ('dbo.DocumentProperty_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.DocumentProperty_Temp_Shard_3
GO

CREATE TABLE dbo.DocumentProperty_Temp_Shard_3
	(
	DocumentPropertyId BIGINT NOT NULL,
	DocumentId         BIGINT NOT NULL,
	ReferenceCode      VARCHAR (1024) NOT NULL,
	VATNumberTypeId    INT,
	CONSTRAINT PK_DocumentProperty_Temp_Shard_3 PRIMARY KEY (DocumentPropertyId)
	)
GO


IF OBJECT_ID ('dbo.DocumentLineParameterBag_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.DocumentLineParameterBag_Temp_Shard_3
GO

CREATE TABLE dbo.DocumentLineParameterBag_Temp_Shard_3
	(
	DocumentLineParameterBagId BIGINT NOT NULL,
	DocumentLineId             BIGINT NOT NULL,
	Name                       VARCHAR (50) NOT NULL,
	Value                      VARCHAR (50) NOT NULL,
	UOMId                      INT,
	UOMIdSystem                INT,
	ValueSystem                VARCHAR (50),
	CONSTRAINT PK_DocumentLineParameterBag_Temp_Shard_3 PRIMARY KEY (DocumentLineParameterBagId)
	)
GO



IF OBJECT_ID ('dbo.DocumentLineProperty_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.DocumentLineProperty_Temp_Shard_3
GO

CREATE TABLE dbo.DocumentLineProperty_Temp_Shard_3
	(
	DocumentLinePropertyId BIGINT NOT NULL,
	DocumentLineId         BIGINT NOT NULL,
	VATCode                VARCHAR (50) NOT NULL,
	VATNumberTypeId        INT NOT NULL,
	HSCodeId               BIGINT,
	HSCode                 VARCHAR (25),
	HSCodeUsed             VARCHAR (25),
	CIF                    MONEY,
	CONSTRAINT PK_DocumentLineProperty_Temp_Shard_3 PRIMARY KEY (DocumentLinePropertyId)
	)
GO



IF OBJECT_ID ('dbo.DocumentLineDetailProperty_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.DocumentLineDetailProperty_Temp_Shard_3
GO

CREATE TABLE dbo.DocumentLineDetailProperty_Temp_Shard_3
	(
	DocumentLineDetailId     BIGINT NOT NULL,
	TaxTypeMappingId         INT NOT NULL,
	RateTypeTaxTypeMappingId INT NOT NULL,
	IsFee                    BIT,
	TaxAuthorityId           INT,
	ReportLevel              INT,
	CONSTRAINT PK_DocumentLineDetailProperty_Temp_Shard_3 PRIMARY KEY (DocumentLineDetailId)
	)
GO


IF OBJECT_ID ('dbo.FormServiceConfig_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.FormServiceConfig_Temp_Shard_3
GO

CREATE TABLE [dbo].[FormServiceConfig_Temp_Shard_3](
	[AccountId] [int] NOT NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedUserId] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ReviewActionDefault] [tinyint] NOT NULL,
	[ZeroDollarActionDefault] [tinyint] NOT NULL,
	[WorksheetDay] [tinyint] NOT NULL,
    CONSTRAINT [PK_FormServiceConfigFormServiceConfig_Temp_Shard_3] PRIMARY KEY CLUSTERED ([AccountId] ASC)
    )
GO






IF OBJECT_ID ('dbo.AccountFeatureList_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.AccountFeatureList_Temp_Shard_3
GO

CREATE TABLE [dbo].[AccountFeatureList_Temp_Shard_3](
	[AccountId] [int] NOT NULL,
	[FeatureListId] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
    CONSTRAINT [PK_AccountFeatureList_Temp_Shard_3] PRIMARY KEY CLUSTERED ([AccountId] ASC,[FeatureListId] ASC)
	)
GO 




IF OBJECT_ID ('dbo.CombinedHSTConfig_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.CombinedHSTConfig_Temp_Shard_3
GO

CREATE TABLE [dbo].[CombinedHSTConfig_Temp_Shard_3](
	[CombinedHSTConfigId] [int] NOT NULL,
	[AccountId] [int] NULL,
	[ExcludeCompanyCodes] [varchar](MAX) NULL,
	[EffDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
    CONSTRAINT [PK_CombinedHSTConfig_Temp_Shard_3] PRIMARY KEY CLUSTERED ([CombinedHSTConfigId] ASC),
    CONSTRAINT [IX_CombinedHSTConfig_Temp_Shard_3_AccountId_EffDate] UNIQUE NONCLUSTERED ([AccountId] ASC,[EffDate] ASC)
    )
GO






IF OBJECT_ID ('dbo.MaxLine_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.MaxLine_Temp_Shard_3
GO

CREATE TABLE [dbo].[MaxLine_Temp_Shard_3](
	[AccountId] [int] NULL,
	[MaxLines] [int] NULL,
	[MaxLineId] [bigint] NOT NULL,
    CONSTRAINT [PK_MaxLine_Temp_Shard_3_MaxLineId] PRIMARY KEY CLUSTERED ([MaxLineId] ASC)
	)
GO




IF OBJECT_ID ('dbo.CompanySetting_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.CompanySetting_Temp_Shard_3
GO

CREATE TABLE [dbo].[CompanySetting_Temp_Shard_3](
	[CompanySettingId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Set] [varchar](25) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Value] [varchar](200) NULL,
    CONSTRAINT [PK_CompanySetting_Temp_Shard_3] PRIMARY KEY CLUSTERED ([CompanySettingId] ASC)
	)



IF OBJECT_ID ('dbo.CompanyDistanceThreshold_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.CompanyDistanceThreshold_Temp_Shard_3
GO

CREATE TABLE [dbo].[CompanyDistanceThreshold_Temp_Shard_3](
	[CompanyDistanceThresholdId] [bigint] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[OriginCountry] [varchar](2) NOT NULL,
	[DestinationCountry] [varchar](2) NOT NULL,
	[EffDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[HasDistanceThresholdExceeded] [bit] NOT NULL,
	[DistanceThresholdType] [varchar](20) NULL,
    CONSTRAINT [PK_CompanyDistanceThreshold_Temp_Shard_3] PRIMARY KEY CLUSTERED ([CompanyDistanceThresholdId] ASC),
    CONSTRAINT [IX_CompanyDistanceThreshold_Temp_Shard_3] UNIQUE NONCLUSTERED ([CompanyId] ASC,	[OriginCountry] ASC,[DestinationCountry] ASC,[EffDate] ASC,[EndDate] ASC,	[HasDistanceThresholdExceeded] ASC,[DistanceThresholdType] ASC)
    )
GO



IF OBJECT_ID ('dbo.CompanyRPSSeries_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.CompanyRPSSeries_Temp_Shard_3
GO

CREATE TABLE [dbo].[CompanyRPSSeries_Temp_Shard_3](
	[CompanyRPSSeriesId] [bigint] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[TaxRegionId] [bigint] NULL,
	[CompanyRPSSeriesTypeId] [tinyint] NULL,
	[SeriesName] [varchar](10) NULL,
	[FirstRPSNumber] [bigint] NOT NULL,
	[NextRPSNumber] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
    CONSTRAINT [PK_CompanyRPSSeries_Temp_Shard_3] PRIMARY KEY CLUSTERED ([CompanyRPSSeriesId] ASC)
    )
GO




IF OBJECT_ID ('dbo.SpecialTaxDistrictOverride_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.SpecialTaxDistrictOverride_Temp_Shard_3
GO

CREATE TABLE [dbo].[SpecialTaxDistrictOverride_Temp_Shard_3](
	[BoundaryOverrideId] [int] NOT NULL,
	[JurisCode] [varchar](10) NOT NULL,
	[SignatureCode] [varchar](4) NOT NULL,
	[JurisName] [varchar](200) NOT NULL,
	CONSTRAINT [PK_SpecialTaxDistrictOverride_Temp_Shard_3] PRIMARY KEY CLUSTERED ([BoundaryOverrideId] ASC,[JurisCode] ASC,[SignatureCode] ASC)
	)
GO



IF OBJECT_ID ('dbo.SubscriptionSetting_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.SubscriptionSetting_Temp_Shard_3
GO

CREATE TABLE [dbo].[SubscriptionSetting_Temp_Shard_3](
	[SubscriptionSettingId] [int] NOT NULL,
	[SubscriptionId] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Value] [varchar](200) NOT NULL,
	CONSTRAINT [PK_SubscriptionSetting_Temp_Shard_3] PRIMARY KEY CLUSTERED ([SubscriptionSettingId] ASC)
	)
GO



IF OBJECT_ID ('dbo.CompanyLocationSetting_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.CompanyLocationSetting_Temp_Shard_3
GO

CREATE TABLE [dbo].[CompanyLocationSetting_Temp_Shard_3](
	[CompanyLocationSettingId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CompanyLocationId] [int] NOT NULL,
	[CompanyLocationSettingConfigId] [int] NOT NULL,
	[CompanyLocationSettingTypeValue] [nvarchar](50) NULL,
	CONSTRAINT [PK_CompanyLocationSetting_Temp_Shard_3] PRIMARY KEY CLUSTERED ([CompanyLocationSettingId] ASC),
	CONSTRAINT [IX_CompanyLocationSetting_Temp_Shard_3] UNIQUE NONCLUSTERED ([CompanyLocationId] ASC,[CompanyLocationSettingConfigId] ASC)
	)
GO


IF OBJECT_ID ('dbo.ExemptCertDetail_Temp_Shard_3') IS NOT NULL
	DROP TABLE dbo.ExemptCertDetail_Temp_Shard_3
GO

CREATE TABLE [dbo].[ExemptCertDetail_Temp_Shard_3](
	[ExemptCertDetailId] [int] NOT NULL,
	[ExemptCertId] [int] NOT NULL,
	[StateFIPS] [varchar](2) NOT NULL,
	[Region] [varchar](3) NULL,
	[IdNo] [varchar](100) NULL,
	[Country] [varchar](2) NULL,
	[EndDate] [datetime] NULL,
	[IdType] [nvarchar](50) NULL,
	[IsTaxCodeListExclusionList] [bit] NULL,
	CONSTRAINT [PK_ExemptCertDetail] PRIMARY KEY CLUSTERED ([ExemptCertDetailId] ASC),
	CONSTRAINT [IX_ExemptCertDetail] UNIQUE NONCLUSTERED ([ExemptCertId] ASC,[Region] ASC)
	)
GO



