IF OBJECT_ID ('dbo.TaxRule_Temp_Shard_1') IS NOT NULL
	DROP TABLE dbo.TaxRule_Temp_Shard_1
GO

CREATE TABLE dbo.TaxRule_Temp_Shard_1
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
	IsAllJuris               BIT NOT NULL,
	Value                    DECIMAL (18, 6) NOT NULL,
	Cap                      DECIMAL (18, 6) NOT NULL,
	Threshold                DECIMAL (18, 6) NOT NULL,
	Options                  VARCHAR (510),
	EffDate                  DATETIME NOT NULL,
	EndDate                  DATETIME NOT NULL,
	Description              NVARCHAR (255),
	CreatedUserId            INT NOT NULL,
	CreatedDate              DATETIME NOT NULL,
	ModifiedUserId           INT NOT NULL,
	ModifiedDate             DATETIME NOT NULL,
	State                    VARCHAR (3) NOT NULL,
	CountyFIPS               VARCHAR (3),
	IsSTPro                  BIT NOT NULL,
	Country                  CHAR (2),
	Sourcing                 CHAR (1),
	UnitOfBasisId            BIGINT,
	AttributeOptions         VARCHAR (510),
	ReturnsRateID            INT,
	ReturnsDeductionID       INT,
	ReturnsTaxTypeID         INT,
	AttributeApplicability   VARCHAR (510),
	TaxTypeMappingId         INT,
	RateTypeTaxTypeMappingId INT,
	NonPassThruExpression    VARCHAR (500) DEFAULT (NULL),
	CurrencyCode             CHAR (3) NOT NULL,
	PreferredProgramId       INT,
	UOMId                    INT,
	HashKey                  VARBINARY (900),
	CONSTRAINT PK_TaxRule_Temp_Shard_1 PRIMARY KEY (TaxRuleId),
	CONSTRAINT IX_TaxRule_Temp_Shard_1 UNIQUE (HashKey)
	)
GO


