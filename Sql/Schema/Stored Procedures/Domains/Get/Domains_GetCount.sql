IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domains_GetCount')
	DROP PROCEDURE [dbo].[Domains_GetCount]
GO
CREATE PROCEDURE [dbo].[Domains_GetCount]
	@subjectIds nvarchar(MAX) = '',
	@search nvarchar(MAX) = '',
	@type int = 0, -- 0 = all, 1 = whitelisted, 2 = blacklisted, 3 = not-listed, 4 = paywall, 5 = free, 6 = unprocessed
	@sort int = 0, -- 0 = ASC, 1 = DESC, 2 = most articles, 3 = newest, 4 = oldest
	@parentId int = -1
AS
	/* get subjects from array */
	SELECT * INTO #subjects FROM dbo.SplitArray(@subjectIds, ',')

	/* get domains that match a search term */
	SELECT * INTO #search FROM dbo.SplitArray(@search, ',')

	/* get list of domains that match filter */
	SELECT COUNT(*)
	FROM [Domains] d
	LEFT JOIN Whitelist_Domains wl ON wl.domain = d.domain
	LEFT JOIN Blacklist_Domains bl ON bl.domain = d.domain
	WHERE
	(
		(@search IS NOT NULL AND @search  <> '' AND (
			d.title LIKE '%' + @search + '%'
			OR d.domain LIKE '%' + @search + '%'
		))
		OR (@search IS NULL OR @search = '')
	) AND (
		(@type = 0)
		OR (@type = 1 AND wl.domain IS NOT NULL)
		OR (@type = 2 AND bl.domain IS NOT NULL)
		OR (@type = 3 AND wl.domain IS NULL AND bl.domain IS NULL)
		OR (@type = 4 AND d.paywall = 1)
		OR (@type = 5 AND d.free = 1)
		OR (@type = 6 AND d.free = 0 AND d.paywall = 0 AND d.type = -1 AND bl.domain IS NULL AND wl.domain IS NULL)
	)
	AND (
		(@sort = 2 AND d.articles > 0)
		OR (@sort <> 2)
	)
	AND (
		(@parentId >= 0 AND d.parentId = @parentId)
		OR (@parentId < 0)
	)
