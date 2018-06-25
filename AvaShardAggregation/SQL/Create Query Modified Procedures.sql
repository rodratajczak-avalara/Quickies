
CREATE OR ALTER PROCEDURE sp_Query_DocumentLine_Modified_Records
	@LastCheckTime		DATETIME,
	@CurrentCheckTime	DATETIME
AS 
BEGIN
	SELECT dl.*
	FROM DocumentLine dl WITH (NOLOCK)
	WHERE dl.DocumentId IN 
	(	
		SELECT d.DocumentId
		FROM Document d WITH (NOLOCK)
		WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
	)
END 
GO



CREATE OR ALTER PROCEDURE sp_Query_DocumentAddress_Modified_Records
	@LastCheckTime		DATETIME,
	@CurrentCheckTime	DATETIME
AS 
BEGIN
	SELECT da.*
	FROM DocumentAddress da WITH (NOLOCK)
	WHERE da.DocumentId IN 
	(	
		SELECT d.DocumentId
		FROM Document d WITH (NOLOCK)
		WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
	)
END 
GO

CREATE OR ALTER PROCEDURE sp_Query_DocumentParameterBag_Modified_Records
	@LastCheckTime		DATETIME,
	@CurrentCheckTime	DATETIME
AS 
BEGIN
	SELECT dpb.*
    FROM DocumentParameterBag dpb WITH (NOLOCK)
    WHERE dpb.DocumentId IN 
	(	
		SELECT d.DocumentId
		FROM Document d WITH (NOLOCK)
		WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
	)
END
GO


CREATE OR ALTER PROCEDURE sp_Query_DocumentProperty_Modified_Records
	@LastCheckTime		DATETIME,
	@CurrentCheckTime	DATETIME
AS 
BEGIN
    SELECT dp.*
    FROM DocumentProperty dp WITH (NOLOCK)
    WHERE dp.DocumentId IN 
	(	
		SELECT d.DocumentId
		FROM Document d WITH (NOLOCK)
		WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
	)
END
GO



CREATE OR ALTER PROCEDURE sp_Query_DocumentLineParameterBag_Modified_Records
	@LastCheckTime		DATETIME,
	@CurrentCheckTime	DATETIME
AS 
BEGIN
    SELECT dlpb.*
    FROM DocumentLineParameterBag dlpb
    WHERE dlpb.DocumentLineId IN 
    (
	    SELECT dl.DocumentLineId
		FROM DocumentLine dl WITH (NOLOCK)
		WHERE dl.DocumentId IN 
		(	
			SELECT d.DocumentId
			FROM Document d WITH (NOLOCK)
			WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
		)
	)
END
GO



CREATE OR ALTER PROCEDURE sp_Query_DocumentLineProperty_Modified_Records
	@LastCheckTime		DATETIME,
	@CurrentCheckTime	DATETIME
AS 
BEGIN
	SELECT dlp.*
    FROM DocumentLineProperty dlp
    WHERE dlp.DocumentLineId IN 
    (
	    SELECT dl.DocumentLineId
		FROM DocumentLine dl WITH (NOLOCK)
		WHERE dl.DocumentId IN 
		(	
			SELECT d.DocumentId
			FROM Document d WITH (NOLOCK)
			WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
		)
	)

END
GO



CREATE OR ALTER PROCEDURE sp_Query_DocumentLineDetail_Modified_Records
	@LastCheckTime		DATETIME,
	@CurrentCheckTime	DATETIME
AS 
BEGIN
	SELECT dld.*
    FROM DocumentLineDetail dld WITH (NOLOCK)
    WHERE dld.DocumentLineId IN 
    (
	    SELECT dl.DocumentLineId
		FROM DocumentLine dl WITH (NOLOCK)
		WHERE dl.DocumentId IN 
		(	
			SELECT d.DocumentId
			FROM Document d WITH (NOLOCK)
			WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
		)
	)

END 
GO 



CREATE OR ALTER PROCEDURE sp_Query_DocumentLineDetailProperty_Modified_Records
	@LastCheckTime		DATETIME,
	@CurrentCheckTime	DATETIME
AS 
BEGIN
	SELECT dldp.*
    FROM DocumentLineDetailProperty dldp
    WHERE dldp.DocumentLineDetailId IN 
    (
		SELECT dld.DocumentLineDetailId
	    FROM DocumentLineDetail dld WITH (NOLOCK)
	    WHERE dld.DocumentLineId IN 
	    (
		    SELECT dl.DocumentLineId
			FROM DocumentLine dl WITH (NOLOCK)
			WHERE dl.DocumentId IN 
			(	
				SELECT d.DocumentId
				FROM Document d WITH (NOLOCK)
				WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
			)
		)
	)
END
GO 
