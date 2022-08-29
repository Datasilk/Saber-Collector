IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_UpdateInfo')
	DROP PROCEDURE [dbo].[Domain_UpdateInfo]
GO
CREATE PROCEDURE [dbo].[Domain_UpdateInfo]
	@domainId int = 0,
	@title nvarchar(128),
	@description nvarchar(255),
	@lang char(2) = 'en'
AS
	UPDATE Domains SET [title]=@title, [description] = @description, lang=@lang, hastitle=1, dateupdated = GETUTCDATE()
	WHERE domainId=@domainId