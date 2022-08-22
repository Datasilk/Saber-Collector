IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_UpdateHttpsWww')
	DROP PROCEDURE [dbo].[Domain_UpdateHttpsWww]
GO
CREATE PROCEDURE [dbo].[Domain_UpdateHttpsWww]
	@domainId int = 0,
	@https bit = 0,
	@www bit = 0
AS
	UPDATE Domains SET [https] = @https, [www] = @www, dateupdated = GETUTCDATE()
	WHERE domainId=@domainId