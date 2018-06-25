-- Document
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_Document_with_Temp_Shard_1
AS
BEGIN
	DELETE FROM Document
	WHERE DocumentId IN
	(
		SELECT d.DocumentId 
		FROM Document d
		INNER JOIN Document_Temp_Shard_1 dts1
		ON d.CompanyId = dts1.CompanyId
		AND d.DocumentCode = dts1.DocumentCode COLLATE DATABASE_DEFAULT
		AND d.DocumentTypeId = dts1.DocumentTypeId
		AND d.Version = dts1.Version  
		WHERE d.DocumentId <> dts1.DocumentId
	)
END
GO 

-- DocumentLine
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_DocumentLine_with_Temp_Shard_1
AS
BEGIN
	DELETE FROM DocumentLine
	WHERE DocumentLineId IN
	(
		SELECT dl.DocumentLineId 
		FROM DocumentLine dl
		INNER JOIN DocumentLine_Temp_Shard_1 dlts1
		ON dl.DocumentId = dlts1.DocumentId 
		AND dl.[LineNo] = dlts1.[LineNo] COLLATE DATABASE_DEFAULT
		WHERE dl.DocumentLineId <> dlts1.DocumentLineId
	)
END
GO 

-- BoundaryOverrideId
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_BoundaryOverride_with_Temp_Shard_1
AS
BEGIN
	DELETE FROM BoundaryOverride 
	WHERE BoundaryOverrideId IN
	(
		SELECT bo.BoundaryOverrideId 
		FROM BoundaryOverride bo
		INNER JOIN BoundaryOverride_Temp_Shard_1 bots1
		ON bo.AccountId = bots1.AccountId 
		AND bo.State = bots1.State COLLATE DATABASE_DEFAULT
		AND bo.City = bots1.City COLLATE DATABASE_DEFAULT
		AND bo.StreetName = bots1.StreetName COLLATE DATABASE_DEFAULT
		AND bo.StreetPre = bots1.StreetPre COLLATE DATABASE_DEFAULT
		AND bo.StreetPost = bots1.StreetPost COLLATE DATABASE_DEFAULT
		AND bo.AddrLo = bots1.AddrLo COLLATE DATABASE_DEFAULT
		AND bo.OddEven = bots1.OddEven COLLATE DATABASE_DEFAULT
		AND bo.ZIP5Lo = bots1.ZIP5Lo COLLATE DATABASE_DEFAULT
		AND bo.ZIP4Lo = bots1.ZIP4Lo COLLATE DATABASE_DEFAULT
		AND bo.EffDate = bots1.EffDate
		AND bo.StreetSuffix = bots1.StreetSuffix COLLATE DATABASE_DEFAULT
		AND bo.TaxRegionId = bots1.TaxRegionId
		WHERE bo.BoundaryOverrideId <> bots1.BoundaryOverrideId
	)
END
GO 

-- JurisdictionOverride
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_JurisdictionOverride_with_Temp_Shard_1
AS
BEGIN
	DELETE FROM JurisdictionOverride 
	WHERE JurisdictionOverrideId IN
	(
		SELECT jo.JurisdictionOverrideId 
		FROM JurisdictionOverride jo
		INNER JOIN JurisdictionOverride_Temp_Shard_1 jots1
		ON jo.AccountId = jots1.AccountId 
		AND jo.Region = jots1.Region COLLATE DATABASE_DEFAULT
		AND jo.City = jots1.City COLLATE DATABASE_DEFAULT
		AND jo.PostalCode = jots1.PostalCode COLLATE DATABASE_DEFAULT
		AND jo.Address = jots1.Address COLLATE DATABASE_DEFAULT
		AND jo.EffDate = jots1.EffDate 
		WHERE jo.JurisdictionOverrideId <> jots1.JurisdictionOverrideId
	)
END
GO 


-- Service
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_Service_with_Temp_Shard_1
AS 
BEGIN 

	DELETE FROM Service
	WHERE ServiceId IN
	(
		SELECT s.ServiceId 
		FROM Service s
		INNER JOIN Service_Temp_Shard_1 sts1
		ON s.AccountId = sts1.AccountId  
		AND s.ServiceTypeId = sts1.ServiceTypeId  
		AND s.EffDate = sts1.EffDate  
		WHERE s.ServiceId <> sts1.ServiceId
	)

END
GO


-- Subscription
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_Subscription_with_Temp_Shard_1
AS 
BEGIN 

	DELETE FROM Subscription
	WHERE SubscriptionId IN
	(
		SELECT s.SubscriptionId 
		FROM Subscription s
		INNER JOIN Subscription_Temp_Shard_1 sts1
		ON s.AccountId = sts1.AccountId  
		AND s.ReferenceCode = sts1.ReferenceCode COLLATE DATABASE_DEFAULT
		AND s.SubscriptionTypeId = sts1.SubscriptionTypeId  
		AND s.RegionCode = sts1.RegionCode COLLATE DATABASE_DEFAULT
		AND s.EffDate = sts1.EffDate  
		WHERE s.SubscriptionId <> sts1.SubscriptionId
	)

END
GO



-- User
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_User_with_Temp_Shard_1
AS 
BEGIN 
	DELETE FROM [User]	
	WHERE UserId IN 
	(
		SELECT u.UserId 
		FROM [User] u 
		INNER JOIN User_Temp_Shard_1 uts1 
		ON u.UserName = uts1.UserName COLLATE DATABASE_DEFAULT  
		WHERE u.UserId <> uts1.UserId
	)
END 
GO 


