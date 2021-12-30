BEGIN TRY
	CREATE INDEX [IndexStatisticsResultsDate]
		ON [dbo].StatisticsResults ([year] ASC, [month] ASC, [day] ASC)
END TRY BEGIN CATCH END CATCH