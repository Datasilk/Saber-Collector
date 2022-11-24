IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DomainTypeMatches_Remove')
	DROP PROCEDURE [dbo].[DomainTypeMatches_Remove]
GO
CREATE PROCEDURE [dbo].[DomainTypeMatches_Remove]
	@matchId int
AS
	DELETE FROM DomainTypeMatches WHERE matchId=@matchId