IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_Collections_GetList')
	DROP PROCEDURE [dbo].[Domain_Collections_GetList]
GO
CREATE PROCEDURE [dbo].[Domain_Collections_GetList]
AS
	SELECT c.*, g.[name] AS groupName FROM DomainCollections c
	LEFT JOIN DomainCollectionGroups g ON g.colgroupId = c.colgroupId
	ORDER BY [name] ASC