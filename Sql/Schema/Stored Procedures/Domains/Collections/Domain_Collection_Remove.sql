IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Domain_Collection_Remove')
	DROP PROCEDURE [dbo].[Domain_Collection_Remove]
GO
CREATE PROCEDURE [dbo].[Domain_Collection_Remove]
	@colId int = 0
AS
	DELETE FROM DomainCollections WHERE colId=@colId