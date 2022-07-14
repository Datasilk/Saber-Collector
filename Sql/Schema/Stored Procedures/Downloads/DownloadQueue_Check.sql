IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DownloadQueue_Check')
	DROP PROCEDURE [dbo].[DownloadQueue_Check]
GO
CREATE PROCEDURE [dbo].[DownloadQueue_Check]
	@domaindelay int = 60, -- in seconds
	@domain nvarchar(64) = '',
	@feedId int = 0,
	@sort int = 0 -- 0 = newest, 1 = oldest
AS
	DECLARE @qid int, @domainId int

	BEGIN TRANSACTION

	SELECT TOP 1 @qid = q.qid, @domainId = q.domainId
	FROM DownloadQueue q WITH (TABLOCKX)
	JOIN Domains d ON d.domainId = q.domainId
	JOIN Whitelist_Domains w ON w.domain = d.domain -- must be a whitelisted domain
	WHERE q.status = 0
	AND (
		(@domain IS NOT NULL AND @domain <> '' AND d.domain = @domain)
		OR @domain IS NULL OR @domain = ''
	)
	AND d.lastchecked < DATEADD(SECOND, 0 - @domaindelay, GETUTCDATE())
	AND (
		(@feedId > 0 AND q.feedId = @feedId)
		OR @feedId <= 0
	)
	ORDER BY 
	CASE WHEN @sort = 0 THEN q.datecreated END DESC

	IF @qid > 0 BEGIN
		
		--WAITFOR DELAY '00:00:03' -- for debugging transactions

		UPDATE DownloadQueue SET status=1 WHERE qid=@qid
		UPDATE Domains SET lastchecked = GETUTCDATE()
		WHERE domainId = @domainId

		-- get next download in the queue
		SELECT q.*, d.domain 
		FROM DownloadQueue q 
		JOIN Domains d ON d.domainId = q.domainId
		WHERE qid=@qid

		-- get list of download rules for domain that queue item belongs to
		SELECT * FROM DownloadRules WHERE domainId = (SELECT domainId FROM DownloadQueue q WHERE qid=@qid)
	END

	COMMIT