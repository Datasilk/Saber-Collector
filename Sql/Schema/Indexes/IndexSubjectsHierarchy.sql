BEGIN TRY
	CREATE INDEX [IndexSubjectsHierarchy]
		ON [dbo].Subjects (hierarchy)
END TRY BEGIN CATCH END CATCH