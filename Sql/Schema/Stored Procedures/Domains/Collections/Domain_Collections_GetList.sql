IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_Collections_GetList')
	DROP PROCEDURE [dbo].[Domain_Collections_GetList]
GO
CREATE PROCEDURE [dbo].[Domain_Collections_GetList]
AS
	SELECT c.* FROM DomainCollections c
	LEFT JOIN DomainCollectionGroups g ON g.colgroupId=c.colgroupId
	ORDER BY g.[name] ASC, c.[name] ASC

	SELECT * FROM DomainCollectionGroups