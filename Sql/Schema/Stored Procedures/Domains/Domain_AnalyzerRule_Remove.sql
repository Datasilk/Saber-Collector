IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_AnalyzerRule_Remove')
	DROP PROCEDURE [dbo].[Domain_AnalyzerRule_Remove]
GO
CREATE PROCEDURE [dbo].[Domain_AnalyzerRule_Remove]
	@ruleId int
AS
	DELETE FROM AnalyzerRules WHERE ruleId=@ruleId