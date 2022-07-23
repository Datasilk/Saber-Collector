IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_CollectionGroup_Remove')
	DROP PROCEDURE [dbo].[Domain_CollectionGroup_Remove]
GO
CREATE PROCEDURE [dbo].[Domain_CollectionGroup_Remove]
	@colgroupId int = 0
AS
	DELETE FROM DomainCollectionGroups WHERE colgroupId=@colgroupId