IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DownloadQueue_Check')
	DROP PROCEDURE [dbo].[DownloadQueue_Check]
GO
CREATE PROCEDURE [dbo].[DownloadQueue_Check]
	@domaindelay int = 5 -- in minutes
AS
	DECLARE @qid int, @domainId int
	SELECT TOP 1 @qid = q.qid, @domainId = q.domainId
	FROM DownloadQueue q
	JOIN DownloadDomains d ON d.domainId = q.domainId
	JOIN Whitelist_Domains w ON w.domain = d.domain -- must be a whitelisted domain
	WHERE q.status = 0
	AND d.lastchecked < DATEADD(MINUTE, 0 - @domaindelay, GETUTCDATE())

	IF @qid > 0 BEGIN
		UPDATE DownloadQueue SET status=1 WHERE qid=@qid
		UPDATE DownloadDomains SET lastchecked = GETUTCDATE()
		WHERE domainId = @domainId

		SELECT q.*, d.domain 
		FROM DownloadQueue q 
		JOIN DownloadDomains d ON d.domainId = q.domainId
		WHERE qid=@qid
	END
	