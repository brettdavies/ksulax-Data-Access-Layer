using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSULax.Logic;
using System.Data.Objects;
using KSULax.Dal;
using KSULax.Entities;

namespace KSULax.Logic.Import
{
    public class MCLA
    {
        public void UpdatePlayers(List<PlayerBE> players)
        {
            using (var ent = new KSULaxEntities())
            {
                foreach (PlayerBE pbe in players)
                {
                    var player = ((from p in ent.PlayerSet
                                   where p.id == pbe.PlayerID
                                   select p) as ObjectQuery<PlayerEntity>)
                                .Include("PlayerSeason")
                                .Take<PlayerEntity>(1)
                                .FirstOrDefault<PlayerEntity>();

                    if (null == player)
                    {
                        ent.PlayerSet.AddObject(GetPlayerEntity(pbe));
                    }

                    else
                    {
                        player.first = pbe.FirstName.Trim();
                        player.highschool = pbe.HighSchool.Trim();
                        player.homestate = pbe.HomeState.Trim();
                        player.hometown = pbe.Hometown.Trim();
                        player.last = pbe.LastName.Trim();
                        player.major = pbe.Major.Trim();
                        player.middle = pbe.MiddleName.Trim();

                        var playerseason = player.PlayerSeason.Single(x => x.season_id.Equals((short)pbe.SeasonID));

                        if (null != playerseason)
                        {
                            playerseason.@class = pbe.ClassYr.Trim();
                            playerseason.eligibility = pbe.EligibilityYr.Trim();
                            playerseason.height = (short)pbe.Height;
                            playerseason.jersey = (short)pbe.JerseyNum;
                            playerseason.position = pbe.Position.Trim();
                            playerseason.weight = (short)pbe.Weight;
                        }

                        player.PlayerSeason.Add(playerseason);
                    }
                }

                ent.SaveChanges();
            }
        }

        public void UpdateGames(List<GameBE> games)
        {
            using (var ent = new KSULaxEntities())
            {
                foreach (GameBE gbe in games)
                {
                    var game = ((from g in ent.GameSet
                                   where g.id == gbe.ID
                                   select g) as ObjectQuery<GameEntity>)
                                .Take<GameEntity>(1)
                                .FirstOrDefault<GameEntity>();

                    if (null == game)
                    {
                        ent.GameSet.AddObject(GetGameEntity(gbe));
                    }

                    else
                    {
                        game.away_team_score = gbe.AwayTeamScore;
                        game.away_team_slug = gbe.AwayTeamSlug.Trim();
                        game.game_datetime = gbe.Datetime;
                        game.game_season_id = (short)gbe.SeasonID;
                        game.game_status = gbe.Status.Trim();
                        game.game_type = gbe.Type.Trim();
                        game.home_team_score = gbe.HomeTeamScore;
                        game.home_team_slug = gbe.HomeTeamSlug.Trim();
                        game.id = (short)gbe.ID;
                        game.venue = gbe.Venue.Trim();
                    }
                }

                ent.SaveChanges();
            }
        }

        /// <summary>
        /// Updates the game stats per game. Only use to update stats for entire games. All stats for included games are cleared out prior to update.
        /// </summary>
        /// <param name="gsbeLst">List of Game Stats to Update</param>
        public void UpdateGameStatByGame(List<GameStatBE> gsbeLst)
        {
            using (var ent = new KSULaxEntities())
            {
                var games =
                    from n in gsbeLst
                    group n by n.GameID into nGroup
                    select nGroup.Key;

                foreach (var game in games)
                {
                    var pge = ((from g in ent.GameSet
                                where g.id == (short)game
                                select g) as ObjectQuery<GameEntity>)
                                .Include("PlayerGames")
                                .Take<GameEntity>(1)
                                .FirstOrDefault<GameEntity>();

                    pge.PlayerGames.Clear();
                }

                ent.SaveChanges();

                foreach (GameStatBE gsbe in gsbeLst)
                {
                    var pge = ((from pg in ent.PlayerGameSet
                                where pg.game_id == gsbe.GameID
                                && pg.player_id == gsbe.PlayerID
                                select pg) as ObjectQuery<PlayerGameEntity>)
                                 .Take<PlayerGameEntity>(1)
                                 .FirstOrDefault<PlayerGameEntity>();

                    if (null == pge)
                    {
                        ent.PlayerGameSet.AddObject(GetPlayerGameEntity(gsbe));
                    }

                    else
                    {
                        pge.assists = (short)gsbe.Assists;
                        pge.ga = (short)gsbe.GoalsAgainst;
                        pge.game_id = (short)gsbe.GameID;
                        pge.goals = (short)gsbe.Goals;
                        pge.player_id = (short)gsbe.PlayerID;
                        pge.saves = (short)gsbe.Saves;
                    }
                }

                ent.SaveChanges();
            }
        }

        private PlayerGameEntity GetPlayerGameEntity(GameStatBE gsbe)
        {
            var pge = new PlayerGameEntity
            {
                assists = (short)gsbe.Assists,
                ga = (short)gsbe.GoalsAgainst,
                game_id = (short)gsbe.GameID,
                goals = (short)gsbe.Goals,
                player_id = (short)gsbe.PlayerID,
                saves = (short)gsbe.Saves
            };

            return pge;
        }

        private GameEntity GetGameEntity(GameBE gbe)
        {            
            var ge = new GameEntity
            {
                away_team_score = gbe.AwayTeamScore,
                away_team_slug = gbe.AwayTeamSlug.Trim(),
                game_datetime = gbe.Datetime,
                game_season_id = (short)gbe.SeasonID,
                game_status = gbe.Status.Trim(),
                game_type = gbe.Type.Trim(),
                home_team_score = gbe.HomeTeamScore,
                home_team_slug = gbe.HomeTeamSlug.Trim(),
                id = (short)gbe.ID,
                venue = gbe.Venue.Trim()
            };

            return ge;
        }

        private PlayerEntity GetPlayerEntity(PlayerBE pbe)
        {
            var pse = new PlayerSeasonEntity
            {
                @class = pbe.ClassYr.Trim(),
                eligibility = pbe.EligibilityYr.Trim(),
                height = (short)pbe.Height,
                jersey = (short)pbe.JerseyNum,
                player_id = (short)pbe.PlayerID,
                position = pbe.Position.Trim(),
                season_id = (short)pbe.SeasonID,
                weight = (short)pbe.Weight
            };

            var pe = new PlayerEntity
            {
                first = pbe.FirstName.Trim(),
                highschool = pbe.HighSchool.Trim(),
                homestate = pbe.HomeState.Trim(),
                hometown = pbe.Hometown.Trim(),
                id = (short)pbe.PlayerID,
                last = pbe.LastName.Trim(),
                major = pbe.Major.Trim(),
                middle = pbe.MiddleName.Trim()
            };

            pe.PlayerSeason.Add(pse);

            return pe;
        }
    }
}
