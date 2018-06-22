-- User Table Merge Error
-- System.Data.SqlClient.SqlException: 'Violation of UNIQUE KEY constraint 'IX_User'. Cannot insert duplicate key in object 'dbo.User'. The duplicate key value is (akAdmin).
-- The statement has been terminated.'

SELECT * FROM [User] 
WHERE UserName = 'akAdmin'

SELECT * FROM User_Temp_Shard_1
WHERE UserName = 'akAdmin'

SELECT * FROM [User] WHERE UserId = 997177

DELETE FROM [User]	
WHERE UserId IN 
(
	SELECT u.UserId FROM [User] u 
	INNER JOIN User_Temp_Shard_1 uts1 
	ON u.UserName = uts1.UserName COLLATE DATABASE_DEFAULT  
	WHERE u.UserId <> uts1.UserId
)

TRUNCATE TABLE User_Temp_Shard_1


-- Company Table Merge Issues
--Violation of UNIQUE KEY constraint 'IX_Company'. Cannot insert duplicate key in object 'dbo.Company'. The duplicate key value is (1100000006, S00001001).
--The statement has been terminated.
SELECT * FROM Company
WHERE AccountId = 1100000006
AND CompanyCode = 'S00001001'

SELECT * FROM Company_Temp_Shard_1
WHERE AccountId = 1100000006
AND CompanyCode = 'S00001001'

DELETE FROM Company
WHERE CompanyId IN
(
	SELECT c.CompanyId FROM Company c
	INNER JOIN Company_Temp_Shard_1 cts1
	ON c.AccountId = cts1.AccountId
	AND c.CompanyCode = cts1.CompanyCode COLLATE DATABASE_DEFAULT
	WHERE c.CompanyId <> cts1.CompanyId
)

SELECT * FROM Company
WHERE AccountId = 1987654354
AND EntityNo = 5

SELECT * FROM Company_Temp_Shard_1
WHERE AccountId = 1987654354
AND EntityNo = 5

DELETE FROM Company
WHERE CompanyId IN
(
	SELECT c.CompanyId FROM Company c
	INNER JOIN Company_Temp_Shard_1 cts1
	ON c.AccountId = cts1.AccountId
	AND c.EntityNo = cts1.EntityNo --COLLATE DATABASE_DEFAULT
	WHERE c.CompanyId <> cts1.CompanyId
)

TRUNCATE TABLE Company_Temp_Shard_1






-- Service Table Merge Issues
SELECT * FROM Service
WHERE AccountId = 1100000006
AND ServiceTypeId = 1
AND EffDate = '1-1-1900'

SELECT * FROM Service_Temp_Shard_1
WHERE AccountId = 1100000006
AND ServiceTypeId = 1
AND EffDate = '1-1-1900'


DELETE FROM Service
WHERE ServiceId IN
(
	SELECT s.ServiceId FROM Service s
	INNER JOIN Service_Temp_Shard_1 sts1
	ON s.AccountId = sts1.AccountId
	AND s.ServiceTypeId = sts1.ServiceTypeId 
	AND s.EffDate = sts1.EffDate --COLLATE DATABASE_DEFAULT
	WHERE s.ServiceId <> sts1.ServiceId
)

TRUNCATE TABLE Service_Temp_Shard_1





-- TaxRule BulkCopy Issue
--'The locale id '0' of the source column 'UnitOfBasisId' and the locale id '1033' of the destination column 'AttributeOptions' do not match.'




-- TaxRule Merge Issues

SELECT * FROM TaxRule
WHERE HashKey= 0x01DF27EA8C5F147D24831BB9F1E573A8

SELECT * FROM TaxRule_Temp_Shard_1
WHERE HashKey = 0x01DF27EA8C5F147D24831BB9F1E573A8


DELETE FROM TaxRule
WHERE TaxRuleId IN
(
	SELECT tr.TaxRuleId FROM TaxRule tr
	INNER JOIN TaxRule_Temp_Shard_1 trts1
	ON tr.HashKey = trts1.HashKey  --COLLATE DATABASE_DEFAULT
	WHERE tr.TaxRuleId <> trts1.TaxRuleId
)

TRUNCATE TABLE TaxRule_Temp_Shard_1


-- DocumentCodeChangeList Merge Errors
-- Violation of PRIMARY KEY constraint 'PK_DocumentCodeChangeList_Temp_Shard_1'. Cannot insert duplicate key in object 'dbo.DocumentCodeChangeList_Temp_Shard_1'. The duplicate key value is (1100033097, cart1536745661, 0).

SELECT * FROM DocumentCodeChangeList
WHERE AccountId=1100033097
AND DocumentCode = 'cart1536745661'
AND Committed = 0

SELECT * FROM DocumentCodeChangeList_Temp_Shard_1
WHERE AccountId=1100033097
AND DocumentCode = 'cart1536745661'
AND Committed = 0

TRUNCATE TABLE TaxRule_Temp_Shard_1


-- Document table Merge errors
-- 'Cannot insert duplicate key row in object 'dbo.Document' with unique index 'IX_Document_Unique'. The duplicate key value is (28073, 0588874IN, 1, 1).
-- The statement has been terminated.'
28073, 0588874IN, 1, 1
CompanyId,DocumentCode,DocumentTypeId,Version
SELECT * FROM Document
WHERE CompanyId = 28073
AND DocumentCode = '0588874IN'
AND DocumentTypeId = 1
AND Version = 1

SELECT * FROM Document_Temp_Shard_1
WHERE CompanyId = 28073
AND DocumentCode = '0588874IN'
AND DocumentTypeId = 1
AND Version = 1

DELETE FROM Document
WHERE DocumentId IN
(
	SELECT d.DocumentId FROM Document d
	INNER JOIN Document_Temp_Shard_1 dts1
	ON d.CompanyId = dts1.CompanyId
	AND d.DocumentCode = dts1.DocumentCode COLLATE DATABASE_DEFAULT
	AND d.DocumentTypeId = dts1.DocumentTypeId
	AND d.Version = 1  --COLLATE DATABASE_DEFAULT
	WHERE d.DocumentId <> dts1.DocumentId
)

TRUNCATE TABLE Document_Temp_Shard_1
