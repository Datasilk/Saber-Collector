IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_FindDescription')
	DROP PROCEDURE [dbo].[Domain_FindDescription]
GO
CREATE PROCEDURE [dbo].[Domain_FindDescription]
	@domainId int = 0
AS
	DECLARE @description nvarchar(255)
	SELECT TOP 1 @description = summary
	FROM Articles 
	WHERE domainId=@domainId
	AND summary != ''
	ORDER BY LEN(url) ASC
	
	UPDATE Domains SET [description] = @description
	WHERE domainId=@domainId
	
	SELECT @description