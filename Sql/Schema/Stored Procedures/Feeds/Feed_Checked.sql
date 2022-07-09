IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Feed_Checked')
	DROP PROCEDURE [dbo].[Feed_Checked]
GO
CREATE PROCEDURE [dbo].[Feed_Checked]
	@feedId int = 0
AS
	UPDATE Feeds SET lastChecked=GETUTCDATE() WHERE feedId=@feedId
RETURN 0
