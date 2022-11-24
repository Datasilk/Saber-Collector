IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DomainTypeMatches_GetList')
	DROP PROCEDURE [dbo].[DomainTypeMatches_GetList]
GO
CREATE PROCEDURE [dbo].[DomainTypeMatches_GetList]
AS
	SELECT * FROM DomainTypeMatches
