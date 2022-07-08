IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DownloadQueue_Check')
	DROP PROCEDURE [dbo].[DownloadQueue_Check]
GO
CREATE PROCEDURE [dbo].[DownloadQueue_Check]
	@domaindelay int = 60, -- in seconds
	@feedId int = 0
AS
	DECLARE @qid int, @domainId int
	SELECT TOP 1 @qid = q.qid, @domainId = q.domainId
	FROM DownloadQueue q
	JOIN Domains d ON d.domainId = q.domainId
	JOIN Whitelist_Domains w ON w.domain = d.domain -- must be a whitelisted domain
	WHERE q.status = 0
	AND d.lastchecked < DATEADD(SECOND, 0 - @domaindelay, GETUTCDATE())
	AND (
		(@feedId > 0 AND q.feedId = @feedId)
		OR @feedId <= 0
	)

	IF @qid > 0 BEGIN
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
	