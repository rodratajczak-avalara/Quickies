DECLARE @cmd varchar(4000)
DECLARE cmds CURSOR FOR
SELECT 'Truncate table AvaTaxAccount_Repl.dbo.[' + at.TableName + ']'
FROM ShardAggregation.dbo.AggregationTable at
WHERE Enabled = 0
AND at.[Database] = 'AvaTaxAccount'

OPEN cmds
WHILE 1 = 1
BEGIN
    FETCH cmds INTO @cmd
    IF @@fetch_status != 0 BREAK
    EXEC(@cmd)
END
CLOSE cmds;
DEALLOCATE cmds

