IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleWord_Add')
	DROP PROCEDURE [dbo].[ArticleWord_Add]
GO
CREATE PROCEDURE [dbo].[ArticleWord_Add]
	@articleId int = 0,
	@wordId int = 0,
	@count int = 0
AS
	IF (SELECT COUNT(*) FROM ArticleWords WHERE articleId=@articleId AND wordId=@wordId) = 0 BEGIN
		INSERT INTO ArticleWords (articleId, wordId, [count]) 
		VALUES (@articleId, @wordId, @count)
	END
