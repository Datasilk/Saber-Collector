BEGIN TRY
    CREATE INDEX [IndexDownloadUrl] ON [dbo].[Downloads] ([url])
END TRY BEGIN CATCH END CATCH

GO