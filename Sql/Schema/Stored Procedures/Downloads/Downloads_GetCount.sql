IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Downloads_GetCount')
	DROP PROCEDURE [dbo].[Downloads_GetCount]
GO
CREATE PROCEDURE [dbo].[Downloads_GetCount]
	
AS
	SELECT COUNT(*) FROM DownloadQueue WHERE [status]=0
