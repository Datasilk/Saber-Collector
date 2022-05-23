BEGIN TRY
	CREATE TABLE [dbo].[Blacklist_Domains]
	(
		[domain] NVARCHAR(64) NOT NULL PRIMARY KEY
	)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[Dictionary]
    (
	    [word] NVARCHAR(25) NOT NULL PRIMARY KEY, 
        [vocabtype] TINYINT NULL,  
        [grammertype] TINYINT NULL,
        [socialtype] TINYINT NULL, 
        [objecttype] TINYINT NULL, 
        [score] TINYINT NULL
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[Subjects]
    (
	    [subjectId] INT NOT NULL PRIMARY KEY, 
        [parentId] INT NULL DEFAULT 0, 
        [grammartype] INT NULL DEFAULT 0, 
        [score] INT NULL DEFAULT 0, 
        [haswords] BIT NULL DEFAULT 0, 
        [title] NVARCHAR(50) NULL DEFAULT '', 
        [hierarchy] NVARCHAR(50) NULL DEFAULT '', 
        [breadcrumb] NVARCHAR(500) NULL DEFAULT ''
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceArticleBugs]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceArticles]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceDomains]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceDownloadQueue]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceFeedCategories]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceFeeds]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceStatisticsProjects]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceStatisticsResults]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceSubjects]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE SEQUENCE [dbo].[SequenceWords]
		AS BIGINT
		START WITH 1
		INCREMENT BY 1
		NO MAXVALUE
		NO CYCLE
		NO CACHE
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
CREATE INDEX [IndexArticleBugs]
	ON [dbo].ArticleBugs (articleId ASC)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexArticleScore]
		ON [dbo].Articles (score DESC)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexArticleSentences]
		ON [dbo].ArticleSentences (articleId ASC)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexArticleSubjects]
		ON [dbo].ArticleSubjects (subjectId ASC, datepublished DESC, datecreated DESC)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexArticleUrl]
		ON [dbo].Articles (url ASC)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexArticleWords]
		ON [dbo].ArticleWords ([wordId])
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexArticleWordsByArticle]
		ON [dbo].ArticleWords ([articleId])
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexDictionary]
		ON [dbo].Dictionary (word ASC)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexStatisticsResults]
		ON [dbo].StatisticsResults (projectId ASC, statId ASC)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexStatisticsResultsDate]
		ON [dbo].StatisticsResults ([year] ASC, [month] ASC, [day] ASC)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexStatisticsResultsLocation]
		ON [dbo].StatisticsResults (country ASC, [state] ASC, city ASC)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexStatisticsResultsProject]
		ON [dbo].StatisticsResults (projectId ASC)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexSubjectsBreadcrumb]
		ON [dbo].Subjects (breadcrumb)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexSubjectsHierarchy]
		ON [dbo].Subjects (hierarchy)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE INDEX [IndexWords]
		ON [dbo].Words (word)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DestroyCollection')
	DROP PROCEDURE [dbo].[DestroyCollection]
GO
CREATE PROCEDURE [dbo].[DestroyCollection]
	@articles BIT = 1,
	@subjects BIT = 1,
	@topics BIT = 1
AS
	IF @articles = 1 OR @subjects = 1 BEGIN
		DELETE FROM ArticleBugs
		DELETE FROM ArticleDates
		DELETE FROM Articles
		DELETE FROM ArticleSentences
		DELETE FROM ArticleSubjects
		DELETE FROM ArticleWords
		DELETE FROM DownloadQueue
		DELETE FROM FeedsCheckedLog
	END

	IF @subjects = 1 BEGIN
		DELETE FROM Subjects
		DELETE FROM Words
	END

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[ArticleBugs]
    (
	    [bugId] INT NOT NULL PRIMARY KEY, 
        [articleId] INT NULL, 
        [title] NVARCHAR(100) NULL, 
        [description] NVARCHAR(MAX) NULL, 
        [datecreated] DATETIME NULL, 
        [status] TINYINT NULL
    )
