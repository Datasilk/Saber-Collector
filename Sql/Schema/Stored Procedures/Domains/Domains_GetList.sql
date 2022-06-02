IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domains_GetList')
	DROP PROCEDURE [dbo].[Domains_GetList]
GO
CREATE PROCEDURE [dbo].[Domains_GetList]
	@subjectIds nvarchar(MAX) = '',
	@search nvarchar(MAX) = '',
	@type int = 0, -- 0 = all, 1 = whitelisted, 2 = blacklisted, 3 = not-listed
	@sort int = 0, -- 0 = ASC, 1 = DESC, 2 = most articles, 3 = newest, 4 = oldest
	@start int = 1,
	@length int = 50
AS
	/* get subjects from array */
	SELECT * INTO #subjects FROM dbo.SplitArray(@subjectIds, ',')

	/* get domains that match a search term */
	SELECT * INTO #search FROM dbo.SplitArray(@search, ',')

	/* get list of domains that match filter */
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER(ORDER BY 
		CASE WHEN @sort = 0 OR @sort = 1 THEN d.hastitle END DESC, 
		CASE WHEN @sort = 0 THEN d.title END,
		CASE WHEN @sort = 1 THEN d.title END DESC,
		CASE WHEN @sort = 2 THEN a.articles END DESC,
		CASE WHEN @sort = 3 THEN d.datecreated END DESC,
		CASE WHEN @sort = 4 THEN d.datecreated END ASC
		) AS rownum, d.*, a.articles, (CASE WHEN wl.domain IS NOT NULL THEN 1 ELSE 0 END) AS whitelisted
		FROM [Domains] d
		LEFT JOIN Whitelist_Domains wl ON wl.domain = d.domain
		LEFT JOIN Blacklist_Domains bl ON bl.domain = d.domain
		CROSS APPLY (
			SELECT COUNT(*) AS articles FROM Articles a 
			WHERE a.domainId = d.domainId
		) AS a
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
		)
		AND (
			(@sort = 2 AND a.articles > 0)
			OR (@sort <> 2)
		)
	) AS tbl WHERE rownum >= @start AND rownum < @start + @length
