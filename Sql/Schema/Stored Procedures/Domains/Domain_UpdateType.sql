IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_UpdateType')
	DROP PROCEDURE [dbo].[Domain_UpdateType]
GO
CREATE PROCEDURE [dbo].[Domain_UpdateType]
	@domainId int = 0,
	@type int = -1
AS
	UPDATE Domains SET [type] = @type
	WHERE domainId=@domainId