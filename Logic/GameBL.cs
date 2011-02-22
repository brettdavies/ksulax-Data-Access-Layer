using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KSULax.Entities;
using System.Data.Objects;
using KSULax.Interfaces;
using KSULax.Dal;

namespace KSULax.Logic
{
    public class GameBL
    {
        private KSULaxEntities _entities;

        public GameBL(KSULaxEntities entitity) { _entities = entitity; }

        public INews GameDetail(int gameID)
        {
            var result = ((from gs in _entities.GameSet
                           where gs.id == gameID
                           select gs) as ObjectQuery<GameEntity>)
                          .Include("AwayTeam")
                          .Include("HomeTeam")
                          .Take(1);

            if (null != result && result.Count<GameEntity>().Equals(1))
            {
                return (INews)GetEntity(result.FirstOrDefault<GameEntity>());
            }

            else
            {
                return null;
            }
        }

        public List<GameBE> GamesBySeason(int seasonID)
        {
            var games = ((from gs in _entities.GameSet
                          where gs.game_season_id == seasonID
                          orderby gs.game_date
                          select gs) as ObjectQuery<GameEntity>)
                          .Include("AwayTeam")
                          .Include("HomeTeam");

            var result = new List<GameBE>();

            foreach (GameEntity ge in games)
            {
                result.Add(GetEntity(ge));
            }

            return result;
        }

        public List<GameBE> GameScheduleBySeason(int seasonID)
        {
            var games = ((from gs in _entities.GameSet
                          where gs.game_season_id == seasonID
                          orderby gs.game_date
                          select gs) as ObjectQuery<GameEntity>)
                          .Include("HomeTeam")
                          .Include("AwayTeam");

            var result = new List<GameBE>();

            foreach (GameEntity ge in games)
            {
                result.Add(GetEntity(ge));
            }

            return result;
        }

        public List<GameBE> GameSummary(int numGames)
        {
            var games = ((from gs in _entities.GameSet
                          where gs.game_date <= DateTime.Now
                          && null != gs.detail
                          orderby gs.game_date descending
                          select gs) as ObjectQuery<GameEntity>)
                          .Include("AwayTeam")
                          .Include("HomeTeam")
                          .Take(numGames);

            var result = new List<GameBE>();

            foreach (GameEntity ge in games)
            {
                result.Add(GetEntity(ge));
            }

            return result;
        }

        public List<GameBE> GameSummaryYear(DateTime date)
        {
            DateTime date2 = date.AddYears(1);
            var games = ((from gs in _entities.GameSet
                          where null != gs.detail
                          && gs.game_date >= date
                          && gs.game_date <= date2
                          && gs.game_date <= DateTime.Now
                          orderby gs.game_date descending
                          select gs) as ObjectQuery<GameEntity>)
                          .Include("AwayTeam")
                          .Include("HomeTeam");

            var result = new List<GameBE>();

            foreach (GameEntity ge in games)
            {
                result.Add(GetEntity(ge));
            }

            return result;
        }

        public List<GameBE> GameSummaryYearMonth(DateTime date)
        {
            DateTime date2 = date.AddMonths(1);
            var games = ((from gs in _entities.GameSet
                          where null != gs.detail
                          && gs.game_date >= date
                          && gs.game_date <= date2
                          && gs.game_date <= DateTime.Now
                          orderby gs.game_date descending
                          select gs) as ObjectQuery<GameEntity>)
                         .Include("AwayTeam")
                         .Include("HomeTeam");

            var result = new List<GameBE>();

            foreach (GameEntity ge in games)
            {
                result.Add(GetEntity(ge));
            }

            return result;
        }

        public List<GameBE> GameSummaryYearMonthDay(DateTime date)
        {
            DateTime date2 = date.AddDays(1);
            var games = ((from gs in _entities.GameSet
                          where null != gs.detail
                          && gs.game_date >= date
                          && gs.game_date <= date2
                          && gs.game_date <= DateTime.Now
                          orderby gs.game_date descending
                          select gs) as ObjectQuery<GameEntity>)
                          .Include("AwayTeam")
                          .Include("HomeTeam");

            var result = new List<GameBE>();

            foreach (GameEntity ge in games)
            {
                result.Add(GetEntity(ge));
            }

            return result;
        }

        /// <summary>
        /// Takes a list of GameBE objects and returns a INews list objects
        /// </summary>
        /// <param name="gameslist">List of games</param>
        /// <returns></returns>
        public List<INews> GameBriefList(List<GameBE> gameslist)
        {
            var newslist = new List<INews>();
            foreach (GameBE game in gameslist)
            {
                newslist.Add((INews)game);
            }
            return newslist;
        }

