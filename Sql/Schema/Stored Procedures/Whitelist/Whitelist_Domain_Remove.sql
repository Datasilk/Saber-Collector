IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Whitelist_Domain_Remove')
	DROP PROCEDURE [dbo].[Whitelist_Domain_Remove]
GO
CREATE PROCEDURE [dbo].[Whitelist_Domain_Remove]
	@domain nvarchar(64)
AS
	DELETE FROM Whitelist_Domains WHERE domain=@domain