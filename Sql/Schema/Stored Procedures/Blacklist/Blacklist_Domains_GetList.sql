IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Blacklist_Domains_GetList')
	DROP PROCEDURE [dbo].[Blacklist_Domains_GetList]
GO
CREATE PROCEDURE [dbo].[Blacklist_Domains_GetList]
AS
	SELECT domain FROM Blacklist_Domains ORDER BY domain ASC