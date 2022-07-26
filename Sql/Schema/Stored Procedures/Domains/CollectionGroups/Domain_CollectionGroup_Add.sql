IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_CollectionGroup_Add')
	DROP PROCEDURE [dbo].[Domain_CollectionGroup_Add]
GO
CREATE PROCEDURE [dbo].[Domain_CollectionGroup_Add]
	@name nvarchar(32)
AS
	DECLARE @id int = NEXT VALUE FOR SequenceDomainCollectionGroups
	INSERT INTO DomainCollectionGroups (colgroupId, [name])
	VALUES (@id, @name)
	SELECT @id