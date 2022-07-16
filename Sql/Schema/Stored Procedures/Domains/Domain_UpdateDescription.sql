IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_UpdateDescription')
	DROP PROCEDURE [dbo].[Domain_UpdateDescription]
GO
CREATE PROCEDURE [dbo].[Domain_UpdateDescription]
	@domainId int = 0,
	@description nvarchar(255)
AS
	UPDATE Domains SET [description] = @description
	WHERE domainId=@domainId