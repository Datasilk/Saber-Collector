IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Whitelist_Domain_Check')
	DROP PROCEDURE [dbo].[Whitelist_Domain_Check]
GO
CREATE PROCEDURE [dbo].[Whitelist_Domain_Check]
	@domain nvarchar(64)
AS
	SELECT COUNT(*) FROM Whitelist_Domains WHERE domain=@domain