BEGIN TRY
    CREATE TABLE [dbo].[DomainTypeMatches]
    (
	    [matchId] INT NOT NULL PRIMARY KEY, 
        [type] INT NOT NULL DEFAULT -1,
        [type2] INT NOT NULL DEFAULT -1, 
        [words] NVARCHAR(MAX), --comma-delimited list of words to match on the homepage
        [threshold] INT NOT NULL DEFAULT 1 -- minimum number of matches that must be found to succeed
    )
END TRY BEGIN CATCH END CATCH
    GO