-- Company
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_Company_with_Temp_Shard_1
AS 
BEGIN 
	DELETE FROM Company
	WHERE CompanyId IN
	(
		SELECT c.CompanyId 
		FROM Company c
		INNER JOIN Company_Temp_Shard_1 cts1
		ON c.AccountId = cts1.AccountId
		AND c.EntityNo = cts1.EntityNo 
		WHERE c.CompanyId <> cts1.CompanyId
	)
	
	
	
	DELETE FROM Company
	WHERE CompanyId IN
	(
		SELECT c.CompanyId 
		FROM Company c
		INNER JOIN Company_Temp_Shard_1 cts1
		ON c.AccountId = cts1.AccountId
		AND c.CompanyCode = cts1.CompanyCode COLLATE DATABASE_DEFAULT
		WHERE c.CompanyId <> cts1.CompanyId
	)
END 
GO 


-- TaxRule
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_TaxRule_with_Temp_Shard_1
AS 
BEGIN 

	DELETE FROM TaxRule
	WHERE TaxRuleId IN
	(
		SELECT tr.TaxRuleId 
		FROM TaxRule tr
		INNER JOIN TaxRule_Temp_Shard_1 trts1
		ON tr.HashKey = trts1.HashKey  
		WHERE tr.TaxRuleId <> trts1.TaxRuleId
	)

END
GO




--CompanyLocation
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_CompanyLocation_with_Temp_Shard_1
AS 
BEGIN 

	DELETE FROM CompanyLocation
	WHERE CompanyLocationId IN
	(
		SELECT cl.CompanyLocationId 
		FROM CompanyLocation cl
		INNER JOIN CompanyLocation_Temp_Shard_1 clts1
		ON cl.CompanyId = clts1.CompanyId  
		AND cl.LocationCode = clts1.LocationCode COLLATE DATABASE_DEFAULT
		AND cl.AddressTypeId = clts1.AddressTypeId  
		AND cl.StartDate = clts1.StartDate  
		WHERE cl.CompanyLocationId <> clts1.CompanyLocationId
	)

END
GO


--Nexus
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_Nexus_with_Temp_Shard_1
AS 
BEGIN 

	DELETE FROM Nexus
	WHERE NexusId IN
	(
		SELECT n.NexusId 
		FROM Nexus n
		INNER JOIN Nexus_Temp_Shard_1 nts1
		ON n.CompanyId = nts1.CompanyId  
		AND n.State = nts1.State COLLATE DATABASE_DEFAULT
		AND n.JurisCode = nts1.JurisCode COLLATE DATABASE_DEFAULT
		AND n.JurisTypeId = nts1.JurisTypeId  
		AND n.JurisName = nts1.JurisName  COLLATE DATABASE_DEFAULT
		AND n.EffDate = nts1.EffDate  
		AND n.NexusTypeId = nts1.NexusTypeId  
		AND n.NexusTaxTypeGroupIdSK = nts1.NexusTaxTypeGroupIdSK  
		WHERE n.NexusId <> nts1.NexusId
	)

END
GO


-- TaxCode
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_TaxCode_with_Temp_Shard_1
AS 
BEGIN 

	DELETE FROM TaxCode
	WHERE TaxCodeId IN
	(
		SELECT tc.TaxCodeId 
		FROM TaxCode tc
		INNER JOIN TaxCode_Temp_Shard_1 tcts1
		ON tc.TaxCode = tcts1.TaxCode COLLATE DATABASE_DEFAULT
		AND tc.CompanyId = tcts1.CompanyId  
		WHERE tc.TaxCodeId <> tcts1.TaxCodeId
	)

END
GO



-- AvaCertServiceConfig
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_AvaCertServiceConfig_with_Temp_Shard_1
AS 
BEGIN 

	DELETE FROM AvaCertServiceConfig
	WHERE AvaCertServiceConfigId IN
	(
		SELECT acsc.AvaCertServiceConfigId 
		FROM AvaCertServiceConfig acsc
		INNER JOIN AvaCertServiceConfig_Temp_Shard_1 acscts1
		ON acsc.CompanyId = acscts1.CompanyId  
		WHERE acsc.AvaCertServiceConfigId <> acscts1.AvaCertServiceConfigId
	)

END
GO



--CompanyReturn
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_CompanyReturn_with_Temp_Shard_1
AS 
BEGIN 

	DELETE FROM CompanyReturn
	WHERE CompanyReturnId IN
	(
		SELECT cr.CompanyReturnId 
		FROM CompanyReturn cr
		INNER JOIN CompanyReturn_Temp_Shard_1 crts1
		ON cr.CompanyId = crts1.CompanyId  
		AND cr.ReturnName = crts1.ReturnName COLLATE DATABASE_DEFAULT
		AND cr.EffDate = crts1.EffDate  
		AND cr.RegistrationId = crts1.RegistrationId  COLLATE DATABASE_DEFAULT
		WHERE cr.CompanyReturnId <> crts1.CompanyReturnId
	)

END
GO


-- Item
CREATE OR ALTER PROCEDURE sp_Delete_Duplicate_Item_with_Temp_Shard_1
AS 
BEGIN 

	DELETE FROM Item
	WHERE ItemId IN
	(
		SELECT i.ItemId 
		FROM Item i
		INNER JOIN Item_Temp_Shard_1 its1
		ON i.CompanyId = its1.CompanyId  
		AND i.ItemCode = its1.ItemCode COLLATE DATABASE_DEFAULT
		WHERE i.ItemId <> its1.ItemId
	)

END
GO