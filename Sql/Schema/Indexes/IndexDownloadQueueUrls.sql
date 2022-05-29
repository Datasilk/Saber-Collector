BEGIN TRY
    CREATE INDEX [IndexDownloadQueueUrl] ON [dbo].[DownloadQueue] ([url])
END TRY BEGIN CATCH END CATCH

GO