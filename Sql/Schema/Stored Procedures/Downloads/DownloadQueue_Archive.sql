IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DownloadQueue_Archive')
	DROP PROCEDURE [dbo].[DownloadQueue_Archive]
GO
CREATE PROCEDURE [dbo].[DownloadQueue_Archive]
	@qid bigint = 0
AS
	UPDATE DownloadQueue SET status=2 WHERE qid=@qid
	