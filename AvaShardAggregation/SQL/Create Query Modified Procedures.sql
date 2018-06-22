
CREATE OR ALTER PROCEDURE sp_Query_DocumentLine_Modified_Records
	@LastCheckTime		DATETIME,
	@CurrentCheckTime	DATETIME
AS 
BEGIN
	
	SELECT dl.*
	FROM Document d
	LEFT JOIN DocumentLine dl
	ON d.DocumentId = dl.DocumentId
	WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
END 
GO

EXEC sp_Query_DocumentLine_Modified_Records @LastCheckTime = '2018/06/22 15:40:00', @CurrentCheckTime='2018/06/22 15:45:00'
