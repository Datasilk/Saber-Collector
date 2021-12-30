IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Subject_GetById')
	DROP PROCEDURE [dbo].[Subject_GetById]
GO
CREATE PROCEDURE [dbo].[Subject_GetById]
	@subjectId int
AS
SELECT * FROM Subjects WHERE subjectId=@subjectId
