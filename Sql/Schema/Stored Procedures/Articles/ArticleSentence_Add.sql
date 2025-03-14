﻿IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleSentence_Add')
	DROP PROCEDURE [dbo].[ArticleSentence_Add]
GO
CREATE PROCEDURE [dbo].[ArticleSentence_Add]
	@articleId int = 0,
	@index int = 0,
	@sentence nvarchar(MAX) = ''
AS
	INSERT INTO ArticleSentences (articleId, [index], sentence)
	VALUES (@articleId, @index, @sentence)
RETURN 0
