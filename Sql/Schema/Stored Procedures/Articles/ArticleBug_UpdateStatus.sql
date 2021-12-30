IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleBug_UpdateStatus')
	DROP PROCEDURE [dbo].[ArticleBug_UpdateStatus]
GO
CREATE PROCEDURE [dbo].[ArticleBug_UpdateStatus]
	@bugId int = 0,
	@status int = 0
AS
	UPDATE ArticleBugs SET [status]=@status WHERE bugId=@bugId
