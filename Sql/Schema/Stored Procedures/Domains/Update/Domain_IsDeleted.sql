IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_IsDeleted')
	DROP PROCEDURE [dbo].[Domain_IsDeleted]
GO
CREATE PROCEDURE [dbo].[Domain_IsDeleted]
	@domainId int,
	@delete bit = 1
AS
	UPDATE [Domains] SET [deleted]=@delete, dateupdated = GETUTCDATE() WHERE domainId=@domainId
	
	