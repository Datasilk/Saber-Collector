IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_Exists')
	DROP PROCEDURE [dbo].[Article_Exists]
GO
CREATE PROCEDURE [dbo].[Article_Exists]
	@url nvarchar(250)
AS
	SELECT COUNT(*) FROM Articles WHERE url=@url
