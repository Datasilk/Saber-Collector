IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_Add')
	DROP PROCEDURE [dbo].[Domain_Add]
GO
CREATE PROCEDURE [dbo].[Domain_Add]
	@domain nvarchar(64),
	@title nvarchar(128) = '',
	@type int = 0 -- 0 = none, 1 = whitelist, 2 = blacklist
AS
	DECLARE @id int = NEXT VALUE FOR SequenceDomains
	INSERT INTO Domains (domainId, domain, title)
	VALUES (@id, @domain, @title)
	SELECT @id

	IF @type = 1 EXEC Whitelist_Domain_Add @domain=@domain
	IF @type = 2 EXEC Blacklist_Domain_Add @domain=@domain

	DECLARE @url nvarchar(MAX) = 'http://' + @domain
	EXEC DownloadQueue_Add @url=@url, @domain=@domain, @feedId=0