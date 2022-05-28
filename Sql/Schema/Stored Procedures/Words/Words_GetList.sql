IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Words_GetList')
	DROP PROCEDURE [dbo].[Words_GetList]
GO
CREATE PROCEDURE [dbo].[Words_GetList]
	@words nvarchar(MAX)
AS
SELECT * INTO #words FROM dbo.SplitArray(@words, ',')
SELECT w.*, SUBSTRING(
	( -- combine subjectIds into comma-delimited string
		SELECT CAST(sw.subjectId AS nvarchar(32)) + ',' AS [text] FROM SubjectWords sw
		WHERE sw.wordId=w.wordId
		FOR XML Path(''), TYPE
	).value('text()[1]', 'nvarchar(max)'
), 2, 9999) AS subjectIds
FROM Words w
WHERE word IN (SELECT value FROM #words)
