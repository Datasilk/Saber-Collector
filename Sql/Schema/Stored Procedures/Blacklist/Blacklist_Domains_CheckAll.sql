IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Blacklist_Domains_CheckAll')
	DROP PROCEDURE [dbo].[Blacklist_Domains_CheckAll]
GO
CREATE PROCEDURE [dbo].[Blacklist_Domains_CheckAll]
	@domains nvarchar(MAX)
AS
	SELECT * INTO #domains FROM dbo.SplitArray(@domains, ',')
	SELECT domain FROM Blacklist_Domains WHERE domain IN (SELECT [value] FROM #domains)
	DROP TABLE #domains