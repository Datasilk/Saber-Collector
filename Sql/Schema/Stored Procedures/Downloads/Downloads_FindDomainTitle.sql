IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Downloads_FindDomainTitle')
	DROP PROCEDURE [dbo].[Downloads_FindDomainTitle]
GO
CREATE PROCEDURE [dbo].[Downloads_FindDomainTitle]
	@domainId int = 0
AS
	SET NOCOUNT ON;
	--get common word found in all article titles
	DECLARE @articles TABLE (
		title nvarchar(250)
	)
	DECLARE @words TABLE (
		word nvarchar(MAX)
	)

	INSERT INTO @articles 
	SELECT top 10 a.title FROM Articles a
	WHERE a.domainId = @domainId

	DECLARE @count int = 0
	SELECT @count = COUNT(*) FROM @articles
	DECLARE @exclude TABLE(value NVARCHAR(32))
	INSERT INTO @exclude ([value])
	VALUES ('and'), ('or'), ('&'), ('the')

	DECLARE @cursor CURSOR, @title nvarchar(250)
	SET @cursor = CURSOR FOR
	SELECT title FROM @articles
	OPEN @cursor
	DECLARE @i int = 0
	FETCH NEXT FROM @cursor INTO @title
	WHILE @@FETCH_STATUS = 0 BEGIN
		--get all words & phrases from the title
		INSERT INTO @words SELECT value FROM STRING_SPLIT(@title, ' ') WHERE LEN(value) > 2 AND [value] NOT IN (SELECT * FROM @exclude)
		INSERT INTO @words SELECT value FROM STRING_SPLIT(@title, '-') WHERE LEN(value) > 2 AND [value] NOT IN (SELECT * FROM @exclude)
		INSERT INTO @words SELECT value FROM STRING_SPLIT(@title, '|') WHERE LEN(value) > 2 AND [value] NOT IN (SELECT * FROM @exclude)
		INSERT INTO @words SELECT value FROM STRING_SPLIT(@title, ':') WHERE LEN(value) > 2 AND [value] NOT IN (SELECT * FROM @exclude)
		INSERT INTO @words SELECT value FROM STRING_SPLIT(@title, ';') WHERE LEN(value) > 2 AND [value] NOT IN (SELECT * FROM @exclude)
		INSERT INTO @words SELECT value FROM STRING_SPLIT(@title, '/') WHERE LEN(value) > 2 AND [value] NOT IN (SELECT * FROM @exclude)
		FETCH NEXT FROM @cursor INTO @title
	END
	CLOSE @cursor
	DEALLOCATE @cursor
	SELECT @count = COUNT(*) FROM @words

	--get count of all duplicate words & phrases
	DECLARE @domainTitle nvarchar(128)
	SELECT TOP 1 @domainTitle = TRIM(word)
	FROM (
		SELECT word, COUNT(word) AS total, LEN(word) AS [length]
		FROM @words
		GROUP BY word
		HAVING COUNT(word) > 1
	) AS tbl
	ORDER BY total DESC, [length] DESC

	UPDATE DownloadDomains SET title=@domainTitle WHERE domainId=@domainId
