IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Download_Delete')
	DROP PROCEDURE [dbo].[Download_Delete]
GO
CREATE PROCEDURE [dbo].[Download_Delete]
	@qid int = 0
AS
	DECLARE @url nvarchar(250)
	SELECT @url = [url] FROM DownloadQueue WHERE qid=@qid
	
	--delete the article associated with download
	DELETE FROM Articles WHERE [url] = (SELECT [url] FROM DownloadQueue WHERE qid=@qid)

	DELETE FROM DownloadQueue WHERE qid=@qid
	DELETE FROM Downloads WHERE id=@qid
	