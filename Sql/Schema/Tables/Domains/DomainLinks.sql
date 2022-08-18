BEGIN TRY
    CREATE TABLE [dbo].[DomainLinks]
    (
	    [domainId] INT NOT NULL PRIMARY KEY, 
        [linkId] INT NOT NULL
    )
END TRY BEGIN CATCH END CATCH
    GO
