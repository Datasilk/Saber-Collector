BEGIN TRY
	CREATE TABLE [dbo].[ArticleWords]
	(
		[articleId] INT NOT NULL,
		[wordId] INT NOT NULL, 
		[count] INT NULL
	)
END TRY BEGIN CATCH END CATCH