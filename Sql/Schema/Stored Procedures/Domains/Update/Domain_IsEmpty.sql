IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_IsEmpty')
	DROP PROCEDURE [dbo].[Domain_IsEmpty]
GO
CREATE PROCEDURE [dbo].[Domain_IsEmpty]
	@domainId int,
	@empty bit = 0
AS
	UPDATE [Domains] SET [empty]=@empty, dateupdated = GETUTCDATE() WHERE domainId=@domainId
	
	