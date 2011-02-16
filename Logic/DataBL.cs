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
            ranking[0].datestr = ranking[0].date.ToString("MMMM dd, yyyy");
            ranking.Add(RecentRankingbyPollID(2));
            ranking[1].datestr = ranking[1].date.ToString("MMMM dd, yyyy");
            ranking.Add(RecentRankingbyPollID(3));
            ranking[2].datestr = ranking[2].date.ToString("MMMM dd, yyyy");

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
                date = award.FirstOrDefault<PollEntity>().date,
                pollSource = award.FirstOrDefault<PollEntity>().pollsource_id,
                rank = award.FirstOrDefault<PollEntity>().rank,
                rankUrl = award.FirstOrDefault<PollEntity>().url
            };
        }
    }
}