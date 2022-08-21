BEGIN TRY
    CREATE TABLE [dbo].[DomainLinks]
    (
	    [domainId] INT NOT NULL, 
        [linkId] INT NOT NULL,
        CONSTRAINT PK_DomainLinks PRIMARY KEY (domainId, linkId)
    )
END TRY BEGIN CATCH END CATCH
    GO
