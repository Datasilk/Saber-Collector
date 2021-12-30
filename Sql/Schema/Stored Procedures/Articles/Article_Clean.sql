IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_Clean')
	DROP PROCEDURE [dbo].[Article_Clean]
GO
CREATE PROCEDURE [dbo].[Article_Clean]
	@articleId int = 0
AS
	EXEC ArticleSubjects_Remove @articleId=@articleId
	EXEC ArticleWords_Remove @articleId=@articleId
RETURN 0
