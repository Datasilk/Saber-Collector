IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Whitelist_Domains_GetList')
	DROP PROCEDURE [dbo].[Whitelist_Domains_GetList]
GO
CREATE PROCEDURE [dbo].[Whitelist_Domains_GetList]
AS
	SELECT domain FROM Whitelist_Domains ORDER BY domain ASC