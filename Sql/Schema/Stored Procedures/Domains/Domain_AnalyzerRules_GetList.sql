IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_AnalyzerRules_GetList')
	DROP PROCEDURE [dbo].[Domain_AnalyzerRules_GetList]
GO
CREATE PROCEDURE [dbo].[Domain_AnalyzerRules_GetList]
	@domainId int
AS
	SELECT * FROM AnalyzerRules WHERE domainId=@domainId
