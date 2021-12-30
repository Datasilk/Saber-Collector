IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Subject_GetByTitle')
	DROP PROCEDURE [dbo].[Subject_GetByTitle]
GO
CREATE PROCEDURE [dbo].[Subject_GetByTitle]
	@title nvarchar(50),
	@breadcrumb nvarchar(MAX)
AS
SELECT * FROM Subjects WHERE breadcrumb = @breadcrumb AND title=@title
