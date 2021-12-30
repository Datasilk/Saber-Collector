IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_GetByUrl')
	DROP PROCEDURE [dbo].[Article_GetByUrl]
GO
CREATE PROCEDURE [dbo].[Article_GetByUrl]
	@url nvarchar(250)
AS
	SELECT * FROM Articles WHERE url=@url
RETURN 0
