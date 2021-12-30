BEGIN TRY
	CREATE INDEX [IndexArticleSubjects]
		ON [dbo].ArticleSubjects (subjectId ASC, datepublished DESC, datecreated DESC)
END TRY BEGIN CATCH END CATCH