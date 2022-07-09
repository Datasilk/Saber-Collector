BEGIN TRY
	CREATE INDEX [IndexArticleTitles]
		ON [dbo].Articles (title ASC)
END TRY BEGIN CATCH END CATCH
BEGIN TRY
	CREATE INDEX [IndexArticleTitlesDesc]
		ON [dbo].Articles (title DESC)
END TRY BEGIN CATCH END CATCH