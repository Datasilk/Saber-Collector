IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'DomainLink_Add')
	DROP PROCEDURE [dbo].[DomainLink_Add]
GO
CREATE PROCEDURE [dbo].[DomainLink_Add]
	@domainId int,
	@linkId int
AS
	BEGIN TRY
		INSERT INTO DomainLinks (domainId, linkId) VALUES (@domainId, @linkId)
	END TRY BEGIN CATCH END CATCH