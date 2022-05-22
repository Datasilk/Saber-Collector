IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'CommonWords_Add')
	DROP PROCEDURE [dbo].[CommonWords_Add]
GO
CREATE PROCEDURE [dbo].[CommonWords_Add]
	@words nvarchar(MAX)
AS
DELETE FROM CommonWords WHERE word IN (SELECT [value] FROM dbo.SplitArray(@words, ','))
INSERT INTO CommonWords SELECT [value] AS word FROM dbo.SplitArray(@words, ',')
	
