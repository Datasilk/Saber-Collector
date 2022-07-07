IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_DownloadRule_Add')
	DROP PROCEDURE [dbo].[Domain_DownloadRule_Add]
GO
CREATE PROCEDURE [dbo].[Domain_DownloadRule_Add]
	@domainId int,
	@url varchar(64) = '',
	@title varchar(64) = '',
	@summary varchar(64) = ''
AS
	DECLARE @id int = NEXT VALUE FOR SequenceDownloadRules
	INSERT INTO DownloadRules (ruleId, domainId, [url], title, summary, datecreated)
	VALUES (@id, @domainId, @url, @title, @summary, GETUTCDATE())

	SELECT @id