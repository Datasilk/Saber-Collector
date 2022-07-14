BEGIN TRY
    CREATE INDEX [IndexDownloadQueueDateCreatedDesc] ON [dbo].[DownloadQueue] ([datecreated] DESC)
END TRY BEGIN CATCH END CATCH

GO