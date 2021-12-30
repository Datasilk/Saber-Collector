BEGIN TRY
	CREATE INDEX [IndexStatisticsResultsLocation]
		ON [dbo].StatisticsResults (country ASC, [state] ASC, city ASC)
END TRY BEGIN CATCH END CATCH