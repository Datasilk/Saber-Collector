IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_Visited')
	DROP PROCEDURE [dbo].[Article_Visited]
GO
CREATE PROCEDURE [dbo].[Article_Visited]
	@articleId int
AS
	UPDATE Articles SET visited += 1, cached = 1 WHERE articleId=@articleId
