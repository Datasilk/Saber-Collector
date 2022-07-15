IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Blacklist_Domain_Remove')
	DROP PROCEDURE [dbo].[Blacklist_Domain_Remove]
GO
CREATE PROCEDURE [dbo].[Blacklist_Domain_Remove]
	@domain nvarchar(64)
AS
	DELETE FROM Blacklist_Domains WHERE domain=@domain