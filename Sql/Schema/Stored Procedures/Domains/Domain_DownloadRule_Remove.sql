IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_DownloadRule_Remove')
	DROP PROCEDURE [dbo].[Domain_DownloadRule_Remove]
GO
CREATE PROCEDURE [dbo].[Domain_DownloadRule_Remove]
	@ruleId int
AS
	DELETE FROM DownloadRules WHERE ruleId=@ruleId