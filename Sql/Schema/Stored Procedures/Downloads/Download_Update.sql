IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Download_Update')
	DROP PROCEDURE [dbo].[Download_Update]
GO
CREATE PROCEDURE [dbo].[Download_Update]
	@qid int = 0,
	@status int = 0
AS
	UPDATE DownloadQueue SET status=@status WHERE qid=@qid