IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domains_GetList')
	DROP PROCEDURE [dbo].[Domains_GetList]
GO
CREATE PROCEDURE [dbo].[Domains_GetList]
	@subjectIds nvarchar(MAX) = '',
	@search nvarchar(MAX) = '',
	@start int = 1,
	@length int = 50
AS
	/* get subjects from array */
	SELECT * INTO #subjects FROM dbo.SplitArray(@subjectIds, ',')

	/* get domains that match a search term */
	SELECT * INTO #search FROM dbo.SplitArray(@search, ',')

	/* get list of domains that match filter */
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER(ORDER BY d.hastitle DESC, d.title ASC) AS rownum, d.*
		FROM [Domains] d
		WHERE
		(
			(@search IS NOT NULL AND @search  <> '' AND (
				d.title LIKE '%' + @search + '%'
				OR d.domain LIKE '%' + @search + '%'
			))
			OR (@search IS NULL OR @search = '')
		) 
	) AS tbl WHERE rownum >= @start AND rownum < @start + @length
