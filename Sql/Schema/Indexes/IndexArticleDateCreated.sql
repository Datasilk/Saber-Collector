BEGIN TRY
	CREATE INDEX [IndexArticleDateCreated]
		ON [dbo].Articles (datecreated ASC)
END TRY BEGIN CATCH END CATCH
BEGIN TRY
	CREATE INDEX [IndexArticleDateCreatedDesc]
		ON [dbo].Articles (datecreated DESC)
END TRY BEGIN CATCH END CATCH