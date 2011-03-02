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

        private TeamRankingBE RecentRankingbyPollID(int pollID)
        {
            var award = ((from ps in _entities.PollSet
                           where ps.pollsource_id == pollID
                           orderby ps.date descending
                           select ps) as ObjectQuery<PollEntity>)
                           .Take(1);

            return new TeamRankingBE
            {
                Date = award.FirstOrDefault<PollEntity>().date,
                Datestr = award.FirstOrDefault<PollEntity>().date.ToString("MMMM dd, yyyy"),
                pollSource = award.FirstOrDefault<PollEntity>().pollsource_id,
                Rank = award.FirstOrDefault<PollEntity>().rank,
                Url = award.FirstOrDefault<PollEntity>().url
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