BEGIN TRY
	CREATE INDEX [IndexStatisticsResultsProject]
		ON [dbo].StatisticsResults (projectId ASC)
END TRY BEGIN CATCH END CATCH