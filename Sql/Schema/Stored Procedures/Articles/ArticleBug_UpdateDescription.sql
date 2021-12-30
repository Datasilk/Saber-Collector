IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleBug_UpdateDescription')
	DROP PROCEDURE [dbo].[ArticleBug_UpdateDescription]
GO
CREATE PROCEDURE [dbo].[ArticleBug_UpdateDescription]
	@bugId int = 0,
	@description nvarchar(MAX) = ''
AS
	UPDATE ArticleBugs SET description=@description WHERE bugId=@bugId
