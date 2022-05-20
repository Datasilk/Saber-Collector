BEGIN TRY
    CREATE TABLE [dbo].[DownloadDomains]
    (
	    [domainId] INT NOT NULL PRIMARY KEY, 
        [domain] NVARCHAR(64) NOT NULL,
        [title] NVARCHAR(128) NOT NULL DEFAULT '',
        [lastchecked] DATETIME2 NOT NULL DEFAULT GETUTCDATE() -- last scraped a URL from the domain
    )
END TRY BEGIN CATCH END CATCH
    GO

BEGIN TRY
    CREATE INDEX [IX_DownloadDomains_Domain] ON [dbo].[DownloadDomains] ([domain])
END TRY BEGIN CATCH END CATCH