IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Blacklist_Wildcards_GetList')
	DROP PROCEDURE [dbo].[Blacklist_Wildcards_GetList]
GO
CREATE PROCEDURE [dbo].[Blacklist_Wildcards_GetList]
AS
	SELECT domain FROM Blacklist_Wildcards ORDER BY domain ASC