IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feeds_Category_Add')
	DROP PROCEDURE [dbo].[Feeds_Category_Add]
GO
CREATE PROCEDURE [dbo].[Feeds_Category_Add]
	@title nvarchar(64)
AS

	DECLARE @id int = NEXT VALUE FOR SequenceFeedCategories
	INSERT INTO FeedCategories (categoryId, title) VALUES (@id, @title)
