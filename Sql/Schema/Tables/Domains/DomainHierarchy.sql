BEGIN TRY
    CREATE TABLE [dbo].[DomainHierarchy]
    (
	    [domainId] INT NOT NULL PRIMARY KEY, 
        [parentId] INT NOT NULL,
        [level] INT NOT NULL,
    )
END TRY BEGIN CATCH END CATCH
    GO
