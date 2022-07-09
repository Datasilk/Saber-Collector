BEGIN TRY
	CREATE INDEX [IndexArticleScore]
		ON [dbo].Articles (score ASC)
END TRY BEGIN CATCH END CATCH

BEGIN TRY
	CREATE INDEX [IndexArticleScoreDesc]
		ON [dbo].Articles (score DESC)
END TRY BEGIN CATCH END CATCH