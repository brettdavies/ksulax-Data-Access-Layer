using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using KSULax.Dal;
using KSULax.Entities;
using KSULax.Interfaces;

namespace KSULax.Logic
{
    public class NewsBL
    {
        private KSULaxEntities _entities;
        private GameBL _gameBL;

        public NewsBL(KSULaxEntities entitity)
        {
            _entities = entitity;
            _gameBL = new GameBL(entitity);
        }

        /// <summary>
        /// Gets a story based on date and title
        /// </summary>
        /// <param name="storyDate">date the story was published</param>
        /// <param name="urlTitle">url formatted title of the story</param>
        /// <returns></returns>
        public NewsBE GetStory(DateTime storyDate, string urlTitle)
        {
            var result = ((from n in _entities.NewsSet
                           where n.date == storyDate
                           && n.date <= DateTime.Now
                           && n.url_title == urlTitle
                           orderby n.date descending
                           select n) as ObjectQuery<NewsEntity>)
                           .Take<NewsEntity>(1)
                           .FirstOrDefault<NewsEntity>();

            return GetEntity(result);
        }

        /// <summary>
        /// Gets a list of news articles
        /// </summary>
        /// <param name="numStories">number of stories to return</param>
        /// <returns></returns>
        public List<INews> NewsList(int numStories)
        {
            var news = ((from n in _entities.NewsSet
                         where n.date <= DateTime.Now
                         orderby n.date descending
                         select n) as ObjectQuery<NewsEntity>)
                         .Take(numStories);

            var result = new List<INews>();

            foreach (NewsEntity newsE in news)
            {
                result.Add((INews)GetEntity(newsE));
            }

            result.AddRange(_gameBL.GameSummary(numStories).Cast<INews>());

            result.Sort((x, y) => DateTime.Compare(y.Date, x.Date));

            return result.GetRange(0, (result.Count < numStories) ? result.Count : numStories);
        }

        /// <summary>
        /// Gets a list of news articles for a certain year
        /// </summary>
        /// <param name="date">year from which to get news stories</param>
        /// <returns></returns>
        public List<INews> NewsYear(DateTime date)
        {
            DateTime date2 = date.AddYears(1);
            var news = (from n in _entities.NewsSet
                         where n.date >= date
                         && n.date <= date2
                         && n.date <= DateTime.Now
                         orderby n.date descending
                         select n) as ObjectQuery<NewsEntity>;

            var result = new List<INews>();

            foreach (NewsEntity newsE in news)
            {
                result.Add((INews)GetEntity(newsE));
            }

            result.AddRange(_gameBL.GameBriefList(_gameBL.GameSummaryYear(date)));
            
            result.Sort((x, y) => DateTime.Compare(y.Date, x.Date));

            return result;
        }

        /// <summary>
        /// Gets a list of news articles for a certain month from a certain year
        /// </summary>
        /// <param name="date">month and year from which to get news stories</param>
        /// <returns></returns>
        public List<INews> NewsYearMonth(DateTime date)
        {
            DateTime date2 = date.AddMonths(1);
            var news = (from n in _entities.NewsSet
                        where n.date >= date
                        && n.date <= date2
                        && n.date <= DateTime.Now
                        orderby n.date descending
                        select n) as ObjectQuery<NewsEntity>;

            var result = new List<INews>();

            foreach (NewsEntity newsE in news)
            {
                result.Add((INews)GetEntity(newsE));
            }

            result.AddRange(_gameBL.GameBriefList(_gameBL.GameSummaryYearMonth(date)));

            result.Sort((x, y) => DateTime.Compare(y.Date, x.Date));

            return result;
        }

        /// <summary>
        /// Gets a list of news articles for a certain complete date
        /// </summary>
        /// <param name="date">complete date from which to get news stories</param>
        /// <returns></returns>
        public List<INews> NewsYearMonthDay(DateTime date)
        {
            DateTime date2 = date.AddDays(1);
            var news = (from n in _entities.NewsSet
                        where n.date >= date
                        && n.date <= date2
                        && n.date <= DateTime.Now
                        orderby n.date descending
                        select n) as ObjectQuery<NewsEntity>;

            var result = new List<INews>();

            foreach (NewsEntity newsE in news)
            {
                result.Add((INews)GetEntity(newsE));
            }

            result.AddRange(_gameBL.GameBriefList(_gameBL.GameSummaryYearMonthDay(date)));

            result.Sort((x, y) => DateTime.Compare(y.Date, x.Date));

            return result;
        }

        /// <summary>
        /// Converts a NewsEntity object to a NewsBE object
        /// </summary>
        /// <param name="news">NewsEntity to convert</param>
        /// <returns></returns>
        protected NewsBE GetEntity(NewsEntity news)
        {
            if (null == news)
            {
                return null;
            }

            var result = new NewsBE
            {
                Author = news.author,
                Date = news.date,
                Source = news.source,
                SourceUrl = news.source_url,
                Story = news.story,
                Title = news.title,
                TitlePath = news.url_title
            };
            return result;
        }
    }
}