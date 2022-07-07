BEGIN TRY
    CREATE TABLE [dbo].[DownloadRules]
    (
	    [ruleId] INT NOT NULL PRIMARY KEY, 
	    [domainId] INT NOT NULL, 
        [url] VARCHAR(64) NOT NULL DEFAULT '',
        [title] VARCHAR(64) NOT NULL DEFAULT '',
        [summary] VARCHAR(64) NOT NULL DEFAULT '',
        [datecreated] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    )
END TRY BEGIN CATCH END CATCH
    GO