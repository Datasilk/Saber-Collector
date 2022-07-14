BEGIN TRY
    CREATE TABLE [dbo].[Domains]
    (
	    [domainId] INT NOT NULL PRIMARY KEY, 
        [domain] NVARCHAR(64) NOT NULL,
        [hastitle] BIT NOT NULL DEFAULT 0,
        [paywall] BIT NOT NULL DEFAULT 0,
        [free] BIT NOT NULL DEFAULT 0,
        [title] NVARCHAR(128) NOT NULL DEFAULT '',
        [description] NVARCHAR(255) NOT NULL DEFAULT '',
        [datecreated] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [lastchecked] DATETIME2 NOT NULL DEFAULT GETUTCDATE() -- last scraped a URL from the domain
    )
END TRY BEGIN CATCH END CATCH
    GO

BEGIN TRY
    CREATE INDEX [IX_Domains_Domain] ON [dbo].[Domains] ([domain])
END TRY BEGIN CATCH END CATCH

BEGIN TRY
    CREATE INDEX [IX_Domains_Title] ON [dbo].[Domains] ([title])
END TRY BEGIN CATCH END CATCH

BEGIN TRY
    CREATE INDEX [IX_Domains_HasTitle] ON [dbo].[Domains] ([hastitle] DESC)
END TRY BEGIN CATCH END CATCH