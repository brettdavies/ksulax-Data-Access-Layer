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

        private PlayerBE GetEntity(PlayerEntity pe)
        {
            if (null == pe)
            {
                return null;
            }

            short MaxSeason = pe.PlayerSeason.Max<PlayerSeasonEntity, short>(x => x.season_id);
            PlayerSeasonEntity pse = pe.PlayerSeason.Single(x => x.season_id.Equals(MaxSeason));

            var result = new PlayerBE
            {
                Bio = string.IsNullOrEmpty(pse.bio) ? "no bio provided" : pse.bio,
                Captain = pse.captain.GetValueOrDefault(),
                ClassYr = pse.@class,
                EligibilityYr = pse.eligibility,
                FirstName = pe.first,
                Height = pse.height.GetValueOrDefault(),
                HighSchool = pe.highschool,
                HomeState = pe.homestate,
                Hometown = pe.hometown,
                JerseyNum = pse.jersey.GetValueOrDefault(),
                LastName = pe.last,
                Major = pe.major,
                MiddleName = pe.middle,
                Officer = pse.officer.GetValueOrDefault(),
                PlayerID = pe.id,
                Position = pse.position,
                President = pse.president.GetValueOrDefault(),
                SeasonID = pse.season_id,
                Weight = pse.weight.GetValueOrDefault()
            };
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