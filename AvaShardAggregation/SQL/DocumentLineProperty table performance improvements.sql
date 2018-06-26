DECLARE @LastCheckTime DateTime = '2018-06-25 19:00:22.950', @CurrentCheckTime DateTime = '2018-06-25 19:53:37.950'


SELECT d.DocumentId, d.ModifiedDate
	INTO #modifiedDocuments
	FROM Document d WITH (NOLOCK)
	WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
CREATE UNIQUE INDEX IX_ModifiedDocuments ON #modifiedDocuments (DocumentId)



SELECT dlp.*
    FROM DocumentLineProperty dlp WITH (NOLOCK, forceseek)
    WHERE dlp.DocumentLineId IN 
    (
	    SELECT dl.DocumentLineId
		FROM DocumentLine dl WITH (NOLOCK, forceseek)
		WHERE dl.DocumentId IN 
		(	
			SELECT md.DocumentId
			FROM #modifiedDocuments md WITH (NOLOCK)
		)
	)


SELECT dlp.*
	FROM #modifiedDocuments md WITH (NOLOCK)
	LEFT JOIN DocumentLine dl WITH (NOLOCK, forceseek)
	ON md.DocumentId = dl.DocumentId
    LEFT JOIN DocumentLineProperty dlp  WITH (NOLOCK, forceseek)
    ON dl.DocumentLineId = dlp.DocumentLineId
	WHERE dlp.DocumentLinePropertyId IS NOT NULL


SELECT dlp.*
	FROM #modifiedDocuments md WITH (NOLOCK)
	INNER JOIN DocumentLine dl WITH (NOLOCK, forceseek)
	ON md.DocumentId = dl.DocumentId
    INNER JOIN DocumentLineProperty dlp  WITH (NOLOCK, forceseek)
    ON dl.DocumentLineId = dlp.DocumentLineId
	WHERE md.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime


drop table #modifiedDocuments


SELECT dlp.*
    FROM DocumentLineProperty dlp WITH (NOLOCK, forceseek)
    WHERE dlp.DocumentLineId IN 
    (
	    SELECT dl.DocumentLineId
		FROM DocumentLine dl WITH (NOLOCK, forceseek)
		WHERE dl.DocumentId IN 
		(	
			SELECT d.DocumentId
			FROM Document d WITH (NOLOCK)
			WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
		)
	)


SELECT dlp.*
	FROM Document d WITH (NOLOCK)
	LEFT JOIN DocumentLine dl WITH (NOLOCK, forceseek)
	ON d.DocumentId = dl.DocumentId
    LEFT JOIN DocumentLineProperty dlp  WITH (NOLOCK, forceseek)
    ON dl.DocumentLineId = dlp.DocumentLineId
	WHERE dlp.DocumentLinePropertyId IS NOT NULL
	AND d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime


SELECT dlp.*
	FROM Document d WITH (NOLOCK)
	INNER JOIN DocumentLine dl WITH (NOLOCK, forceseek)
	ON d.DocumentId = dl.DocumentId
    INNER JOIN DocumentLineProperty dlp  WITH (NOLOCK, forceseek)
    ON dl.DocumentLineId = dlp.DocumentLineId
	WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime

