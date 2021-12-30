IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feeds_Categories_GetList')
	DROP PROCEDURE [dbo].[Feeds_Categories_GetList]
GO
CREATE PROCEDURE [dbo].[Feeds_Categories_GetList]
AS
	SELECT * FROM FeedCategories ORDER BY title ASC
