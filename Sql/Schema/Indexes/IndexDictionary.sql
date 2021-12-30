BEGIN TRY
	CREATE INDEX [IndexDictionary]
		ON [dbo].Dictionary (word ASC)
END TRY BEGIN CATCH END CATCH