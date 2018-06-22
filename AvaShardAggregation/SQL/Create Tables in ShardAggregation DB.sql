IF OBJECT_ID ('dbo.AggregationTable') IS NOT NULL
	DROP TABLE dbo.AggregationTable
GO

CREATE TABLE dbo.AggregationTable
	(
	AggregationTableId INT IDENTITY NOT NULL,
	[Database]         NVARCHAR (128) NOT NULL,
	[Schema]           NVARCHAR (128) NOT NULL,
	TableName          NVARCHAR (128) NOT NULL,
	ParentTableId      INT,
	FullTable          BIT,
	ModifiedDateExists BIT,
	ExecutionGroup     TINYINT,
	RemoveDuplicate    BIT,
	Enabled            BIT DEFAULT ((1)),
	CreatedUserId      BIGINT DEFAULT ((0)) NOT NULL,
	CreatedDate        DATETIME DEFAULT (getutcdate()) NOT NULL,
	ModifiedUserId     BIGINT DEFAULT ((0)) NOT NULL,
	ModifiedDate       DATETIME DEFAULT (getutcdate()) NOT NULL,
	CONSTRAINT PK_AggregationTable PRIMARY KEY (AggregationTableId),
	CONSTRAINT IX_AggregationTable UNIQUE ([Database],[Schema],TableName)
	)
GO



IF OBJECT_ID ('dbo.LastSynch') IS NOT NULL
	DROP TABLE dbo.LastSynch
GO

CREATE TABLE dbo.LastSynch
	(
	ApplicationName NVARCHAR (255) NOT NULL,
	LastSynch       DATETIME NOT NULL,
	CONSTRAINT PK_LastSynch PRIMARY KEY (ApplicationName)
	)
GO
