BEGIN TRY
	CREATE INDEX [IndexArticleVisited]
		ON [dbo].Articles (visited DESC)
END TRY BEGIN CATCH END CATCH