IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DownloadQueue_Add')
	DROP PROCEDURE [dbo].[DownloadQueue_Add]
GO
CREATE PROCEDURE [dbo].[DownloadQueue_Add]
	@url nvarchar(MAX) = '',
	@domain nvarchar(64) = '',
	@parentId int,
	@feedId int = 0
AS
DECLARE @domainId INT, @qid BIGINT, @count INT = 0, @title nvarchar(128)
IF EXISTS(SELECT * FROM Domains WHERE domain=@domain) BEGIN
	--get domain ID
	SELECT @domainId = domainId, @title = title FROM Domains WHERE domain=@domain
	IF @title = '' BEGIN
		IF (SELECT COUNT(*) FROM Articles WHERE domainId=@domainId) >= 10 BEGIN
			--get common word found in all article titles
			EXEC Domain_FindTitle @domainId=@domainId
		END
	END
END ELSE BEGIN
	--create domain ID
	EXEC Domain_Add @domain=@domain, @parentId=@parentId
	SELECT @domainId = domainId, @title = title FROM Domains WHERE domain=@domain
END

	IF NOT EXISTS(SELECT * FROM DownloadQueue WHERE url=@url) 
	AND NOT EXISTS(SELECT * FROM Downloads WHERE url=@url) BEGIN
		SET @qid = NEXT VALUE FOR SequenceDownloadQueue
		INSERT INTO DownloadQueue (qid, [url], [path], feedId, domainId, [status], datecreated) 
		VALUES (@qid, @url, dbo.GetPathFromUrl(@url, @domain), @feedId, @domainId, 0, GETUTCDATE())
	END ELSE BEGIN
		SELECT @qid = qid FROM DownloadQueue WHERE url=@url
		IF @qid IS NULL BEGIN
			SELECT @qid = id FROM Downloads WHERE url=@url
		END
	END
	
	SELECT @qid AS qid


	