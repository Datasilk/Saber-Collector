BEGIN TRY
	CREATE INDEX [IndexArticleWords]
		ON [dbo].ArticleWords ([wordId])
END TRY BEGIN CATCH END CATCH