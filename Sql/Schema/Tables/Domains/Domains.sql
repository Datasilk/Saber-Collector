BEGIN TRY
    CREATE TABLE [dbo].[Domains]
    (
	    [domainId] INT NOT NULL PRIMARY KEY, 
        [domain] NVARCHAR(64) NOT NULL,
        [hastitle] BIT NOT NULL DEFAULT 0,
        [paywall] BIT NOT NULL DEFAULT 0,
        [free] BIT NOT NULL DEFAULT 0,
        [type] INT NOT NULL DEFAULT 0, -- 0 = unused, 1 = website, 2 = ecommerce, 3 = wiki, 4 = blog, 5 = science journal, 6 = SASS, 7 = social network, 8 = advertiser, 9 = search engine, 10 = portfolio
        [title] NVARCHAR(128) NOT NULL DEFAULT '',
        [description] NVARCHAR(255) NOT NULL DEFAULT '',
        [datecreated] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [dateupdated] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
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