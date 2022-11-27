BEGIN TRY
    CREATE INDEX [IndexDownloadQueueDateCreatedDesc] ON [dbo].[DownloadQueue] ([datecreated] DESC)
END TRY BEGIN CATCH END CATCH

GO

BEGIN TRY
    CREATE INDEX [IndexDownloadQueueFeedUrlDateCreatedDesc] ON [dbo].[DownloadQueue] ([feedId], [url], [datecreated] DESC)
END TRY BEGIN CATCH END CATCH

GO

BEGIN TRY
    CREATE INDEX [IndexDownloadQueueDomainStatus] ON [dbo].[DownloadQueue] ([domainId], [status])
    INCLUDE ([feedId], [url], [datecreated])
END TRY BEGIN CATCH END CATCH

GO