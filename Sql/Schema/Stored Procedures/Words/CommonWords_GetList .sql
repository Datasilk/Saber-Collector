IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'CommonWords_GetList')
	DROP PROCEDURE [dbo].[CommonWords_GetList]
GO
CREATE PROCEDURE [dbo].[CommonWords_GetList]
AS
SELECT * FROM CommonWords
