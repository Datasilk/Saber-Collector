IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_DownloadRules_GetList')
	DROP PROCEDURE [dbo].[Domain_DownloadRules_GetList]
GO
CREATE PROCEDURE [dbo].[Domain_DownloadRules_GetList]
	@domainId int
AS
	SELECT * FROM DownloadRules WHERE domainId=@domainId ORDER BY datecreated ASC
