BEGIN TRY
	CREATE INDEX [IndexSubjectsBreadcrumb]
		ON [dbo].Subjects (breadcrumb)
END TRY BEGIN CATCH END CATCH