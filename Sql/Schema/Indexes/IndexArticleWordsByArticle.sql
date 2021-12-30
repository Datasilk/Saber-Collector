BEGIN TRY
	CREATE INDEX [IndexArticleWordsByArticle]
		ON [dbo].ArticleWords ([articleId])
END TRY BEGIN CATCH END CATCH