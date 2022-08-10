IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Download_Archive')
	DROP PROCEDURE [dbo].[Download_Archive]
GO
CREATE PROCEDURE [dbo].[Download_Archive]
	@qid bigint = 0
AS
	UPDATE DownloadQueue SET status=2 WHERE qid=@qid
	