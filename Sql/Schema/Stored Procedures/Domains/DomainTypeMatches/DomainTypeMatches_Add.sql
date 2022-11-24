IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DomainTypeMatches_Add')
	DROP PROCEDURE [dbo].[DomainTypeMatches_Add]
GO
CREATE PROCEDURE [dbo].[DomainTypeMatches_Add]
	@type int,
	@type2 int = -1,
	@words nvarchar(MAX),
	@threshold int,
	@rank int
AS
	DECLARE @id int
	SET @id = NEXT VALUE FOR SequenceDomainTypeMatches
	INSERT INTO DomainTypeMatches (matchId, [type], [type2], words, threshold, [rank])
	VALUES (@id, @type, @type2, @words, @threshold, @rank)