        /// <summary>
        /// Returns a list of gamestats for official MCLA games that are not scrimages and have a game status of 0.
        /// </summary>
        /// <param name="gameID">Optional GameID</param>
        /// <param name="seasonID">Optional SeasonID</param>
        /// <param name="playerID">Optional PlayerID</param>
        /// <returns></returns>
        public List<GameStatBE> PlayerGameStats(int gameID = -1, int seasonID = -1, int playerID = -1)
        {
            if (gameID.Equals(-1) && seasonID.Equals(-1) && playerID.Equals(-1))
            {
                return null;
            }

            ObjectQuery<PlayerGameEntity> stats = null;

            if (!gameID.Equals(-1) && seasonID.Equals(-1) && playerID.Equals(-1))
            {
                stats = ((from p in _entities.PlayerGameSet
                         where p.game_id == gameID
                         && p.Game.game_type != "scrimage"
                         && p.Game.game_status == "0"
                         select p) as ObjectQuery<PlayerGameEntity>)
                         .Include("Game");
            }

            if (gameID.Equals(-1) && !seasonID.Equals(-1) && playerID.Equals(-1))
            {
                stats = ((from p in _entities.PlayerGameSet
                         where p.Game.game_season_id == seasonID
                         && p.Game.game_type != "scrimage"
                         && p.Game.game_status == "0"
                         select p) as ObjectQuery<PlayerGameEntity>)
                         .Include("Game");
            }

            if (gameID.Equals(-1) && seasonID.Equals(-1) && !playerID.Equals(-1))
            {
                stats = ((from p in _entities.PlayerGameSet
                         where p.player_id == playerID
                         && p.Game.game_type != "scrimage"
                         && p.Game.game_status == "0"
                         select p) as ObjectQuery<PlayerGameEntity>)
                         .Include("Game");
            }

            else if (!gameID.Equals(-1) && !seasonID.Equals(-1) && playerID.Equals(-1))
            {
                stats = ((from p in _entities.PlayerGameSet
                         where p.game_id == gameID
                         && p.Game.game_season_id == seasonID
                         && p.Game.game_type != "scrimage"
                         && p.Game.game_status == "0"
                         select p) as ObjectQuery<PlayerGameEntity>)
                         .Include("Game");
            }

            else if (!gameID.Equals(-1) && seasonID.Equals(-1) && !playerID.Equals(-1))
            {
                stats = ((from p in _entities.PlayerGameSet
                         where p.game_id == gameID
                         && p.player_id == playerID
                         && p.Game.game_type != "scrimage"
                         && p.Game.game_status == "0"
                         select p) as ObjectQuery<PlayerGameEntity>)
                         .Include("Game");
            }

            else if (gameID.Equals(-1) && !seasonID.Equals(-1) && !playerID.Equals(-1))
            {
                stats = ((from p in _entities.PlayerGameSet
                         where p.Game.game_season_id == seasonID
                         && p.player_id == playerID
                         && p.Game.game_type != "scrimage"
                         && p.Game.game_status == "0"
                         select p) as ObjectQuery<PlayerGameEntity>)
                         .Include("Game");
            }

            else if (!gameID.Equals(-1) && !seasonID.Equals(-1) && !playerID.Equals(-1))
            {
                stats = ((from p in _entities.PlayerGameSet
                         where p.game_id == gameID
                         && p.Game.game_season_id == seasonID
                         && p.player_id == playerID
                         && p.Game.game_type != "scrimage"
                         && p.Game.game_status == "0"
                         select p) as ObjectQuery<PlayerGameEntity>)
                         .Include("Game");
            }

            var result = new List<GameStatBE>();

            foreach (PlayerGameEntity pge in stats)
            {
                result.Add(GetEntity(pge));
            }

            return result;
        }

        private GameStatBE GetEntity(PlayerGameEntity pge)
        {
            if (null == pge)
            {
                return null;
            }

            var result = new GameStatBE
            {
                Assists = pge.assists,
                GameID = pge.game_id,
                Goals = pge.goals,
                GoalsAgainst = pge.ga,
                PlayerID = pge.player_id,
                Saves = pge.saves,
                SeasonID = pge.Game.game_season_id
            };
            return result;
        }

        private GameBE GetEntity(GameEntity ge)
        {
            if (null == ge)
            {
                return null;
            }

            var result = new GameBE
            {
                AwayTeam = GetEntity(ge.AwayTeam),
                AwayTeamScore = ge.away_team_score.HasValue ? ge.away_team_score.Value : -1,
                AwayTeamSlug = ge.away_team_slug,
                Date = ge.game_date,
                Detail = string.IsNullOrEmpty(ge.detail) ? string.Empty : ge.detail,
                HomeTeam = GetEntity(ge.HomeTeam),
                HomeTeamScore = ge.home_team_score.HasValue ? ge.home_team_score.Value : -1,
                HomeTeamSlug = ge.home_team_slug,
                ID = ge.id,
                isHome = ge.home_team_slug.Equals("kennesaw_state") ? true : false,
                SeasonID = ge.game_season_id,
                Status = ge.game_status,
                Time = ge.game_time,
                Type = ge.game_type,
                Venue = ge.venue
            };
            return result;
        }

        private TeamBE GetEntity(TeamEntity te)
        {
            if (null == te)
            {
                return null;
            }

            var result = new TeamBE
            {
                Abr = te.abr,
                ID = te.id,
                Mascot = te.mascot,
                Name = te.name,
                Slug = te.slug,
                TeamURL = te.team_url
            };
            return result;
        }
    }
}