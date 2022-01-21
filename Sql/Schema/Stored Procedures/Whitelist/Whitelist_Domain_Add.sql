IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Whitelist_Domain_Add')
	DROP PROCEDURE [dbo].[Whitelist_Domain_Add]
GO
CREATE PROCEDURE [dbo].[Whitelist_Domain_Add]
	@domain nvarchar(64)
AS
	DECLARE @domainId int
	BEGIN TRY
	INSERT INTO Whitelist_Domains (domain) VALUES (@domain)
	END TRY
	BEGIN CATCH
	END CATCH