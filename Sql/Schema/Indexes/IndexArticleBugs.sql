BEGIN TRY
CREATE INDEX [IndexArticleBugs]
	ON [dbo].ArticleBugs (articleId ASC)
END TRY BEGIN CATCH END CATCH