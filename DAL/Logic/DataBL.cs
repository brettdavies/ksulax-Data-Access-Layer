using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KSULax.Entities;
using System.Data.Objects;
using KSULax.Dal;

namespace KSULax.Logic
{
    public class DataBL
    {
        private KSULaxEntities _entities;

        public DataBL(KSULaxEntities entitity) { _entities = entitity; }

        /// <summary>
        /// Returns the most recent Top 25 Ranking for the team.
        /// </summary>
        /// <returns></returns>
        public List<TeamRankingBE> GetRanking()
        {
            List<TeamRankingBE> ranking = new List<TeamRankingBE>(3);
            ranking.Add(RecentRankingbyPollID(1));
            ranking.Add(RecentRankingbyPollID(2));
            ranking.Add(RecentRankingbyPollID(3));

            return ranking;
        }

        /// <summary>
        /// Returns the rankings for an entire season for a single poll 
        /// </summary>
        /// <param name="pollID">poll to get rankings for</param>
        /// <param name="season">season to get rankings for</param>
        /// <returns></returns>
        public List<TeamRankingBE> GetRanking(int pollID, int season)
        {
            var rankings = (from ps in _entities.PollSet
                         where ps.date >= new DateTime(season,1,1)
                         && ps.date <= new DateTime(season, 12, 31)
                         && ps.pollsource_id == pollID
                         orderby ps.date
                         select ps) as ObjectQuery<PollEntity>;

            List<TeamRankingBE> ranking = new List<TeamRankingBE>();

            foreach (PollEntity pe in rankings)
            {
                ranking.Add(getEntity(pe, true));
            }

            return ranking;
        }

        public List<double> GetRankingDates(int season)
        {
            var rankings = ((from ps in _entities.PollSet
                             where ps.chart_date >= new DateTime(season, 1, 1)
                             && ps.chart_date <= new DateTime(season, 12, 31)
                             orderby ps.chart_date
                             select ps.chart_date).Distinct()) as ObjectQuery<DateTime>;

            List<double> ranking = new List<double>();

            foreach (DateTime dt in rankings)
            {
                ranking.Add(dt.ToOADate());
            }

            return ranking;
        }

        private TeamRankingBE RecentRankingbyPollID(int pollID)
        {
            var rank = ((from ps in _entities.PollSet
                           where ps.pollsource_id == pollID
                           orderby ps.date descending
                           select ps) as ObjectQuery<PollEntity>)
                           .Take(1)
                           .FirstOrDefault<PollEntity>();

            return getEntity(rank, false);
        }

        private TeamRankingBE getEntity(PollEntity pe, bool chart)
        {
            return new TeamRankingBE
            {
                Date = chart ? pe.chart_date : pe.date,
                Datestr = chart ? pe.chart_date.ToString("MMMM dd, yyyy") : pe.date.ToString("MMMM dd, yyyy"),
                pollSource = pe.pollsource_id,
                Rank = pe.rank,
                Url = pe.url
            };
        }
    }
}

namespace KSULax.Logic.Import
{
    public class DataBL
    {
        public void UpdatePoll(TeamRankingBE poll)
        {
            using (var ent = new KSULaxEntities())
            {
                var pe = ((from p in ent.PollSet
                           where p.pollsource_id == poll.pollSource
                           && p.date == poll.Date
                           select p) as ObjectQuery<PollEntity>)
                                .Take<PollEntity>(1)
                                .FirstOrDefault<PollEntity>();

                if (null == pe)
                {
                    ent.PollSet.AddObject(GetPollEntity(poll));
                }

                else
                {
                    pe.rank = poll.Rank;
                    pe.url = poll.Url;
                }

                ent.SaveChanges();
            }
        }

        private PollEntity GetPollEntity(TeamRankingBE mclaPoll)
        {
            var pe = new PollEntity
            {
                date = mclaPoll.Date,
                pollsource_id = mclaPoll.pollSource,
                rank = mclaPoll.Rank,
                url = mclaPoll.Url
            };

            return pe;
        }
    }
}