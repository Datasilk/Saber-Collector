IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_CollectionGroups_GetList')
	DROP PROCEDURE [dbo].[Domain_CollectionGroups_GetList]
GO
CREATE PROCEDURE [dbo].[Domain_CollectionGroups_GetList]
AS
	SELECT * FROM DomainCollectionGroups ORDER BY [name] ASC