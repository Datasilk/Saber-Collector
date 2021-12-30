IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feeds_Check')
	DROP PROCEDURE [dbo].[Feeds_Check]
GO
CREATE PROCEDURE [dbo].[Feeds_Check]
	
AS
	SELECT f.*, c.title AS category
	FROM Feeds f 
	JOIN FeedCategories c ON c.categoryId = f.categoryId
	WHERE f.lastChecked < DATEADD(HOUR, -24, GETUTCDATE())
