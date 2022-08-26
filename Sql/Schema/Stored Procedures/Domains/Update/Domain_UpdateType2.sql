IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_UpdateType2')
	DROP PROCEDURE [dbo].[Domain_UpdateType2]
GO
CREATE PROCEDURE [dbo].[Domain_UpdateType2]
	@domainId int = 0,
	@type int = -1
AS
	UPDATE Domains SET [type2] = @type, dateupdated = GETUTCDATE()
	WHERE domainId=@domainId