IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_GetById')
	DROP PROCEDURE [dbo].[Article_GetById]
GO
CREATE PROCEDURE [dbo].[Article_GetById]
	@articleId int
AS
	SELECT * FROM Articles WHERE articleId=@articleId
RETURN 0
