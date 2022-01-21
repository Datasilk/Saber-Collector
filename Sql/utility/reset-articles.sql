DELETE FROM ArticleSubjects
DELETE FROM ArticleSentences
DELETE FROM ArticleWords
DELETE FROM ArticleBugs
DELETE FROM ArticleDates
DELETE FROM Articles
DELETE FROM DownloadDomains
DELETE FROM DownloadQueue
DELETE FROM FeedsCheckedLog
--DELETE FROM Feeds
UPDATE Feeds SET lastChecked = DATEADD(HOUR, -24, GETUTCDATE())

SELECT * FROM DownloadQueue
SELECT * FROM DownloadDomains
SELECT * FROM Feeds
SELECT * FROM FeedsCheckedLog