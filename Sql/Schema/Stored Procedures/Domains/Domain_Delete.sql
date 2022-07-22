IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_Delete')
	DROP PROCEDURE [dbo].[Domain_Delete]
GO
CREATE PROCEDURE [dbo].[Domain_Delete]
	@domainId int
AS
	DECLARE @domain nvarchar(128)
	SELECT @domain = domain FROM Domains WHERE domainId=@domainId
	EXEC Domain_DeleteAllArticles @domainId=@domainId
	DELETE FROM Domains WHERE domainId=@domainId
	DELETE FROM DownloadQueue WHERE domainId=@domainId
	DELETE FROM Downloads WHERE domainId=@domainId
	DELETE FROM Whitelist_Domains WHERE domain=@domain
	DELETE FROM Blacklist_Domains WHERE domain=@domain