END TRY BEGIN CATCH END CATCH

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[ArticleDates]
    (
	    [articleId] INT NOT NULL PRIMARY KEY, 
        [date] DATE NULL, 
        [hasyear] BIT NULL, 
        [hasmonth] BIT NULL, 
        [hasday] BIT NULL
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[Articles]
    (
	    [articleId] INT NOT NULL PRIMARY KEY, 
        [feedId] INT NULL DEFAULT 0, 
	    [subjects] TINYINT NULL DEFAULT 0,
	    [subjectId] INT NULL DEFAULT 0,
	    [score] SMALLINT NULL DEFAULT 0,
        [images] TINYINT NULL DEFAULT 0, 
	    [filesize] FLOAT NULL DEFAULT 0,
        [wordcount] INT DEFAULT 0, 
        [sentencecount] SMALLINT DEFAULT 0, 
        [paragraphcount] SMALLINT DEFAULT 0,
        [importantcount] SMALLINT DEFAULT 0, 
	    [analyzecount] SMALLINT DEFAULT 0,
        [yearstart] SMALLINT NULL DEFAULT 0, 
        [yearend] SMALLINT NULL DEFAULT 0, 
	    [years] NVARCHAR(50) DEFAULT '',
        [datecreated] DATETIME NULL, 
        [datepublished] DATETIME NULL, 
        [relavance] SMALLINT NULL DEFAULT 0, 
        [importance] SMALLINT NULL DEFAULT 0, 
        [fiction] SMALLINT NULL DEFAULT 1, 
        [domain] NVARCHAR(50) NULL DEFAULT '', 
        [url] NVARCHAR(250) NULL DEFAULT '', 
        [title] NVARCHAR(250) NULL DEFAULT '', 
        [summary] NVARCHAR(250) NULL DEFAULT '',
	    [analyzed] FLOAT DEFAULT 0,
	    [cached] BIT NULL DEFAULT 0, 
        [active] BIT NULL DEFAULT 0, 
        [deleted] BIT NULL DEFAULT 0
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[ArticleSentences]
    (
	    [articleId] INT NOT NULL, 
        [index] SMALLINT NULL, 
        [sentence] NVARCHAR(MAX) NULL
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[ArticleSubjects]
    (
	    [subjectId] INT NOT NULL, 
        [articleId] INT NULL, 
        [score] SMALLINT NULL, 
        [datecreated] DATETIME NULL, 
        [datepublished] DATETIME NULL
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE TABLE [dbo].[ArticleWords]
	(
		[articleId] INT NOT NULL,
		[wordId] INT NOT NULL, 
		[count] INT NULL
	)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[Domains]
    (
	    [domainId] INT NOT NULL PRIMARY KEY, 
        [domain] NVARCHAR(64) NOT NULL,
        [lastchecked] DATETIME2 NOT NULL DEFAULT GETUTCDATE() -- last scraped a URL from the domain
    )
END TRY BEGIN CATCH END CATCH
    GO

BEGIN TRY
    CREATE INDEX [IX_Domains_Domain] ON [dbo].[Domains] ([domain])
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[DownloadQueue]
    (
	    [qid] INT NOT NULL,
        [feedId] INT NULL, 
        [domainId] INT NULL, 
        [status] INT NOT NULL DEFAULT 0, 
	    [url] NVARCHAR(255) NOT NULL, 
        [datecreated] DATETIME NULL, 
        CONSTRAINT [PK_DownloadQueue] PRIMARY KEY ([qid])
    )
END TRY BEGIN CATCH END CATCH

GO

BEGIN TRY
    CREATE INDEX [IX_DownloadQueue_Url] ON [dbo].[DownloadQueue] ([url])
END TRY BEGIN CATCH END CATCH

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE TABLE [dbo].[FeedCategories]
	(
		[categoryId] INT NOT NULL PRIMARY KEY, 
		[title] NVARCHAR(64) NOT NULL
	)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[Feeds]
    (
	    [feedId] INT NOT NULL PRIMARY KEY, 
        [categoryId] INT NULL, 
        [title] NVARCHAR(100) NULL, 
	    [url] NVARCHAR(100) NULL, 
        [checkIntervals] INT NULL, 
        [lastChecked] DATETIME NULL, 
        [filter] NVARCHAR(MAX) NULL
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[FeedsCheckedLog]
    (
	    [feedId] INT NOT NULL, 
        [links] SMALLINT NULL, 
        [datechecked] DATETIME NULL 
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE TABLE [dbo].[StatisticsProjectResearchers]
	(
		[projectId] INT NOT NULL PRIMARY KEY, 
		[researcherId] INT NULL
	)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[StatisticsProjects]
    (
	    [projectId] INT NOT NULL PRIMARY KEY, 
        [title] NVARCHAR(100) NULL, 
        [url] NVARCHAR(100) NULL, 
        [summary] NVARCHAR(250) NULL, 
        [publishdate] DATETIME NULL 
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[StatisticsResearchers]
    (
	    [researcherId] INT NOT NULL PRIMARY KEY, 
        [name] NVARCHAR(100) NULL, 
        [credentials] NVARCHAR(MAX) NULL,
        [bday] DATE NULL
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
    CREATE TABLE [dbo].[StatisticsResults]
    (
	    [statId] INT NOT NULL PRIMARY KEY, 
        [projectId] INT NULL, 
        [year] INT NULL, 
        [month] INT NULL, 
        [day] INT NULL,
        [test] FLOAT NULL, 
        [result] FLOAT NULL, 
        [country] NVARCHAR(3) NULL, 
        [city] NVARCHAR(45) NULL, 
        [state] NVARCHAR(5) NULL, 
        [topic] NVARCHAR(50) NULL, 
        [target] NVARCHAR(50) NULL, 
        [sentence] NVARCHAR(250) NULL
    )
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE TABLE [dbo].[SubjectWords]
	(
		[wordId] INT NOT NULL PRIMARY KEY, 
		[subjectId] INT NOT NULL
	)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

BEGIN TRY
	CREATE TABLE [dbo].[Words]
	(
		[wordId] INT NOT NULL PRIMARY KEY,
		[word] NVARCHAR(50) NOT NULL, 
		[grammartype] INT NULL, 
		[score] INT NULL
	)
END TRY BEGIN CATCH END CATCH
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Blacklist_Domains_GetList')
	DROP PROCEDURE [dbo].[Blacklist_Domains_GetList]
GO
CREATE PROCEDURE [dbo].[Blacklist_Domains_GetList]
AS
	SELECT domain FROM Blacklist_Domains ORDER BY domain ASC
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Blacklist_Domain_Add')
	DROP PROCEDURE [dbo].[Blacklist_Domain_Add]
GO
CREATE PROCEDURE [dbo].[Blacklist_Domain_Add]
	@domain nvarchar(64)
AS
	DECLARE @domainId int
	BEGIN TRY
	INSERT INTO Blacklist_Domains (domain) VALUES (@domain)
	END TRY
	BEGIN CATCH
	END CATCH
	SELECT @domainId=domainId FROM Domains WHERE domain=@domain

	-- delete all articles related to domain
	DECLARE @cursor CURSOR, @articleId int
	SET @cursor = CURSOR FOR
	SELECT articleId FROM Articles WHERE url LIKE '%' + @domain + '/%'
	OPEN @cursor
	FETCH NEXT FROM @cursor INTO @articleId
	WHILE @@FETCH_STATUS = 0 BEGIN
		DELETE FROM ArticleBugs WHERE articleId=@articleId
		DELETE FROM ArticleDates WHERE articleId=@articleId
		DELETE FROM ArticleSentences WHERE articleId=@articleId
		DELETE FROM ArticleSubjects WHERE articleId=@articleId
		DELETE FROM ArticleWords WHERE articleId=@articleId
		DELETE FROM Articles WHERE articleId=@articleId
		FETCH NEXT FROM @cursor INTO @articleId
	END
	CLOSE @cursor
	DEALLOCATE @cursor

	--delete all download queue related to domain
	DELETE FROM DownloadQueue WHERE domainId=@domainId
	DELETE FROM Domains WHERE domainId=@domainId
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleBugs_GetList')
	DROP PROCEDURE [dbo].[ArticleBugs_GetList]
GO
CREATE PROCEDURE [dbo].[ArticleBugs_GetList]
	@articleId int = 0,
	@start int = 1,
	@length int = 50,
	@orderby int = 1
AS
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER(ORDER BY 
		CASE WHEN @orderby = 1 THEN [status] END ASC,
		CASE WHEN @orderby = 2 THEN [status] END DESC,
		CASE WHEN @orderby = 3 THEN datecreated END ASC,
		CASE WHEN @orderby = 4 THEN datecreated END DESC
		) AS rownum, * FROM ArticleBugs 
			WHERE articleId = CASE WHEN @articleId > 0 THEN @articleId ELSE articleId END
	) AS tbl WHERE rownum >= @start AND rownum < @start + @length

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleBug_Add')
	DROP PROCEDURE [dbo].[ArticleBug_Add]
GO
CREATE PROCEDURE [dbo].[ArticleBug_Add]
	@articleId int = 0,
	@title nvarchar(100) = '',
	@description nvarchar(MAX) = '',
	@status tinyint = 0
AS
	DECLARE @bugId int = NEXT VALUE FOR SequenceArticleBugs
	INSERT INTO ArticleBugs (bugId, articleId, title, [description], datecreated, [status])
	VALUES (@bugId, @articleId, @title, @description, GETDATE(), @status)

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleBug_UpdateDescription')
	DROP PROCEDURE [dbo].[ArticleBug_UpdateDescription]
GO
CREATE PROCEDURE [dbo].[ArticleBug_UpdateDescription]
	@bugId int = 0,
	@description nvarchar(MAX) = ''
AS
	UPDATE ArticleBugs SET description=@description WHERE bugId=@bugId

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleBug_UpdateStatus')
	DROP PROCEDURE [dbo].[ArticleBug_UpdateStatus]
GO
CREATE PROCEDURE [dbo].[ArticleBug_UpdateStatus]
	@bugId int = 0,
	@status int = 0
AS
	UPDATE ArticleBugs SET [status]=@status WHERE bugId=@bugId

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleDate_Add')
	DROP PROCEDURE [dbo].[ArticleDate_Add]
GO
CREATE PROCEDURE [dbo].[ArticleDate_Add]
	@articleId int = 0,
	@date date,
	@hasyear bit = 0,
	@hasmonth bit = 0,
	@hasday bit = 0
AS
	INSERT INTO ArticleDates (articleId, [date], hasyear, hasmonth, hasday)
	VALUES (@articleId, @date, @hasyear, @hasmonth, @hasday)
RETURN 0

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleSentences_Remove')
	DROP PROCEDURE [dbo].[ArticleSentences_Remove]
GO
CREATE PROCEDURE [dbo].[ArticleSentences_Remove]
	@articleId int = 0
AS
	DELETE FROM ArticleSentences WHERE articleId=@articleId
RETURN 0

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleSentence_Add')
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

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleSubjects_Remove')
	DROP PROCEDURE [dbo].[ArticleSubjects_Remove]
GO
CREATE PROCEDURE [dbo].[ArticleSubjects_Remove]
	@articleId int = 0,
	@subjectId int = 0
AS
	IF @subjectId = 0 BEGIN
		DELETE FROM ArticleSubjects WHERE articleId=@articleId
	END ELSE BEGIN
		DELETE FROM ArticleSubjects WHERE articleId=@articleId AND subjectId=@subjectId
	END
RETURN 0

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleSubject_Add')
	DROP PROCEDURE [dbo].[ArticleSubject_Add]
GO
CREATE PROCEDURE [dbo].[ArticleSubject_Add]
	@articleId int = 0,
	@subjectId int = 0,
	@datepublished datetime = null,
	@score int = 0
AS
	IF (SELECT COUNT(*) FROM ArticleSubjects WHERE articleId=@articleId AND subjectId=@subjectId) = 0 BEGIN
		INSERT INTO ArticleSubjects (articleId, subjectId, datecreated, datepublished, score) 
		VALUES (@articleId, @subjectId, GETDATE(), @datepublished, @score)
	END

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Articles_GetList')
	DROP PROCEDURE [dbo].[Articles_GetList]
GO
CREATE PROCEDURE [dbo].[Articles_GetList]
	@subjectIds nvarchar(MAX),
	@search nvarchar(MAX),
	@isActive int = 2,
	@isDeleted bit = 0,
	@minImages int = 0,
	@dateStart nvarchar(50),
	@dateEnd nvarchar(50),
	@orderby int = 1,
	@start int = 1,
	@length int = 50,
	@bugsonly bit = 0
AS
	/* set default dates */
	IF (@dateStart IS NULL OR @dateStart = '') BEGIN SET @dateStart = DATEADD(YEAR, -100, GETDATE()) END
	IF (@dateEnd IS NULL OR @dateEnd = '') BEGIN SET @dateEnd = DATEADD(YEAR, 100, GETDATE()) END

	/* get subjects from array */
	SELECT * INTO #subjects FROM dbo.SplitArray(@subjectIds, ',')
	SELECT articleId INTO #subjectarticles FROM ArticleSubjects
	WHERE subjectId IN (SELECT CONVERT(int, value) FROM #subjects)
	AND datecreated >= CONVERT(datetime, @dateStart) AND datecreated <= CONVERT(datetime, @dateEnd)

	/* get articles that match a search term */
	SELECT * INTO #search FROM dbo.SplitArray(@search, ',')
	SELECT wordid INTO #wordids FROM Words WHERE word IN (SELECT value FROM #search)
	SELECT articleId INTO #searchedarticles FROM ArticleWords
	WHERE wordId IN (SELECT * FROM #wordids)

	/* get list of articles that match filter */
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER(ORDER BY 
			CASE WHEN @orderby = 1 THEN a.datecreated END ASC,
			CASE WHEN @orderby = 2 THEN a.datecreated END DESC,
			CASE WHEN @orderby = 3 THEN a.score END ASC,
			CASE WHEN @orderby = 4 THEN a.score END DESC
		) AS rownum, a.*,
		s.breadcrumb, s.hierarchy, s.title AS subjectTitle
		FROM Articles a 
		LEFT JOIN Subjects s ON s.subjectId=a.subjectId
		WHERE
		(
			a.articleId IN (SELECT * FROM #subjectarticles)
			OR a.articleId IN (SELECT * FROM #searchedarticles)
			OR a.articleId = CASE WHEN @subjectIds = '' THEN a.articleId ELSE 0 END
			OR a.title LIKE '%' + @search + '%'
			OR a.summary LIKE '%' + @search + '%'
		) 
		AND a.active = CASE WHEN @isActive = 2 THEN a.active ELSE @isActive END
		AND a.deleted=@isDeleted
		AND a.images >= @minImages
		AND a.datecreated >= CONVERT(datetime, @dateStart) AND a.datecreated <= CONVERT(datetime, @dateEnd)
	) AS tbl WHERE rownum >= @start AND rownum < @start + @length

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Articles_GetListForFeeds')
	DROP PROCEDURE [dbo].[Articles_GetListForFeeds]
GO
CREATE PROCEDURE [dbo].[Articles_GetListForFeeds]
	@subjectIds nvarchar(MAX),
	@feedId int = -1,
	@search nvarchar(MAX),
	@isActive int = 2,
	@isDeleted bit = 0,
	@minImages int = 0,
	@dateStart nvarchar(50),
	@dateEnd nvarchar(50),
	@orderby int = 1,
	@start int = 1,
	@length int = 10,
	@bugsonly bit = 0
AS
	
	/* create results table */
	DECLARE @results TABLE(
		rownum int,
		feedId INT NULL DEFAULT 0, 
		isfeed BIT NULL DEFAULT 0,
		feedTitle NVARCHAR(100) NULL DEFAULT '', 
		feedUrl NVARCHAR(100) NULL DEFAULT '', 
		feedCheckIntervals INT DEFAULT 1440,
		feedLastChecked DATETIME NULL, 
		feedFilter NVARCHAR(MAX) NULL DEFAULT '',
		articleId INT NULL DEFAULT 0, 
		subjects TINYINT NULL DEFAULT 0,
		images TINYINT NULL DEFAULT 0, 
		filesize FLOAT NULL DEFAULT 0,
		wordcount INT NULL DEFAULT 0, 
		sentencecount SMALLINT NULL DEFAULT 0, 
		paragraphcount SMALLINT NULL DEFAULT 0,
		importantcount SMALLINT NULL DEFAULT 0, 
		analyzecount SMALLINT NULL DEFAULT 0,
		yearstart SMALLINT NULL, 
		yearend SMALLINT NULL, 
		years NVARCHAR(50),
		datecreated DATETIME NULL, 
		datepublished DATETIME NULL, 
		relavance SMALLINT NULL DEFAULT 0, 
		importance SMALLINT NULL DEFAULT 0, 
		fiction SMALLINT NULL DEFAULT 1, 
		domain NVARCHAR(50) NULL DEFAULT '', 
		url NVARCHAR(250) NULL DEFAULT '', 
		title NVARCHAR(250) NULL DEFAULT '', 
		summary NVARCHAR(250) NULL DEFAULT '',
		breadcrumb NVARCHAR(250) NULL DEFAULT '',
		hierarchy NVARCHAR(50) NULL DEFAULT '',
		subjectId INT NULL DEFAULT 0,
		subjectTitle NVARCHAR(50) NULL DEFAULT '',
		score INT NULL DEFAULT 0,
		analyzed FLOAT NULL DEFAULT 0,
		cached BIT NULL DEFAULT 0, 
		active BIT NULL DEFAULT 0, 
		deleted BIT NULL DEFAULT 0, 
		bugsopen SMALLINT NULL DEFAULT 0, 
		bugsresolved SMALLINT NULL DEFAULT 0
	)

	DECLARE 
	@cursor1 CURSOR, 
	@cursor2 CURSOR, 
	@rownum int,
	@feedId1 int,
	@feedId2 int,
	@feedTitle nvarchar(100),
	@feedUrl nvarchar(100),
	@feedcheckIntervals int,
	@feedLastChecked datetime,
	@feedFilter nvarchar(MAX),
	@articleId INT,
	@subjects TINYINT,
    @images TINYINT, 
	@filesize FLOAT,
    @wordcount INT, 
    @sentencecount SMALLINT, 
    @paragraphcount SMALLINT,
    @importantcount SMALLINT, 
	@analyzecount SMALLINT,
    @yearstart SMALLINT, 
    @yearend SMALLINT, 
	@years NVARCHAR(50),
    @datecreated DATETIME, 
    @datepublished DATETIME, 
    @relavance SMALLINT, 
    @importance SMALLINT, 
    @fiction SMALLINT, 
    @domain NVARCHAR(50), 
    @url NVARCHAR(250), 
    @title NVARCHAR(250), 
    @summary NVARCHAR(250),
	@breadcrumb NVARCHAR(500),
	@hierarchy NVARCHAR(50),
	@subjectId INT,
	@subjectTitle nvarchar(50),
	@score INT,
	@analyzed FLOAT,
	@cached BIT, 
    @active BIT, 
    @deleted BIT, 
    @bugsopen SMALLINT, 
    @bugsresolved SMALLINT

	/* set default dates */
	IF (@dateStart IS NULL OR @dateStart = '') BEGIN SET @dateStart = DATEADD(YEAR, -100, GETDATE()) END
	IF (@dateEnd IS NULL OR @dateEnd = '') BEGIN SET @dateEnd = DATEADD(YEAR, 100, GETDATE()) END

	/* get subjects from array */
	SELECT * INTO #subjects FROM dbo.SplitArray(@subjectIds, ',')
	SELECT articleId INTO #subjectarticles FROM ArticleSubjects
	WHERE subjectId IN (SELECT CONVERT(int, value) FROM #subjects)
	AND datecreated >= CONVERT(datetime, @dateStart) AND datecreated <= CONVERT(datetime, @dateEnd)
	
	/* get articles that match a search term */
	SELECT * INTO #search FROM dbo.SplitArray(@search, ',')
	SELECT wordId INTO #wordids FROM Words WHERE word IN (SELECT value FROM #search)
	SELECT articleId INTO #searchedarticles FROM ArticleWords WHERE wordId IN (SELECT * FROM #wordids)
	
	/* first, get feeds list //////////////////////////////////////////////////////////////////////////////////////////// */
	SELECT * INTO #feeds FROM Feeds WHERE feedId = CASE WHEN @feedId >= 0 THEN @feedId ELSE feedId END ORDER BY title ASC
	SET @cursor1 = CURSOR FOR
	SELECT * FROM #feeds
	OPEN @cursor1
	FETCH FROM @cursor1 INTO
	@feedId1, @feedTitle, @feedUrl, @feedcheckIntervals, @feedLastChecked, @feedFilter

	WHILE @@FETCH_STATUS = 0 BEGIN
		/* get a list of feeds */
		INSERT INTO @results (feedId, isfeed, feedTitle, feedUrl, feedCheckIntervals, feedLastChecked, feedFilter)
		VALUES (@feedId1, 1, @feedTitle, @feedUrl, @feedcheckIntervals, @feedLastChecked, @feedFilter)
		
		FETCH FROM @cursor1 INTO
		@feedId1, @feedTitle, @feedUrl, @feedcheckIntervals, @feedLastChecked, @feedFilter
	END
	CLOSE @cursor1
	DEALLOCATE @cursor1

	/* next, loop through feeds list to get articles for each feed ////////////////////////////////////////////////////// */
	SET @cursor1 = CURSOR FOR
	SELECT feedId FROM #feeds
	OPEN @cursor1
	FETCH FROM @cursor1 INTO @feedId1

	WHILE @@FETCH_STATUS = 0 BEGIN
		/* get 10 articles for each feed */
		SET @cursor2 = CURSOR FOR
		SELECT * FROM (
			SELECT ROW_NUMBER() OVER(ORDER BY 
			CASE WHEN @orderby = 1 THEN a.datecreated END ASC,
			CASE WHEN @orderby = 2 THEN a.datecreated END DESC,
			CASE WHEN @orderby = 3 THEN a.score END ASC,
			CASE WHEN @orderby = 4 THEN a.score END DESC
			) AS rownum, a.*,
			(SELECT COUNT(*) FROM ArticleBugs WHERE articleId=a.articleId AND status=0) AS bugsopen,
			(SELECT COUNT(*) FROM ArticleBugs WHERE articleId=a.articleId AND status=1) AS bugsresolved,
			s.breadcrumb, s.hierarchy, s.title AS subjectTitle
			FROM Articles a 
			LEFT JOIN ArticleSubjects asub ON asub.articleId=a.articleId AND asub.subjectId=a.subjectId
			LEFT JOIN Subjects s ON s.subjectId=a.subjectId
			WHERE feedId=@feedId1
			AND 
			(
				a.articleId IN (SELECT * FROM #subjectarticles)
				OR a.articleId IN (SELECT * FROM #searchedarticles)
				OR a.articleId = CASE WHEN @subjectIds = '' THEN a.articleId ELSE 0 END
			) 
			AND a.active = CASE WHEN @isActive = 2 THEN a.active ELSE @isActive END
			AND a.deleted=@isDeleted
			AND a.images >= @minImages
			AND a.datecreated >= CONVERT(datetime, @dateStart) AND a.datecreated <= CONVERT(datetime, @dateEnd)

			AND (
				a.articleId = CASE 
				WHEN @search = '' THEN a.articleId 
				ELSE (SELECT articleId FROM #searchedarticles WHERE articleId=a.articleId)
				END
				OR a.title LIKE CASE WHEN @search <> '' THEN '%' + @search + '%' ELSE '_81!!{}!' END /*either return search term or find an impossibly random text*/
				OR a.summary LIKE CASE WHEN @search <> '' THEN '%' + @search + '%' ELSE '_81!!{}!' END /*either return search term or find an impossibly random text*/
				)
			AND a.articleId = CASE 
			WHEN @bugsonly=1 THEN (SELECT TOP 1 articleId FROM ArticleBugs WHERE articleId=a.articleId) 
			ELSE a.articleId END
		) AS tbl WHERE rownum >= @start AND rownum < @start + @length
		OPEN @cursor2
		FETCH FROM @cursor2 INTO
		@rownum, @articleId, @feedId2, @subjects, @subjectId, @score, @images, @filesize, @wordcount, @sentencecount, 
		@paragraphcount, @importantcount, @analyzecount, @yearstart, @yearend, @years, @datecreated, @datepublished, 
		@relavance, @importance, @fiction, @domain, @url, @title, @summary, @analyzed, @cached, @active, @deleted,
		@bugsopen, @bugsresolved, @breadcrumb, @hierarchy, @subjectTitle

		WHILE @@FETCH_STATUS = 0 BEGIN
			INSERT INTO @results (rownum, articleId, feedId, subjects, subjectId, score, images, filesize, wordcount, sentencecount, 
			paragraphcount, importantcount, analyzecount, yearstart, yearend, years, datecreated, datepublished, 
			relavance, importance, fiction, domain, url, title, summary, analyzed, cached,  active, deleted,
			bugsopen, bugsresolved, breadcrumb, hierarchy, subjectTitle)
			VALUES (@rownum, @articleId, @feedId1, @subjects, @subjectId, @score, @images, @filesize, @wordcount, @sentencecount, 
			@paragraphcount, @importantcount, @analyzecount, @yearstart, @yearend, @years, @datecreated, @datepublished, 
			@relavance, @importance, @fiction, @domain, @url, @title, @summary, @analyzed, @cached, @active, @deleted,
			@bugsopen, @bugsresolved, @breadcrumb, @hierarchy, @subjectTitle)

			FETCH FROM @cursor2 INTO
			@rownum, @articleId, @feedId2, @subjects, @subjectId, @score, @images, @filesize, @wordcount, @sentencecount, 
			@paragraphcount, @importantcount, @analyzecount, @yearstart, @yearend, @years, @datecreated, @datepublished, 
			@relavance, @importance, @fiction, @domain, @url, @title, @summary, @analyzed, @cached, @active, @deleted,
		@bugsopen, @bugsresolved, @breadcrumb, @hierarchy, @subjectTitle
		END
		CLOSE @cursor2
		DEALLOCATE @cursor2
		
		FETCH FROM @cursor1 INTO @feedId1
	END
	CLOSE @cursor1
	DEALLOCATE @cursor1

	SELECT * FROM @results ORDER BY isfeed DESC, feedTitle ASC

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Articles_GetListForSubjects')
	DROP PROCEDURE [dbo].[Articles_GetListForSubjects]
GO
CREATE PROCEDURE [dbo].[Articles_GetListForSubjects]
	@subjectIds nvarchar(MAX),
	@search nvarchar(MAX),
	@isActive int = 2,
	@isDeleted bit = 0,
	@minImages int = 0,
	@dateStart nvarchar(50),
	@dateEnd nvarchar(50),
	@orderby int = 1,
	@start int = 1,
	@length int = 10,
	@subjectStart int = 1,
	@subjectLength int = 10,
	@bugsonly bit = 0
AS
	
	/* create results table */
	DECLARE @results TABLE(
		rownum int,
		feedId INT NULL DEFAULT 0, 
		articleId INT NULL DEFAULT 0, 
		subjects TINYINT NULL DEFAULT 0,
		images TINYINT NULL DEFAULT 0, 
		filesize FLOAT NULL DEFAULT 0,
		wordcount INT NULL DEFAULT 0, 
		sentencecount SMALLINT NULL DEFAULT 0, 
		paragraphcount SMALLINT NULL DEFAULT 0,
		importantcount SMALLINT NULL DEFAULT 0, 
		analyzecount SMALLINT NULL DEFAULT 0,
		yearstart SMALLINT NULL, 
		yearend SMALLINT NULL, 
		years NVARCHAR(50),
		datecreated DATETIME NULL, 
		datepublished DATETIME NULL, 
		relavance SMALLINT NULL DEFAULT 0, 
		importance SMALLINT NULL DEFAULT 0, 
		fiction SMALLINT NULL DEFAULT 1, 
		domain NVARCHAR(50) NULL DEFAULT '', 
		url NVARCHAR(250) NULL DEFAULT '', 
		title NVARCHAR(250) NULL DEFAULT '', 
		summary NVARCHAR(250) NULL DEFAULT '',
		breadcrumb NVARCHAR(250) NULL DEFAULT '',
		hierarchy NVARCHAR(50) NULL DEFAULT '',
		subjectId INT NULL DEFAULT 0,
		issubject BIT NULL DEFAULT 0,
		subjectTitle NVARCHAR(50) NULL DEFAULT '',
		score INT NULL DEFAULT 0,
		analyzed FLOAT NULL DEFAULT 0,
		cached BIT NULL DEFAULT 0, 
		active BIT NULL DEFAULT 0, 
		deleted BIT NULL DEFAULT 0, 
		bugsopen SMALLINT NULL DEFAULT 0, 
		bugsresolved SMALLINT NULL DEFAULT 0
	)

	DECLARE 
	@cursor1 CURSOR, 
	@cursor2 CURSOR, 
	@rownum int,
	@feedId int,
	@articleId INT,
	@subjects TINYINT,
    @images TINYINT, 
	@filesize FLOAT,
    @wordcount INT, 
    @sentencecount SMALLINT, 
    @paragraphcount SMALLINT,
    @importantcount SMALLINT, 
	@analyzecount SMALLINT,
    @yearstart SMALLINT, 
    @yearend SMALLINT, 
	@years NVARCHAR(50),
    @datecreated DATETIME, 
    @datepublished DATETIME, 
    @relavance SMALLINT, 
    @importance SMALLINT, 
    @fiction SMALLINT, 
    @domain NVARCHAR(50), 
    @url NVARCHAR(250), 
    @title NVARCHAR(250), 
    @summary NVARCHAR(250),
	@breadcrumb NVARCHAR(500),
	@hierarchy NVARCHAR(50),
	@subjectId INT,
	@subjectTitle nvarchar(50),
	@score INT,
	@analyzed FLOAT,
	@cached BIT, 
    @active BIT, 
    @deleted BIT, 
    @bugsopen SMALLINT, 
    @bugsresolved SMALLINT

	/* set default dates */
	IF (@dateStart IS NULL OR @dateStart = '') BEGIN SET @dateStart = DATEADD(YEAR, -100, GETDATE()) END
	IF (@dateEnd IS NULL OR @dateEnd = '') BEGIN SET @dateEnd = DATEADD(YEAR, 100, GETDATE()) END

	/* get subjects from array */

	SELECT subjectId, title INTO #subjects FROM 
		(SELECT ROW_NUMBER() OVER(ORDER BY breadcrumb) AS rownum, subjectId, title FROM Subjects
		WHERE subjectId IN (SELECT valueInt FROM dbo.SplitArray(@subjectIds, ',')) 
		OR subjectId = CASE WHEN @subjectIds = '' THEN subjectId ELSE -1 END) AS tbl
	WHERE rownum >= @subjectStart AND rownum < @subjectStart + @subjectLength
	
	/* get articles that match a search term */
	SELECT * INTO #search FROM dbo.SplitArray(@search, ',')
	SELECT wordId INTO #wordids FROM Words WHERE word IN (SELECT value FROM #search)
	SELECT articleId INTO #searchedarticles FROM ArticleWords WHERE wordId IN (SELECT * FROM #wordids)
	
	/* first, get subjects list //////////////////////////////////////////////////////////////////////////////////////////// */
	SET @cursor1 = CURSOR FOR
	SELECT * FROM #subjects
	OPEN @cursor1
	FETCH FROM @cursor1 INTO
	@subjectId, @subjectTitle

	WHILE @@FETCH_STATUS = 0 BEGIN
		/* get a list of subjects */
		INSERT INTO @results (subjectId, issubject, subjectTitle)
		VALUES (@subjectId, 1, @subjectTitle)
		
		FETCH FROM @cursor1 INTO
		@subjectId, @subjectTitle
	END
	CLOSE @cursor1
	DEALLOCATE @cursor1

	/* next, loop through subjects list to get articles for each subject ////////////////////////////////////////////////////// */
	SET @cursor1 = CURSOR FOR
	SELECT subjectId FROM #subjects
	OPEN @cursor1
	FETCH FROM @cursor1 INTO @subjectId

	WHILE @@FETCH_STATUS = 0 BEGIN
		/* get 10 articles for each subject */
		SET @cursor2 = CURSOR FOR
		SELECT * FROM (
			SELECT ROW_NUMBER() OVER(ORDER BY 
			CASE WHEN @orderby = 1 THEN a.datecreated END ASC,
			CASE WHEN @orderby = 2 THEN a.datecreated END DESC,
			CASE WHEN @orderby = 3 THEN a.score END ASC,
			CASE WHEN @orderby = 4 THEN a.score END DESC
			) AS rownum, a.*,
			(SELECT COUNT(*) FROM ArticleBugs WHERE articleId=a.articleId AND status=0) AS bugsopen,
			(SELECT COUNT(*) FROM ArticleBugs WHERE articleId=a.articleId AND status=1) AS bugsresolved,
			s.breadcrumb, s.hierarchy, s.title AS subjectTitle
			FROM Articles a 
			LEFT JOIN ArticleSubjects asub ON asub.articleId=a.articleId AND asub.subjectId=a.subjectId
			LEFT JOIN Subjects s ON s.subjectId=a.subjectId
			WHERE a.subjectId=@subjectId
			AND 
			(
				a.articleId IN (SELECT * FROM #searchedarticles)
				OR a.articleId = CASE WHEN @subjectIds = '' THEN a.articleId ELSE 0 END
			) 
			AND a.active = CASE WHEN @isActive = 2 THEN a.active ELSE @isActive END
			AND a.deleted=@isDeleted
			AND a.images >= @minImages
			AND a.datecreated >= CONVERT(datetime, @dateStart) AND a.datecreated <= CONVERT(datetime, @dateEnd)

			AND (
				a.articleId = CASE 
				WHEN @search = '' THEN a.articleId 
				ELSE (SELECT articleId FROM #searchedarticles WHERE articleId=a.articleId)
				END
				OR a.title LIKE CASE WHEN @search <> '' THEN '%' + @search + '%' ELSE '_81!!{}!' END /*either return search term or find an impossibly random text*/
				OR a.summary LIKE CASE WHEN @search <> '' THEN '%' + @search + '%' ELSE '_81!!{}!' END /*either return search term or find an impossibly random text*/
				)
			AND a.articleId = CASE 
			WHEN @bugsonly=1 THEN (SELECT TOP 1 articleId FROM ArticleBugs WHERE articleId=a.articleId) 
			ELSE a.articleId END
		) AS tbl WHERE rownum >= @start AND rownum < @start + @length
		OPEN @cursor2
		FETCH FROM @cursor2 INTO
		@rownum, @articleId, @feedId, @subjects, @subjectId, @score, @images, @filesize, @wordcount, @sentencecount, 
		@paragraphcount, @importantcount, @analyzecount, @yearstart, @yearend, @years, @datecreated, @datepublished, 
		@relavance, @importance, @fiction, @domain, @url, @title, @summary, @analyzed, @cached, @active, @deleted,
		@bugsopen, @bugsresolved, @breadcrumb, @hierarchy, @subjectTitle

		WHILE @@FETCH_STATUS = 0 BEGIN
			INSERT INTO @results (rownum, articleId, feedId, subjects, subjectId, score, images, filesize, wordcount, sentencecount, 
			paragraphcount, importantcount, analyzecount, yearstart, yearend, years, datecreated, datepublished, 
			relavance, importance, fiction, domain, url, title, summary, analyzed, cached,  active, deleted,
			bugsopen, bugsresolved, breadcrumb, hierarchy, subjectTitle)
			VALUES (@rownum, @articleId, @feedId, @subjects, @subjectId, @score, @images, @filesize, @wordcount, @sentencecount, 
			@paragraphcount, @importantcount, @analyzecount, @yearstart, @yearend, @years, @datecreated, @datepublished, 
			@relavance, @importance, @fiction, @domain, @url, @title, @summary, @analyzed, @cached, @active, @deleted,
			@bugsopen, @bugsresolved, @breadcrumb, @hierarchy, @subjectTitle)

			FETCH FROM @cursor2 INTO
			@rownum, @articleId, @feedId, @subjects, @subjectId, @score, @images, @filesize, @wordcount, @sentencecount, 
			@paragraphcount, @importantcount, @analyzecount, @yearstart, @yearend, @years, @datecreated, @datepublished, 
			@relavance, @importance, @fiction, @domain, @url, @title, @summary, @analyzed, @cached, @active, @deleted,
		@bugsopen, @bugsresolved, @breadcrumb, @hierarchy, @subjectTitle
		END
		CLOSE @cursor2
		DEALLOCATE @cursor2
		
		FETCH FROM @cursor1 INTO @subjectId
	END
	CLOSE @cursor1
	DEALLOCATE @cursor1

	SELECT * FROM @results ORDER BY issubject DESC, subjectTitle ASC

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleWords_Remove')
	DROP PROCEDURE [dbo].[ArticleWords_Remove]
GO
CREATE PROCEDURE [dbo].[ArticleWords_Remove]
	@articleId int = 0,
	@word nvarchar(50) = ''
AS
	IF @word = '' BEGIN
		DELETE FROM ArticleWords WHERE articleId=@articleId
	END ELSE BEGIN
		DECLARE @wordId int = 0
		SELECT @wordId=wordId FROM words WHERE word=@word
		DELETE FROM ArticleWords WHERE articleId=@articleId AND wordId=@wordId
	END
RETURN 0

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ArticleWord_Add')
	DROP PROCEDURE [dbo].[ArticleWord_Add]
GO
CREATE PROCEDURE [dbo].[ArticleWord_Add]
	@articleId int = 0,
	@wordId int = 0,
	@count int = 0
AS
	IF (SELECT COUNT(*) FROM ArticleWords WHERE articleId=@articleId AND wordId=@wordId) = 0 BEGIN
		INSERT INTO ArticleWords (articleId, wordId, [count]) 
		VALUES (@articleId, @wordId, @count)
	END

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_Add')
	DROP PROCEDURE [dbo].[Article_Add]
GO
	CREATE PROCEDURE [dbo].[Article_Add]
		@feedId int = 0,
		@subjects int = 0,
		@subjectId int = 0,
		@score smallint = 0,
		@domain nvarchar(50),
		@url nvarchar(250),
		@title nvarchar(250),
		@summary nvarchar(250),
		@filesize float = 0,
		@wordcount int = 0,
		@sentencecount smallint = 0,
		@paragraphcount smallint = 0,
		@importantcount smallint = 0,
		@yearstart smallint = 0,
		@yearend smallint = 0,
		@years nvarchar(50),
		@images tinyint = 0,
		@datepublished datetime,
		@relavance smallint = 1,
		@importance smallint = 1,
		@fiction smallint = 1,
		@analyzed float = 0.1,
		@active bit = 1
	AS
		DECLARE @articleId int = NEXT VALUE FOR SequenceArticles
		INSERT INTO Articles 
		(articleId, feedId, subjects, subjectId, score, domain, url, title, summary, filesize, wordcount, sentencecount, paragraphcount, importantcount, analyzecount,
		yearstart, yearend, years, images, datecreated, datepublished, relavance, importance, fiction, analyzed, active)
		VALUES 
		(@articleId, @feedId, @subjects, @subjectId, @score, @domain, @url, @title, @summary, @filesize, @wordcount, @sentencecount, @paragraphcount, @importantcount, 1,
		@yearstart, @yearend, @years, @images, GETDATE(), @datepublished, @relavance, @importance, @fiction, @analyzed, @active)

		SELECT @articleId

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_Clean')
	DROP PROCEDURE [dbo].[Article_Clean]
GO
CREATE PROCEDURE [dbo].[Article_Clean]
	@articleId int = 0
AS
	EXEC ArticleSubjects_Remove @articleId=@articleId
	EXEC ArticleWords_Remove @articleId=@articleId
RETURN 0

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_Exists')
	DROP PROCEDURE [dbo].[Article_Exists]
GO
CREATE PROCEDURE [dbo].[Article_Exists]
	@url nvarchar(250)
AS
	SELECT COUNT(*) FROM Articles WHERE url=@url

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_GetByUrl')
	DROP PROCEDURE [dbo].[Article_GetByUrl]
GO
CREATE PROCEDURE [dbo].[Article_GetByUrl]
	@url nvarchar(250)
AS
	SELECT * FROM Articles WHERE url=@url
RETURN 0

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_Remove')
	DROP PROCEDURE [dbo].[Article_Remove]
GO
CREATE PROCEDURE [dbo].[Article_Remove]
	@articleId int = 0
AS
	DELETE FROM ArticleSentences WHERE articleId=@articleId
	DELETE FROM ArticleWords WHERE articleId=@articleId
	DELETE FROM ArticleSubjects WHERE articleId=@articleId
	/* DELETE FROM ArticleStatistics WHERE articleId=@articleId */
	DELETE FROM Articles WHERE articleId=@articleId
RETURN 0

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Article_Update')
	DROP PROCEDURE [dbo].[Article_Update]
GO
CREATE PROCEDURE [dbo].[Article_Update]
	@articleId int = 0,
	@subjects int = 0,
	@subjectId int = 0,
	@score smallint = 0,
	@title nvarchar(250),
	@summary nvarchar(250),
	@filesize float = 0,
	@wordcount int = 0,
	@sentencecount int = 0,
	@paragraphcount int = 0,
	@importantcount int = 0,
	@yearstart int = 0,
	@yearend int = 0,
	@years nvarchar(50),
	@images int = 0,
	@datepublished datetime,
	@relavance smallint = 1,
	@importance smallint = 1,
	@fiction smallint = 1,
	@analyzed float = 0.1
AS

UPDATE Articles SET 
subjects=@subjects, subjectId=@subjectId, score=@score, title=@title, summary=@summary, filesize=@filesize, wordcount=@wordcount, sentencecount=@sentencecount,
paragraphcount=@paragraphcount, importantcount=@importantcount, analyzecount=analyzecount+1, 
yearstart=@yearstart, yearend=@yearend, years=@years, images=@images, datepublished=@datepublished, 
relavance=@relavance, importance=@importance, fiction=@fiction, analyzed=@analyzed
WHERE articleId=@articleId

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DownloadQueue_Add')
	DROP PROCEDURE [dbo].[DownloadQueue_Add]
GO
CREATE PROCEDURE [dbo].[DownloadQueue_Add]
	@urls nvarchar(MAX) = '', --comma delimited list
	@domain nvarchar(64) = '',
	@feedId int = 0
AS
SELECT * INTO #urls FROM dbo.SplitArray(@urls, ',')
DECLARE @cursor CURSOR, @url nvarchar(MAX), @domainId INT, @qid INT, @count INT = 0
IF EXISTS(SELECT * FROM Domains WHERE domain=@domain) BEGIN
	SELECT @domainId = domainId FROM Domains WHERE domain=@domain
END ELSE BEGIN
	SET @domainId = NEXT VALUE FOR SequenceDomains
	INSERT INTO Domains (domainId, domain, lastchecked) VALUES (@domainId, @domain, DATEADD(HOUR, -1, GETUTCDATE()))
END
SET @cursor = CURSOR FOR
SELECT [value] FROM #urls
OPEN @cursor
FETCH NEXT FROM @cursor INTO @url
WHILE @@FETCH_STATUS = 0 BEGIN
	IF (SELECT COUNT(*) FROM DownloadQueue WHERE url=@url) = 0 BEGIN
		IF (SELECT COUNT(*) FROM Articles WHERE url=@url) = 0 BEGIN
			SET @qid = NEXT VALUE FOR SequenceDownloadQueue
			INSERT INTO DownloadQueue (qid, url, feedId, domainId, [status], datecreated) 
			VALUES (@qid, @url, @feedId, @domainId, 0, GETDATE())
			SET @count += 1
		END
	END
	FETCH NEXT FROM @cursor INTO @url
END
CLOSE @cursor
DEALLOCATE @cursor
SELECT @count AS [count]

	
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DownloadQueue_Check')
	DROP PROCEDURE [dbo].[DownloadQueue_Check]
GO
CREATE PROCEDURE [dbo].[DownloadQueue_Check]
	@domaindelay int = 5 -- in minutes
AS
	DECLARE @qid int, @domainId int
	SELECT TOP 1 @qid = q.qid, @domainId = q.domainId
	FROM DownloadQueue q
	JOIN Domains d ON d.domainId = q.domainId
	WHERE q.status = 0
	AND d.lastchecked < DATEADD(MINUTE, 0 - @domaindelay, GETUTCDATE())

	IF @qid > 0 BEGIN
		UPDATE DownloadQueue SET status=1 WHERE qid=@qid
		UPDATE Domains SET lastchecked = GETUTCDATE()
		WHERE domainId = @domainId

		SELECT q.*, d.domain 
		FROM DownloadQueue q 
		JOIN Domains d ON d.domainId = q.domainId
		WHERE qid=@qid
	END
	
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Downloads_GetCount')
	DROP PROCEDURE [dbo].[Downloads_GetCount]
GO
CREATE PROCEDURE [dbo].[Downloads_GetCount]
	
AS
	SELECT COUNT(*) FROM DownloadQueue WHERE [status]=0

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Download_Update')
	DROP PROCEDURE [dbo].[Download_Update]
GO
CREATE PROCEDURE [dbo].[Download_Update]
	@qid int = 0,
	@status int = 0
AS
	UPDATE DownloadQueue SET status=@status WHERE qid=@qid
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feeds_Categories_GetList')
	DROP PROCEDURE [dbo].[Feeds_Categories_GetList]
GO
CREATE PROCEDURE [dbo].[Feeds_Categories_GetList]
AS
	SELECT * FROM FeedCategories ORDER BY title ASC

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feeds_Category_Add')
	DROP PROCEDURE [dbo].[Feeds_Category_Add]
GO
CREATE PROCEDURE [dbo].[Feeds_Category_Add]
	@title nvarchar(64)
AS

	DECLARE @id int = NEXT VALUE FOR SequenceFeedCategories
	INSERT INTO FeedCategories (categoryId, title) VALUES (@id, @title)

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feeds_Check')
	DROP PROCEDURE [dbo].[Feeds_Check]
GO
CREATE PROCEDURE [dbo].[Feeds_Check]
	
AS
	SELECT f.*, c.title AS category
	FROM Feeds f 
	JOIN FeedCategories c ON c.categoryId = f.categoryId
	WHERE f.lastChecked < DATEADD(HOUR, -24, GETUTCDATE())

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feeds_GetList')
	DROP PROCEDURE [dbo].[Feeds_GetList]
GO
CREATE PROCEDURE [dbo].[Feeds_GetList]
AS
SELECT f.*, fc.title AS category
FROM Feeds f
JOIN FeedCategories fc ON fc.categoryId = f.categoryId
WHERE feedId > 0 ORDER BY fc.title ASC, f.title ASC

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feeds_GetListWithLogs')
	DROP PROCEDURE [dbo].[Feeds_GetListWithLogs]
GO
CREATE PROCEDURE [dbo].[Feeds_GetListWithLogs]
	@days int = 7,
	@dateStart date
AS
	DECLARE 
	@cursor1 CURSOR,
	@cursor2 CURSOR,
	@feedId int,
	@title nvarchar(100),
	@url nvarchar(100),
	@checkIntervals int = 720,
	@lastChecked datetime,
	@filter nvarchar(MAX),
	@logfeedId INT,
	@loglinks smallint,
	@logdatechecked datetime

	DECLARE @tblresults TABLE (
		feedId int NOT NULL,
		title nvarchar(100) NULL,
		url nvarchar(100) NULL,
		checkIntervals int,
		lastChecked datetime NULL,
		filter nvarchar(MAX) NULL,
		loglinks smallint NULL,
		logdatechecked datetime NULL
	)


	SET @cursor1 = CURSOR FOR 
	SELECT * FROM feeds WHERE feedId > 0 ORDER BY checkIntervals ASC, title ASC
	OPEN @cursor1
	FETCH FROM @cursor1 INTO
	@feedId, @title, @url, @checkIntervals, @lastChecked, @filter
	WHILE @@FETCH_STATUS = 0 BEGIN
		/*add feed to results table */
		INSERT INTO @tblresults (feedId, title, url, checkIntervals, lastChecked, filter)
		VALUES (@feedId, @title, @url, @checkIntervals, @lastChecked, @filter)

		/* get log data for each feed */
		SET @cursor2 = CURSOR FOR 
		SELECT * FROM FeedsCheckedLog 
		WHERE feedId=@feedId 
		AND datechecked >= @dateStart
		AND datechecked <= DATEADD(DAY, @days, @dateStart)
		ORDER BY datechecked ASC
		OPEN @cursor2
		FETCH FROM @cursor2 INTO
		@logfeedId, @loglinks, @logdatechecked
		WHILE @@FETCH_STATUS = 0 BEGIN
			/* add feed log record to results table */
			INSERT INTO @tblresults (feedId, loglinks, logdatechecked)
			VALUES(@feedId, @loglinks, @logdatechecked)

			FETCH FROM @cursor2 INTO
			@logfeedId, @loglinks, @logdatechecked
		END
		CLOSE @cursor2
		DEALLOCATE @cursor2

		FETCH FROM @cursor1 INTO
		@feedId, @title, @url, @checkIntervals, @lastChecked, @filter
	END
	CLOSE @cursor1
	DEALLOCATE @cursor1

	/* finally, return results */
	SELECT * FROM @tblresults
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feed_Add')
	DROP PROCEDURE [dbo].[Feed_Add]
GO
CREATE PROCEDURE [dbo].[Feed_Add]
	@categoryId int,
	@title nvarchar(100) = '',
	@url nvarchar(100) = '',
	@filter nvarchar(MAX) = '',
	@checkIntervals int = 720 --(12 hours)
AS
	DECLARE @feedId int = NEXT VALUE FOR SequenceFeeds
	INSERT INTO Feeds (feedId, categoryId, title, url, checkIntervals, filter, lastChecked) 
	VALUES (@feedId, @categoryId, @title, @url, @checkIntervals, @filter, DATEADD(HOUR, -24, GETUTCDATE()))
	SELECT @feedId

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'FeedCheckedLog_Add')
	DROP PROCEDURE [dbo].[FeedCheckedLog_Add]
GO
CREATE PROCEDURE [dbo].[FeedCheckedLog_Add]
	@feedId int = 0,
	@links int = 0
AS
	INSERT INTO FeedsCheckedLog (feedId, links, datechecked)
	VALUES (@feedId, @links, GETDATE())
	UPDATE Feeds SET lastChecked = GETDATE()
/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feed_Checked')
	DROP PROCEDURE [dbo].[Feed_Checked]
GO
CREATE PROCEDURE [dbo].[Feed_Checked]
	@feedId int = 0
AS
	UPDATE Feeds SET lastChecked=GETDATE() WHERE feedId=@feedId
RETURN 0

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Subjects_GetList')
	DROP PROCEDURE [dbo].[Subjects_GetList]
GO
CREATE PROCEDURE [dbo].[Subjects_GetList]
	@subjectIds nvarchar(MAX),
	@parentId int = -1
AS
IF @subjectIds <> '' BEGIN
	SELECT * INTO #subjects FROM dbo.SplitArray(@subjectIds, ',')
	SELECT * FROM Subjects 
	WHERE subjectId IN (SELECT CONVERT(int, value) FROM #subjects)
	AND parentId = CASE WHEN @parentId >= 0 THEN @parentId ELSE parentId END
	ORDER BY title ASC
END ELSE BEGIN
/* parentId only */
	SELECT * FROM Subjects 
	WHERE parentId = CASE WHEN @parentId >= 0 THEN @parentId ELSE parentId END
	ORDER BY title ASC
END


/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Subject_Create')
	DROP PROCEDURE [dbo].[Subject_Create]
GO
CREATE PROCEDURE [dbo].[Subject_Create]
	@parentId int = 0,
	@grammartype int = 0,
	@score int = 0,
	@title nvarchar(50),
	@breadcrumb nvarchar(MAX) = ''
AS
	DECLARE @create bit = 1, @hierarchy nvarchar(50) = ''
	IF @parentId > 0 BEGIN
		IF (SELECT COUNT(*) FROM Subjects WHERE breadcrumb = @breadcrumb AND title=@title) > 0 BEGIN
			/* subject already exists */
			SET @create = 0
		END ELSE BEGIN
			/* get hierarchy indexes */
			SELECT @hierarchy = hierarchy FROM Subjects WHERE subjectId=@parentId
			if @hierarchy <> '' BEGIN
			 SET @hierarchy = @hierarchy  + '>' + CONVERT(nvarchar(10),@parentId)
			END ELSE BEGIN
			 SET @hierarchy =  CONVERT(nvarchar(10),@parentId)
			END
		END
	END ELSE BEGIN
		IF (SELECT COUNT(*) FROM Subjects WHERE parentId=0 AND title=@title) > 0 BEGIN
			/* root subject already exists */
			SET @create = 0
		END
	END

	IF @create = 1 BEGIN
		/* finally, create subject */
		DECLARE @id int = NEXT VALUE FOR SequenceSubjects
		INSERT INTO Subjects (subjectId, parentId, grammartype, score, title, breadcrumb, hierarchy)
		VALUES (@id, @parentId, @grammartype, @score, @title, @breadcrumb, @hierarchy)

		SELECT @id
	END ELSE BEGIN
		SELECT 0
	END

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Subject_GetById')
	DROP PROCEDURE [dbo].[Subject_GetById]
GO
CREATE PROCEDURE [dbo].[Subject_GetById]
	@subjectId int
AS
SELECT * FROM Subjects WHERE subjectId=@subjectId

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Subject_GetByTitle')
	DROP PROCEDURE [dbo].[Subject_GetByTitle]
GO
CREATE PROCEDURE [dbo].[Subject_GetByTitle]
	@title nvarchar(50),
	@breadcrumb nvarchar(MAX)
AS
SELECT * FROM Subjects WHERE breadcrumb = @breadcrumb AND title=@title

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Subject_Move')
	DROP PROCEDURE [dbo].[Subject_Move]
GO
CREATE PROCEDURE [dbo].[Subject_Move]
	@subjectId int = 1,
	@newParent int = 127
AS
	DECLARE 
	@title NVARCHAR(50) = '',
	@bread NVARCHAR(500) = '', 
	@hier NVARCHAR(50), 
	@newBread NVARCHAR(500) = '',
	@newHier NVARCHAR(50),
	@newTitle NVARCHAR(50),
	@cursor1 CURSOR,
	@childId INT, @parentId INT,
	@parentTitle NVARCHAR(50),
	@parentHier NVARCHAR(50),
	@parentBread NVARCHAR(500)

	/* get breadcrumb info */
	SELECT @bread = breadcrumb, @hier = hierarchy FROM Subjects WHERE subjectId=@subjectId
	IF @bread <> '' BEGIN
		SET @bread = @bread + '>' + @title
		SET @hier = @hier + '>' + CONVERT(NVARCHAR(25),@subjectId)
	END ELSE BEGIN
		SET @bread = @title
		SET @hier = CONVERT(NVARCHAR(25),@subjectId)
	END
	SELECT @newBread = breadcrumb, @newHier = hierarchy, @newTitle=title FROM Subjects WHERE subjectId=@newParent
	IF @newBread <> '' BEGIN
		SET @newBread = @newBread + '>' + @newTitle
		SET @newHier = @newHier + '>' + CONVERT(NVARCHAR(25),@newParent)
	END ELSE BEGIN
		SET @newBread = @newTitle
		SET @newHier = CONVERT(NVARCHAR(25),@newParent)
	END

	/* update subject */
	UPDATE Subjects 
	SET parentId=@newParent, hierarchy=@newHier, breadcrumb=@newBread 
	WHERE subjectId=@subjectId

	/* update each child subject */
	SET @cursor1 = CURSOR FOR
	SELECT subjectId, parentId FROM Subjects WHERE hierarchy LIKE @hier + '>%' OR hierarchy = @hier ORDER BY hierarchy ASC
	OPEN @cursor1
	FETCH FROM @cursor1 INTO
	@childId, @parentId
	WHILE @@FETCH_STATUS = 0
    BEGIN
		SELECT @parentTitle = title, @parentHier=hierarchy, @parentBread=breadcrumb FROM Subjects WHERE subjectId=@parentId
		IF @parentBread <> '' BEGIN
			SET @parentBread = @parentBread + '>' + @parentTitle
			SET @parentHier = @parentHier + '>' + CONVERT(NVARCHAR(25),@parentId)
		END ELSE BEGIN
			SET @parentBread = @parentTitle
			SET @parentHier = CONVERT(NVARCHAR(25),@parentId)
		END
		UPDATE Subjects SET hierarchy=@parentHier, breadcrumb=@parentBread WHERE subjectId=@childId

		FETCH FROM @cursor1 INTO
		@childId, @parentId
	END

	CLOSE @cursor1
	DEALLOCATE @cursor1

	

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Words_GetList')
	DROP PROCEDURE [dbo].[Words_GetList]
GO
CREATE PROCEDURE [dbo].[Words_GetList]
	@words nvarchar(MAX)
AS
SELECT * INTO #words FROM dbo.SplitArray(@words, ',')
SELECT w.*, sw.subjectId
FROM Words w
JOIN SubjectWords sw ON sw.wordId=w.wordId
WHERE word IN (SELECT value FROM #words)

/* ////////////////////////////////////////////////////////////////////////////////////// */

GO

/* ////////////////////////////////////////////////////////////////////////////////////// */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Word_Add')
	DROP PROCEDURE [dbo].[Word_Add]
GO
CREATE PROCEDURE [dbo].[Word_Add]
	@word nvarchar(50),
	@subjectId int = 0,
	@grammartype int = 0,
	@score int = 1
AS
	DECLARE @wordId int
	IF(SELECT COUNT(*) FROM Words WHERE word=@word AND grammartype=@grammartype) = 0 BEGIN
		/* word doesn't exists */
		SET @wordId = NEXT VALUE FOR SequenceWords
		INSERT INTO Words (wordId, word, grammartype, score) 
		VALUES (@wordId, @word, @grammartype, @score)
	END ELSE BEGIN
		SELECT @wordId = wordId FROM Words WHERE word=@word
	END

	IF @wordId IS NOT NULL BEGIN
		INSERT INTO SubjectWords (wordId, subjectId) VALUES (@wordId, @subjectId)
	END
	
	SELECT @wordId
