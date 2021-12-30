BEGIN TRY
	CREATE INDEX [IndexStatisticsResults]
		ON [dbo].StatisticsResults (projectId ASC, statId ASC)
END TRY BEGIN CATCH END CATCH