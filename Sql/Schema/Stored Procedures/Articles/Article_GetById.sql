IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_GetById')
	DROP PROCEDURE [dbo].[Article_GetById]
GO
CREATE PROCEDURE [dbo].[Article_GetById]
	@articleId int
AS
	SELECT a.*, d.domain FROM Articles a
	LEFT JOIN DownloadDomains d ON d.domainId = a.domainId
	WHERE a.articleId=@articleId
RETURN 0
