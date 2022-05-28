IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_UpdateCache')
	DROP PROCEDURE [dbo].[Article_UpdateCache]
GO
CREATE PROCEDURE [dbo].[Article_UpdateCache]
	@articleId int = 0,
	@cached bit = 1
AS
	UPDATE Articles SET cached=@cached WHERE articleId=@articleId
