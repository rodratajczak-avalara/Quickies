DECLARE @LastCheckTime DateTime = '6/29/2018 8:42:01 AM', @CurrentCheckTime DateTime = '6/29/2018 9:42:01 AM'

SELECT count(d.DocumentId)
	FROM Document d WITH (NOLOCK)
	WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime

SELECT count(da.DocumentAddressId)
	FROM Document d WITH (NOLOCK)
	INNER JOIN DocumentAddress da WITH (NOLOCK, forceseek)
	ON d.DocumentId = da.DocumentId
	WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime

SELECT count(dl.DocumentLineId)
	FROM Document d WITH (NOLOCK)
	INNER JOIN DocumentLine dl WITH (NOLOCK, forceseek)
	ON d.DocumentId = dl.DocumentId
	WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime

SELECT count(dld.DocumentLineDetailId)
	FROM Document d WITH (NOLOCK)
	INNER JOIN DocumentLine dl WITH (NOLOCK, forceseek)
	ON d.DocumentId = dl.DocumentId
    INNER JOIN DocumentLineDetail dld WITH (NOLOCK, forceseek)
    ON dl.DocumentLineId = dld.DocumentLineId
	WHERE d.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime

SELECT count(ecd.ExemptCertDetailId)
    FROM ExemptCert ec WITH (NOLOCK)
    INNER JOIN ExemptCertDetail ecd WITH (NOLOCK, FORCESEEK)
    ON ec.ExemptCertId = ecd.ExemptCertId
    WHERE ec.ModifiedDate BETWEEN @LastCheckTime AND @CurrentCheckTime
