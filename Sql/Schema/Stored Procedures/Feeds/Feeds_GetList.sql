IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feeds_GetList')
	DROP PROCEDURE [dbo].[Feeds_GetList]
GO
CREATE PROCEDURE [dbo].[Feeds_GetList]
AS
SELECT f.*, fc.title AS category
FROM Feeds f
JOIN FeedCategories fc ON fc.categoryId = f.categoryId
WHERE feedId > 0 ORDER BY fc.title ASC, f.title ASC
