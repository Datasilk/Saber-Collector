IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feed_Add')
	DROP PROCEDURE [dbo].[Feed_Add]
GO
CREATE PROCEDURE [dbo].[Feed_Add]
	@doctype int = 1,
	@categoryId int,
	@title nvarchar(100) = '',
	@url nvarchar(100) = '',
	@domain nvarchar(64) = '',
	@filter nvarchar(MAX) = '',
	@checkIntervals int = 720 --(12 hours)
AS
	DECLARE @feedId int = NEXT VALUE FOR SequenceFeeds
	INSERT INTO Feeds (feedId, doctype, categoryId, title, url, checkIntervals, filter, lastChecked) 
	VALUES (@feedId, @doctype, @categoryId, @title, @url, @checkIntervals, @filter, DATEADD(HOUR, -24, GETUTCDATE()))
	
	BEGIN TRY
		INSERT INTO Whitelist_Domains (domain) VALUES (@domain)
	END TRY
	BEGIN CATCH
	END CATCH
	
	SELECT @feedId

