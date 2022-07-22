BEGIN TRY
    CREATE INDEX [IndexDomainNames] ON [dbo].[Domains] ([domain])
END TRY BEGIN CATCH END CATCH

GO

BEGIN TRY
    CREATE INDEX [IndexDomainNamesDesc] ON [dbo].[Domains] ([domain] DESC)
END TRY BEGIN CATCH END CATCH

GO

BEGIN TRY
    CREATE INDEX [IndexDomainsCreated] ON [dbo].[Domains] ([datecreated])
END TRY BEGIN CATCH END CATCH

GO

BEGIN TRY
    CREATE INDEX [IndexDomainsCreatedDesc] ON [dbo].[Domains] ([datecreated] DESC)
END TRY BEGIN CATCH END CATCH

GO

BEGIN TRY
    CREATE INDEX [IndexDomainArtciles] ON [dbo].[Domains] ([articles] DESC)
END TRY BEGIN CATCH END CATCH

GO