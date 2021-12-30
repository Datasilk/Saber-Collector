BEGIN TRY
	CREATE INDEX [IndexArticleScore]
		ON [dbo].Articles (score DESC)
END TRY BEGIN CATCH END CATCH