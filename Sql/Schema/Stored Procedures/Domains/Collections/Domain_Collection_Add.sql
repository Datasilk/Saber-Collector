IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_Collection_Add')
	DROP PROCEDURE [dbo].[Domain_Collection_Add]
GO
CREATE PROCEDURE [dbo].[Domain_Collection_Add]
	@colgroupId int = 0,
	@name nvarchar(32),
	@search nvarchar(128),
	@subjectId int = 0,
	@type int = 0,
	@sort int = 0
AS
	DECLARE @id int = NEXT VALUE FOR SequenceDomainCollections
	INSERT INTO DomainCollections (colId, colgroupId, [name], [search], subjectId, [type], [sort], datecreated)
	VALUES (@id, @colgroupId, @name, @search, @subjectId, @type, @sort, GETUTCDATE())
	SELECT @id