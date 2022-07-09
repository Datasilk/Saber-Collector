IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleBug_Add')
	DROP PROCEDURE [dbo].[ArticleBug_Add]
GO
CREATE PROCEDURE [dbo].[ArticleBug_Add]
	@articleId int = 0,
	@title nvarchar(100) = '',
	@description nvarchar(MAX) = '',
	@status tinyint = 0
AS
	DECLARE @bugId int = NEXT VALUE FOR SequenceArticleBugs
	INSERT INTO ArticleBugs (bugId, articleId, title, [description], datecreated, [status])
	VALUES (@bugId, @articleId, @title, @description, GETUTCDATE(), @status)
