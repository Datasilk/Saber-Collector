IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleSentences_Remove')
	DROP PROCEDURE [dbo].[ArticleSentences_Remove]
GO
CREATE PROCEDURE [dbo].[ArticleSentences_Remove]
	@articleId int = 0
AS
	DELETE FROM ArticleSentences WHERE articleId=@articleId
RETURN 0
