IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleSubjects_Remove')
	DROP PROCEDURE [dbo].[ArticleSubjects_Remove]
GO
CREATE PROCEDURE [dbo].[ArticleSubjects_Remove]
	@articleId int = 0,
	@subjectId int = 0
AS
	IF @subjectId = 0 BEGIN
		DELETE FROM ArticleSubjects WHERE articleId=@articleId
	END ELSE BEGIN
		DELETE FROM ArticleSubjects WHERE articleId=@articleId AND subjectId=@subjectId
	END
RETURN 0
