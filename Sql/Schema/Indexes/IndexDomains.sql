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
    CREATE INDEX [IndexDomainsLastChecked] ON [dbo].[Domains] ([lastchecked] DESC)
END TRY BEGIN CATCH END CATCH

GO

BEGIN TRY
    CREATE INDEX [IndexDomainsCreatedDesc] ON [dbo].[Domains] ([datecreated] DESC)
END TRY BEGIN CATCH END CATCH

GO

BEGIN TRY
    CREATE INDEX [IndexDomainArticles] ON [dbo].[Domains] ([articles] DESC)
END TRY BEGIN CATCH END CATCH

GO

BEGIN TRY
    CREATE INDEX [IX_Domains_Title] ON [dbo].[Domains] ([title])
END TRY BEGIN CATCH END CATCH

BEGIN TRY
    CREATE INDEX [IX_Domains_HasTitle] ON [dbo].[Domains] ([hastitle] DESC)
END TRY BEGIN CATCH END CATCH

BEGIN TRY
    CREATE INDEX [IX_Domains_Language] ON [dbo].[Domains] ([lang])
    INCLUDE ([domain], [paywall], [free])
END TRY BEGIN CATCH END CATCH