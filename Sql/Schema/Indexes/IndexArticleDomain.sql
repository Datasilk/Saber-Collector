BEGIN TRY
	CREATE INDEX [IndexArticleDomain]
		ON [dbo].Articles (domainId)
END TRY BEGIN CATCH END CATCH