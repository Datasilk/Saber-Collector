IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_UpdateUrl')
	DROP PROCEDURE [dbo].[Article_UpdateUrl]
GO
CREATE PROCEDURE [dbo].[Article_UpdateUrl]
	@articleId int = 0,
	@url nvarchar(250),
	@domain nvarchar(250)
AS

DECLARE @oldurl nvarchar(250), @domainId int
SELECT @oldurl=[url] FROM Articles WHERE articleId=@articleId
SELECT @domainId=domainId FROM Domains WHERE domain=@domain

IF @domainId IS NULL BEGIN
	SET @domainId = NEXT VALUE FOR SequenceDomains
	INSERT INTO Domains (domainId, domain) VALUES (@domainId, @domain)
END

IF @oldurl != @url BEGIN
	DECLARE @newarticleId int
	SELECT TOP 1 @newarticleId = articleId FROM Articles WHERE [url]=@url ORDER BY datecreated ASC
	IF @newarticleId IS NOT NULL AND @newarticleId != @articleId BEGIN
		DELETE FROM Articles WHERE articleId=@articleId
	END
	UPDATE Articles SET [url]=@url, domainId=@domainId, domain=@domain WHERE articleId=@articleId

	--delete any downloads that already use the new URL
	DELETE FROM DownloadQueue WHERE [url]=@url
	DELETE FROM Downloads WHERE [url]=@url

	--update downloads that used the old URL
	UPDATE DownloadQueue SET [url]=@url, domainId=@domainId WHERE [url]=@oldurl
	UPDATE Downloads SET [url]=@url, domainId=@domainId WHERE [url]=@oldurl
END
