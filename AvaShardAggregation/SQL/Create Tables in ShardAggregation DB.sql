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
	ApplicationName NVARCHAR (450) NOT NULL,
	LastSynch       DATETIME NOT NULL,
	CONSTRAINT PK_LastSynch PRIMARY KEY (ApplicationName)
	)
GO

INSERT INTO dbo.LastSynch (ApplicationName, LastSynch)
VALUES ('AvaShardAggregator', '2018-06-23 03:28:55.047')
GO



IF OBJECT_ID ('dbo.BCPBatch') IS NOT NULL
	DROP TABLE dbo.BCPBatch
GO

CREATE TABLE dbo.BCPBatch
	(
	BCPBatchId	BIGINT IDENTITY(1,1) NOT NULL,
	ApplicationName NVARCHAR (450) NOT NULL,
	BatchStartTime  DATETIME NOT NULL,
	CONSTRAINT PK_BCPBatch PRIMARY KEY (BCPBatchId)
	)
GO



IF OBJECT_ID ('dbo.BatchProcess') IS NOT NULL
	DROP TABLE dbo.BatchProcess
GO

CREATE TABLE dbo.BatchProcess
	(
	BatchProcessId	TINYINT NOT NULL,
	BatchProcessName NVARCHAR (50) NOT NULL,
	BatchProcessDescription  NVARCHAR(MAX) NOT NULL,
	CONSTRAINT PK_BatchProcess PRIMARY KEY (BatchProcessId)
	)
GO

INSERT INTO dbo.BatchProcess (BatchProcessId, BatchProcessName, BatchProcessDescription)
VALUES (1, 'TotalProcessing', 'Time to complete the entire batch')
GO

INSERT INTO dbo.BatchProcess (BatchProcessId, BatchProcessName, BatchProcessDescription)
VALUES (2, 'QueryModified', 'Time to query Modified Records from the source table')
GO

INSERT INTO dbo.BatchProcess (BatchProcessId, BatchProcessName, BatchProcessDescription)
VALUES (3, 'BulkCopy', 'Time to bulkcopy to temp table from the source database')
GO

INSERT INTO dbo.BatchProcess (BatchProcessId, BatchProcessName, BatchProcessDescription)
VALUES (4, 'RemoveDuplicates', 'Time to remove natural key duplicates found between the temp and destination tables')
GO

INSERT INTO dbo.BatchProcess (BatchProcessId, BatchProcessName, BatchProcessDescription)
VALUES (5, 'Merge', 'Time to merge temp table with the destination table')
GO






IF OBJECT_ID ('dbo.BCPBatchLog') IS NOT NULL
	DROP TABLE dbo.BCPBatchLog
GO

CREATE TABLE dbo.BCPBatchLog
	(
	BCPBatchLogId BIGINT IDENTITY(1,1) NOT NULL,
	BCPBatchId BIGINT NOT NULL, 
	AggregationTableId INT,
	BatchProcessId TINYINT NOT NULL,
	BatchProcessTime  INT NOT NULL,
	CONSTRAINT PK_BCPBatchLog PRIMARY KEY (BCPBatchLogId)
	)
GO


IF OBJECT_ID ('dbo.BCPBatchError') IS NOT NULL
	DROP TABLE dbo.BCPBatchError
GO

CREATE TABLE dbo.BCPBatchError
	(
	BCPBatchErrorId      BIGINT IDENTITY NOT NULL,
	BCPBatchId         BIGINT NOT NULL,
	AggregationTableId INT NOT NULL,
	ErrorMessage       NVARCHAR(MAX) NOT NULL,
	CONSTRAINT PK_BCPBatchError PRIMARY KEY (BCPBatchErrorId)
	)
GO
