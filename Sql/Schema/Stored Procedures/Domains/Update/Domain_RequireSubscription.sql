IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_RequireSubscription')
	DROP PROCEDURE [dbo].[Domain_RequireSubscription]
GO
CREATE PROCEDURE [dbo].[Domain_RequireSubscription]
	@domainId int,
	@required bit = 0
AS
	UPDATE [Domains] SET paywall=@required, dateupdated = GETUTCDATE() WHERE domainId=@domainId
	
	