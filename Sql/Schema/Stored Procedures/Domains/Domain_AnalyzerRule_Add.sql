IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_AnalyzerRule_Add')
	DROP PROCEDURE [dbo].[Domain_AnalyzerRule_Add]
GO
CREATE PROCEDURE [dbo].[Domain_AnalyzerRule_Add]
	@domainId int,
	@selector varchar(64) = '',
	@rule bit = 0
AS
	DECLARE @id int = NEXT VALUE FOR SequenceAnalyzerRules
	INSERT INTO AnalyzerRules (ruleId, domainId, selector, [rule], datecreated)
	VALUES (@id, @domainId, @selector, @rule, GETUTCDATE())

	SELECT @id