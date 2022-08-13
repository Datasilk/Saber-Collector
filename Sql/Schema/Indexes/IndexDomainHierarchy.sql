BEGIN TRY
    CREATE INDEX [IX_DomainHierarchy_Domain] ON [dbo].[DomainHierarchy] ([domainId])
END TRY BEGIN CATCH END CATCH

BEGIN TRY
    CREATE INDEX [IX_DomainHierarchy_Parent] ON [dbo].[DomainHierarchy] ([parentId], [level])
END TRY BEGIN CATCH END CATCH