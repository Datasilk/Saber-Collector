BEGIN TRY
    CREATE TABLE [dbo].[DomainCollectionGroups]
    (
	    [colgroupId] INT NOT NULL PRIMARY KEY, 
        [name] NVARCHAR(32) NOT NULL
    )
END TRY BEGIN CATCH END CATCH
    GO

BEGIN TRY
    CREATE INDEX [IX_Domains_DomainCollectionGroupNames] ON [dbo].[DomainCollectionGroups] ([name] DESC)
END TRY BEGIN CATCH END CATCH
    GO