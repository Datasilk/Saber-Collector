IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_HasFreeContent')
	DROP PROCEDURE [dbo].[Domain_HasFreeContent]
GO
CREATE PROCEDURE [dbo].[Domain_HasFreeContent]
	@domainId int,
	@free bit = 0
AS
	UPDATE [Domains] SET free=@free, dateupdated = GETUTCDATE() WHERE domainId=@domainId
	
	