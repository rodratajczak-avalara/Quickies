SELECT ApplicationName, LastSynch
FROM ShardAggregation.dbo.LastSynch

SELECT * FROM ShardAggregation.dbo.BCPBatch
WHERE ApplicationName = 'AvaShardAggregator_Temp_Shard_3'
ORDER BY BCPBatchId DESC 

SELECT * FROM ShardAggregation.dbo.BCPBatchLog
WHERE BCPBatchId = 12959
ORDER BY BCPBatchLogId DESC

SELECT bbl.* --, at.TableName
FROM ShardAggregation.dbo.BCPBatchLog bbl
--INNER JOIN ShardAggregation.dbo.AggregationTable at
--ON bbl.AggregationTableId = at.AggregationTableId
WHERE bbl.BCPBatchId IN (SELECT BCPBatchId FROM ShardAggregation.dbo.BCPBatch WHERE ApplicationName='AvaShardAggregator_Temp_Shard_3')
AND bbl.BatchProcessId = 1
--AND bbl.AggregationTableId IN (34, 48, 52, 57)
ORDER BY bbl.BCPBatchId DESC   --bbl.BatchProcessTime DESC 

SELECT TOP 5 * 
FROM ShardAggregation.dbo.BCPBatchError
ORDER BY BCPBatchErrorId DESC


DELETE FROM ShardAggregation.dbo.BCPBatch
--SELECT * FROM ShardAggregation.dbo.BCPBatch
WHERE BatchStartTime < '6/26/2018 00:00:00.000'

DELETE FROM ShardAggregation.dbo.BCPBatchLog
WHERE BCPBatchId NOT IN 
(
	SELECT BCPBatchId
	FROM ShardAggregation.dbo.BCPBatch
)

DELETE FROM ShardAggregation.dbo.BCPBatchError
WHERE BCPBatchId NOT IN
(
	SELECT BCPBatchId
	FROM ShardAggregation.dbo.BCPBatch
)



SELECT * 
FROM ShardAggregation.dbo.AggregationTable
WHERE Enabled=0

















SELECT SUM(BatchProcessTime) , Max(BatchProcessTime), AVG(BatchProcessTime)
FROM ShardAggregation.dbo.BCPBatchLog
WHERE AggregationTableId = 0
AND BcpBatchId IN 
(
 	SELECT BCPBatchId 
 	FROM ShardAggregation.dbo.BCPBatch
	WHERE ApplicationName = 'AvaShardAggregator_Temp_Shard_3'
)



SELECT sum(BatchProcessTime)
FROM ShardAggregation.dbo.BCPBatchLog
WHERE AggregationTableId IN 
(
	SELECT AggregationTableId
	FROM ShardAggregation.dbo.AggregationTable
	WHERE TableName LIKE '%Document%'
)
AND BcpBatchId IN 
(
 	SELECT BCPBatchId 
 	FROM ShardAggregation.dbo.BCPBatch
	WHERE ApplicationName = 'AvaShardAggregator_Temp_Shard_3'
)  
AND BcpBatchId IN 
(
 	SELECT BCPBatchId 
 	FROM ShardAggregation.dbo.BCPBatchLog
	WHERE AggregationTableId = 0
)  
