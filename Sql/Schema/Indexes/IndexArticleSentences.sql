BEGIN TRY
	CREATE INDEX [IndexArticleSentences]
		ON [dbo].ArticleSentences (articleId ASC)
END TRY BEGIN CATCH END CATCH