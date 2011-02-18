using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KSULax.Entities;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using KSULax.Dal;

namespace KSULax.Logic
{
    public class PlayerBL
    {
        private KSULaxEntities _entities;

        public PlayerBL(KSULaxEntities entitity) { _entities = entitity; }

        public bool getPlayerNamebyID(int player_id, out string name)
        {
            var result = (from p in _entities.PlayerSet
                           where p.id == player_id
                           select (p.first + "-" + p.last))
                         .Take<string>(1)
                         .FirstOrDefault<string>()
                         .ToLower();

            if (!string.IsNullOrEmpty(result))
            {
                name = result;
                return true;
            }
            else
            {
                name = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// Returns a list of PlayerBE objects for a certain season
        /// </summary>
        /// <param name="seasonID">Season to find players for</param>
        /// <returns></returns>
        public List<PlayerBE> PlayersBySeason(int seasonID)
        {
            var playerseason = ((from ps in _entities.PlayerSeasonSet
                                where ps.season_id == seasonID
                                orderby ps.jersey
                                select ps) as ObjectQuery<PlayerSeasonEntity>)
                                .Include("Player");

            var result = new List<PlayerBE>();

            foreach (PlayerSeasonEntity pse in playerseason)
            {
                result.Add(GetEntity(pse));
            }

            return result;
        }

        /// <summary>
        /// Takes a player id and information about that player during the specified season
        /// </summary>
        /// <param name="playerID">Player ID to get information about</param>
        /// <param name="seasonID">Season ID to get information about. Optional, if not specified then most recent season is returned.</param>
        /// <returns></returns>
        public PlayerBE PlayerByID(int playerID, short seasonID = -1)
        {
            var player = ((from p in _entities.PlayerSet
                           where p.id == playerID
                           select p) as ObjectQuery<PlayerEntity>)
                        .Include("PlayerSeason")
                        .Take<PlayerEntity>(1)
                        .FirstOrDefault<PlayerEntity>();

            return GetEntity(player, seasonID);
        }

        /// <summary>
        /// Takes a player name and returns current season information about that player
        /// </summary>
        /// <param name="name">Player name to get information about</param>
        /// <returns></returns>
        public PlayerBE PlayerByName(string name)
        {
            string first = string.Empty;
            string last = string.Empty;

            try
            {
                first = name.Substring(0, name.IndexOf("-"));
                last = name.Substring(name.IndexOf("-") + 1);

                var player = ((from p in _entities.PlayerSet
                            where p.first == first && p.last == last
                            select p) as ObjectQuery<PlayerEntity>)
                            .Include("PlayerSeason")
                            .Take<PlayerEntity>(1)
                            .FirstOrDefault<PlayerEntity>();

                return GetEntity(player);
            }

            catch (Exception)
            {
                throw new Exception("KSULAX||we can't find the player you requested");
            }
        }

        private PlayerBE GetEntity(PlayerEntity pe, short seasonID = -1)
        {
            if (null == pe)
            {
                return null;
            }

            if (seasonID.Equals(-1))
            {
                seasonID = pe.PlayerSeason.Max<PlayerSeasonEntity, short>(x => x.season_id);
            }

            PlayerSeasonEntity pse = pe.PlayerSeason.Single(x => x.season_id.Equals(seasonID));

            var result = new PlayerBE
            {
                FirstName = pe.first,
                HighSchool = pe.highschool,
                HomeState = pe.homestate,
                Hometown = pe.hometown,
                LastName = pe.last,
                Major = pe.major,
                MiddleName = pe.middle,
                PlayerID = pe.id
            };

            bool seasonExists = (null == pse);

            result.Bio = (seasonExists || string.IsNullOrEmpty(pse.bio)) ? "no bio provided" : pse.bio;
            result.Captain = seasonExists ? false : pse.captain.GetValueOrDefault();
            result.ClassYr = seasonExists ? string.Empty : pse.@class;
            result.EligibilityYr = seasonExists ? string.Empty : pse.eligibility;
            result.Height = seasonExists ? 0 : pse.height.GetValueOrDefault();
            result.JerseyNum = seasonExists ? -1 : pse.jersey.GetValueOrDefault();
            result.Officer = seasonExists ? false : pse.officer.GetValueOrDefault();
            result.Position = seasonExists ? string.Empty : pse.position;
            result.President = seasonExists ? false : pse.president.GetValueOrDefault();
            result.SeasonID = seasonExists ? 0 : pse.season_id;
            result.Weight = seasonExists ? 0 : pse.weight.GetValueOrDefault();            
            
            return result;
        }

        private PlayerBE GetEntity(PlayerSeasonEntity pse)
        {
            if (null == pse)
            {
                return null;
            }

            var result = new PlayerBE
            {
                Bio = string.IsNullOrEmpty(pse.bio) ? "no bio provided" : pse.bio,
                Captain = pse.captain.GetValueOrDefault(),
                ClassYr = pse.@class,
                EligibilityYr = pse.eligibility,
                FirstName = pse.Player.first,
                Height = pse.height.GetValueOrDefault(),
                HighSchool = pse.Player.highschool,
                HomeState = pse.Player.homestate,
                Hometown = pse.Player.hometown,
                JerseyNum = pse.jersey.GetValueOrDefault(),
                LastName = pse.Player.last,
                Major = pse.Player.major,
                MiddleName = pse.Player.middle,
                Officer = pse.officer.GetValueOrDefault(),
                PlayerID = pse.Player.id,
                Position = pse.position,
                President = pse.president.GetValueOrDefault(),
                SeasonID = pse.season_id,
                Weight = pse.weight.GetValueOrDefault()                
            };
            return result;
        }
    }
}