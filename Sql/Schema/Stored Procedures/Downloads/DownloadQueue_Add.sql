IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DownloadQueue_Add')
	DROP PROCEDURE [dbo].[DownloadQueue_Add]
GO
CREATE PROCEDURE [dbo].[DownloadQueue_Add]
	@urls nvarchar(MAX) = '', --comma delimited list
	@domain nvarchar(64) = '',
	@feedId int = 0
AS
SELECT * INTO #urls FROM dbo.SplitArray(@urls, ',')
DECLARE @cursor CURSOR, @url nvarchar(MAX), @domainId INT, @qid BIGINT, @count INT = 0, @title nvarchar(128)
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
	SET @domainId = NEXT VALUE FOR SequenceDomains
	INSERT INTO Domains (domainId, domain, lastchecked) VALUES (@domainId, @domain, DATEADD(HOUR, -1, GETUTCDATE()))
END
SET @cursor = CURSOR FOR
SELECT [value] FROM #urls
OPEN @cursor
FETCH NEXT FROM @cursor INTO @url
WHILE @@FETCH_STATUS = 0 BEGIN
	IF NOT EXISTS(SELECT * FROM DownloadQueue WHERE url=@url) BEGIN
		IF NOT EXISTS(SELECT * FROM Articles WHERE url=@url) BEGIN
			SET @qid = NEXT VALUE FOR SequenceDownloadQueue
			INSERT INTO DownloadQueue (qid, [url], [path], feedId, domainId, [status], datecreated) 
			VALUES (@qid, @url, dbo.GetPathFromUrl(@url, @domain), @feedId, @domainId, 0, GETUTCDATE())
			SET @count += 1
		END
	END
	FETCH NEXT FROM @cursor INTO @url
END
CLOSE @cursor
DEALLOCATE @cursor
SELECT @count AS [count]

	