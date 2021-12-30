BEGIN TRY
    CREATE TABLE [dbo].[ArticleSubjects]
    (
	    [subjectId] INT NOT NULL, 
        [articleId] INT NULL, 
        [score] SMALLINT NULL, 
        [datecreated] DATETIME NULL, 
        [datepublished] DATETIME NULL
    )
END TRY BEGIN CATCH END CATCH