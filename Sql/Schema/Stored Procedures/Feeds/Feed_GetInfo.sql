IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feed_GetInfo')
	DROP PROCEDURE [dbo].[Feed_GetInfo]
GO
CREATE PROCEDURE [dbo].[Feed_GetInfo]
	@feedId int
AS
SELECT * FROM Feeds WHERE feedId=@feedId
