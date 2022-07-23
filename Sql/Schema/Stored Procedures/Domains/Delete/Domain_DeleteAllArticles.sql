IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_DeleteAllArticles')
	DROP PROCEDURE [dbo].[Domain_DeleteAllArticles]
GO
CREATE PROCEDURE [dbo].[Domain_DeleteAllArticles]
	@domainId int
AS
	SELECT articleId INTO #articles FROM Articles WHERE domainId=@domainId
	DELETE FROM ArticleSentences WHERE articleId IN (SELECT * FROM #articles)
	DELETE FROM ArticleWords WHERE articleId IN (SELECT * FROM #articles)
	DELETE FROM ArticleSubjects WHERE articleId IN (SELECT * FROM #articles)
	/* DELETE FROM ArticleStatistics articleId IN (SELECT * FROM #articles) */
	DELETE FROM Articles WHERE articleId IN (SELECT * FROM #articles)