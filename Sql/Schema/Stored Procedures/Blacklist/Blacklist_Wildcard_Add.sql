IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Blacklist_Wildcard_Add')
	DROP PROCEDURE [dbo].[Blacklist_Wildcard_Add]
GO
CREATE PROCEDURE [dbo].[Blacklist_Wildcard_Add]
	@domain nvarchar(64)
AS
	INSERT INTO Blacklist_Wildcards (domain) VALUES (@domain)