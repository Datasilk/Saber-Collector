IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FeedCheckedLog_Add')
	DROP PROCEDURE [dbo].[FeedCheckedLog_Add]
GO
CREATE PROCEDURE [dbo].[FeedCheckedLog_Add]
	@feedId int = 0,
	@links int = 0
AS
	INSERT INTO FeedsCheckedLog (feedId, links, datechecked)
	VALUES (@feedId, @links, GETUTCDATE())
	UPDATE Feeds SET lastChecked = GETUTCDATE()