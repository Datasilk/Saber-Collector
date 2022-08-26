IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_UpdateLanguage')
	DROP PROCEDURE [dbo].[Domain_UpdateLanguage]
GO
CREATE PROCEDURE [dbo].[Domain_UpdateLanguage]
	@domainId int = 0,
	@lang varchar(6)
AS
	UPDATE Domains SET lang = @lang, dateupdated = GETUTCDATE()
	WHERE domainId=@domainId