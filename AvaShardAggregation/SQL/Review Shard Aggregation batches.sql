SELECT ApplicationName, LastSynch
FROM ShardAggregation.dbo.LastSynch

SELECT * FROM ShardAggregation.dbo.BCPBatch
WHERE ApplicationName = 'AvaShardAggregator_Temp_Shard_3'
ORDER BY BCPBatchId DESC 

SELECT * FROM ShardAggregation.dbo.BCPBatchLog
WHERE BCPBatchId = 12844
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



SELECT * 
FROM ShardAggregation.dbo.AggregationTable
WHERE Enabled=0



