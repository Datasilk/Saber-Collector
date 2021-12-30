IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Words_GetList')
	DROP PROCEDURE [dbo].[Words_GetList]
GO
CREATE PROCEDURE [dbo].[Words_GetList]
	@words nvarchar(MAX)
AS
SELECT * INTO #words FROM dbo.SplitArray(@words, ',')
SELECT w.*, sw.subjectId
FROM Words w
JOIN SubjectWords sw ON sw.wordId=w.wordId
WHERE word IN (SELECT value FROM #words)
