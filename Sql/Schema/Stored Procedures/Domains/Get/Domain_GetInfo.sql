IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_GetInfo')
	DROP PROCEDURE [dbo].[Domain_GetInfo]
GO
CREATE PROCEDURE [dbo].[Domain_GetInfo]
	@domain nvarchar(64)
AS
	SELECT d.*, (SELECT COUNT(*) FROM Articles a WHERE a.domainId = d.domainId) AS articles,
	CASE WHEN EXISTS(SELECT * FROM Whitelist_Domains WHERE domain=@domain) THEN 1 ELSE 0 END AS whitelisted,
	CASE WHEN EXISTS(SELECT * FROM Blacklist_Domains WHERE domain=d.domain) THEN 1 ELSE 0 END AS blacklisted
	FROM [Domains] d
	WHERE d.domain=@domain
