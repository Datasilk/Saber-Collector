-- delete all downloads that do not have a valid URL
DELETE FROM DownloadQueue
WHERE qid IN
(
	SELECT q.qid FROM DownloadQueue q
	JOIN Domains d ON d.domainId = q.domainId
	WHERE LEN(q.url) <= LEN(d.domain) + 9
	AND q.qid NOT IN (
		SELECT q2.qid FROM DownloadQueue q2
		JOIN Domains d2 ON d2.domainId = q2.domainId
		WHERE LEN(q2.url) <= LEN(d2.domain) + 9
		AND q2.url LIKE 'http%://%.%'
	)
)

SELECT * FROM DownloadQueue q
JOIN Domains d ON d.domainId = q.domainId
WHERE LEN(q.url) <= LEN(d.domain) + 9
AND q.qid NOT IN (
	SELECT q2.qid FROM DownloadQueue q2
	JOIN Domains d2 ON d2.domainId = q2.domainId
	WHERE LEN(q2.url) <= LEN(d2.domain) + 9
	AND q2.url LIKE 'http%://%.%'
)



INSERT INTO DownloadQueue
SELECT q.id AS qid, q.feedId, q.domainId, q.status, q.tries, q.url, q.path, q.datecreated 
FROM Downloads q
JOIN Domains d ON d.domainId = q.domainId
WHERE LEN(q.url) <= LEN(d.domain) + 9

DELETE FROM Downloads WHERE id IN (
	SELECT q.id FROM Downloads q
	JOIN Domains d ON d.domainId = q.domainId
	WHERE LEN(q.url) <= LEN(d.domain) + 9
)

UPDATE DownloadQueue SET status=0
WHERE qid IN (
SELECT qid FROM DownloadQueue q
JOIN Domains d ON d.domainId = q.domainId
WHERE LEN(q.url) <= LEN(d.domain) + 9)

SELECT * FROM DownloadQueue q
JOIN Domains d ON d.domainId = q.domainId
WHERE LEN(q.url) <= LEN(d.domain) + 9