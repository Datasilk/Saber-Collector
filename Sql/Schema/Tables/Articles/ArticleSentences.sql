BEGIN TRY
    CREATE TABLE [dbo].[ArticleSentences]
    (
	    [articleId] INT NOT NULL, 
        [index] SMALLINT NULL, 
        [sentence] NVARCHAR(MAX) NULL
    )
END TRY BEGIN CATCH END CATCH