IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Download_MoveArchived')
	DROP PROCEDURE [dbo].[Download_MoveArchived]
GO
CREATE PROCEDURE [dbo].[Download_MoveArchived]
AS
	INSERT INTO Downloads ([id], [feedId], [domainId], [status], [tries], [url], [path], [datecreated]) 
	SELECT * FROM DownloadQueue WHERE [status]=2
	DELETE FROM DownloadQueue WHERE [status]=2
	