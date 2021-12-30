BEGIN TRY
	CREATE TABLE [dbo].[StatisticsProjectResearchers]
	(
		[projectId] INT NOT NULL PRIMARY KEY, 
		[researcherId] INT NULL
	)
END TRY BEGIN CATCH END CATCH