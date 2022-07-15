using System;
using System.Collections.Generic;
using System.Linq;

namespace Query
{
    public static class Articles
    {

        public enum SortBy
        {
            TitleAscending = 0,
            TitleDescending = 1,
            UrlAscending = 2,
            UrlDescending = 3,
            BestScore = 4,
            WorstScore = 5,
            Newest = 6,
            Oldest = 7,
            Popular = 8
        }

        public enum IsActive
        {
            notActive = 0,
            Active = 1,
            Both = 2
        }

        public static int Add(Models.Article article)
        {
            return Sql.ExecuteScalar<int>("Article_Add", new { 
                article.feedId,
                article.subjects,
                article.subjectId,
                article.score,
                article.domain,
                article.url,
                article.title,
                article.summary,
                article.filesize,
                article.linkcount,
                article.linkwordcount,
                article.wordcount,
                article.sentencecount,
                article.paragraphcount,
                article.importantcount,
                article.yearstart,
                article.yearend,
                article.years,
                article.images,
                article.datepublished,
                article.relavance,
                article.importance,
                article.fiction,
                article.analyzed,
                article.active 
            });
            
        }

        public static void Clean(int articleId)
        {
            Sql.ExecuteNonQuery("Article_Clean", new { articleId });
        }

        public static bool Exists(string url)
        {
            return Sql.ExecuteScalar<int>("Article_Exists", new { url }) > 0;
        }

        public static List<Models.ArticleDetails> GetList(int[] subjectId, int feedId = 0, int domainId = 0, int score = 0, string search = "", IsActive isActive = IsActive.Both, bool isDeleted = false, int minImages = 0, DateTime? dateStart = null, DateTime? dateEnd = null, SortBy orderBy = SortBy.BestScore, int start = 1, int length = 50, bool bugsOnly = false)
        {
            return Sql.Populate<Models.ArticleDetails>(
                "Articles_GetList",
                new
                {
                    subjectIds = subjectId.Length == 0 ? "" : string.Join(",", subjectId),
                    feedId,
                    domainId,
                    score,
                    search,
                    isActive = (int)isActive,
                    isDeleted,
                    minImages,
                    dateStart = dateStart,
                    dateEnd = dateEnd,
                    orderby = (int)orderBy,
                    start,
                    length,
                    bugsOnly
                });
        }

        public static int GetCount(int[] subjectId, int feedId = 0, int domainId = 0, int score = 0, string search = "", IsActive isActive = IsActive.Both, bool isDeleted = false, int minImages = 0, DateTime? dateStart = null, DateTime? dateEnd = null, bool bugsOnly = false)
        {
            return Sql.ExecuteScalar<int>(
                "Articles_GetCount",
                new
                {
                    subjectIds = subjectId.Length == 0 ? "" : string.Join(",", subjectId),
                    feedId,
                    domainId,
                    score,
                    search,
                    isActive = (int)isActive,
                    isDeleted,
                    minImages,
                    dateStart = dateStart,
                    dateEnd = dateEnd,
                    bugsOnly
                });
        }

        public static Models.Article GetByUrl(string url)
        {
            return Sql.Populate<Models.Article>("Article_GetByUrl", new { url }).FirstOrDefault();
        }

        public static Models.Article GetById(int articleId)
        {
            return Sql.Populate<Models.Article>("Article_GetById", new { articleId }).FirstOrDefault();
        }

        public static void Remove(int articleId)
        {
            Sql.ExecuteNonQuery("Article_Remove", new { articleId });
        }

        public static void Update(Models.Article article)
        {
            Sql.ExecuteNonQuery("Article_Update", new {
                article.articleId,
                article.subjects,
                article.subjectId,
                article.score,
                article.title,
                article.summary,
                article.filesize,
                article.wordcount,
                article.sentencecount,
                article.paragraphcount,
                article.importantcount,
                article.yearstart,
                article.yearend,
                article.years,
                article.images,
                article.datepublished,
                article.relavance,
                article.importance,
                article.fiction,
                article.analyzed 
                });
        }

        public static void UpdateUrl(int articleId, string url, string domain)
        {
            Sql.ExecuteNonQuery("Article_UpdateUrl", new
            {
                articleId, url, domain
            });
        }

        public static void UpdateCache(int articleId, bool cached)
        {
            Sql.ExecuteNonQuery("Article_UpdateCache", new { articleId, cached });
        }

        public static void Visited(int articleId)
        {
            Sql.ExecuteNonQuery("Article_Visited", new { articleId });
        }

        #region "Dates, sentences, subjects, words, etc"

        public static void AddDate(int articleId, DateTime date, bool hasYear, bool hasMonth, bool hasDay)
        {
            Sql.ExecuteNonQuery("ArticleDate_Add", new { articleId, date, hasYear, hasMonth, hasDay });
        }

        public static void AddSentence(int articleId, int index, string sentence)
        {
            Sql.ExecuteNonQuery("ArticleSentence_Add", new { articleId, index, sentence });
        }

        public static void RemoveSentences(int articleId)
        {
            Sql.ExecuteNonQuery("ArticleSentences_Remove", new { articleId });
        }

        public static void AddSubject(int articleId, int subjectId, DateTime? datePublished = null, int score = 0)
        {
            Sql.ExecuteNonQuery("ArticleSubject_Add", new { articleId, subjectId, datePublished, score });
        }

        public static void RemoveSubjects(int articleId, int subjectId = 0)
        {
            Sql.ExecuteNonQuery("ArticleSubjects_Remove", new { articleId, subjectId });
        }

        public static void AddWord(int articleId, int wordId, int count)
        {
            Sql.ExecuteNonQuery("ArticleWord_Add", new { articleId, wordId, count });
        }

        public static void RemoveWords(int articleId, string word = "")
        {
            Sql.ExecuteNonQuery("ArticleWords_Remove", new { articleId, word });
        }

        #endregion
    }
}
