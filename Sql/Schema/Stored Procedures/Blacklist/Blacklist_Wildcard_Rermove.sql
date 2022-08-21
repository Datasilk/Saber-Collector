IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Blacklist_Wildcard_Remove')
	DROP PROCEDURE [dbo].[Blacklist_Wildcard_Remove]
GO
CREATE PROCEDURE [dbo].[Blacklist_Wildcard_Remove]
	@domain nvarchar(64)
AS
	DELETE FROM Blacklist_Wildcards WHERE domain=@domain