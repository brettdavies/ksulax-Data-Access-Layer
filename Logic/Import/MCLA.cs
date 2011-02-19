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
                        //ent.PlayerSet.AddObject(GetPlayerEntity(pbe));
                    }

                    else
                    {
                        player.first = pbe.FirstName;
                        player.highschool = pbe.HighSchool;
                        player.homestate = pbe.HomeState;
                        player.hometown = pbe.Hometown;
                        player.last = pbe.LastName;
                        player.major = pbe.Major;
                        player.middle = pbe.MiddleName;

                        var playerseason = player.PlayerSeason.Single(x => x.season_id.Equals(pbe.SeasonID));
                        
                        player.PlayerSeason.Remove(playerseason);

                        if (null != playerseason)
                        {
                            playerseason.@class = pbe.ClassYr;
                            playerseason.eligibility = pbe.EligibilityYr;
                            playerseason.height = (short)pbe.Height;
                            playerseason.jersey = (short)pbe.JerseyNum;
                            playerseason.position = pbe.Position;
                            playerseason.weight = (short)pbe.Weight;
                        }

                        player.PlayerSeason.Add(playerseason);
                    }
                }

                //ent.SaveChanges();
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
                        //ent.GameSet.AddObject(GetGameEntity(gbe));
                    }

                    else
                    {
                        game.away_team_score = gbe.AwayTeamScore;
                        game.away_team_slug = gbe.AwayTeamSlug;
                        game.game_date = gbe.Date;
                        game.game_season_id = (short)gbe.SeasonID;
                        game.game_status = gbe.Status;
                        game.game_time = gbe.Time;
                        game.game_type = gbe.Type;
                        game.home_team_score = gbe.HomeTeamScore;
                        game.home_team_slug = gbe.HomeTeamSlug;
                        game.id = (short)gbe.ID;
                        game.venue = gbe.Venue;
                    }
                }

                //ent.SaveChanges();
            }
        }

        public void UpdatePlayerGame(List<PlayerGameBE> playergames)
        {
            using (var ent = new KSULaxEntities())
            {
                foreach (PlayerGameBE pgbe in playergames)
                {
                    var pge = ((from pg in ent.PlayerGameSet
                                 where pg.game_id == pgbe.GameID
                                 && pg.player_id == pgbe.PlayerID
                                 select pg) as ObjectQuery<PlayerGameEntity>)
                                .Take<PlayerGameEntity>(1)
                                .FirstOrDefault<PlayerGameEntity>();

                    if (null == pge)
                    {
                        //ent.PlayerGameSet.AddObject(GetPlayerGameEntity(pgbe));
                    }

                    else
                    {
                        pge.assists = (short)pgbe.Assists;
                        pge.ga = (short)pgbe.GoalsAgainst;
                        pge.game_id = (short)pgbe.GameID;
                        pge.goals = (short)pgbe.Goals;
                        pge.player_id = (short)pgbe.PlayerID;
                        pge.saves = (short)pgbe.Saves;
                    }
                }

                //ent.SaveChanges();
            }
        }

        private PlayerGameEntity GetPlayerGameEntity(PlayerGameBE pgbe)
        {
            var pge = new PlayerGameEntity
            {
                assists = (short)pgbe.Assists,
                ga = (short)pgbe.GoalsAgainst,
                game_id = (short)pgbe.GameID,
                goals = (short)pgbe.Goals,
                player_id = (short)pgbe.PlayerID,
                saves = (short)pgbe.Saves
            };

            return pge;
        }

        private GameEntity GetGameEntity(GameBE gbe)
        {            
            var ge = new GameEntity
            {
                away_team_score = gbe.AwayTeamScore,
                away_team_slug = gbe.AwayTeamSlug,
                game_date = gbe.Date,
                game_season_id = (short)gbe.SeasonID,
                game_status = gbe.Status,
                game_time = gbe.Time,
                game_type = gbe.Type,
                home_team_score = gbe.HomeTeamScore,
                home_team_slug = gbe.HomeTeamSlug,
                id = (short)gbe.ID,
                venue = gbe.Venue
            };

            return ge;
        }

        private PlayerEntity GetPlayerEntity(PlayerBE pbe)
        {
            var pse = new PlayerSeasonEntity
            {
                @class = pbe.ClassYr,
                eligibility = pbe.EligibilityYr,
                height = (short)pbe.Height,
                jersey = (short)pbe.JerseyNum,
                player_id = (short)pbe.PlayerID,
                position = pbe.Position,
                season_id = (short)pbe.SeasonID,
                weight = (short)pbe.Weight
            };

            var pe = new PlayerEntity
            {

                first = pbe.FirstName,
                highschool = pbe.HighSchool,
                homestate = pbe.HomeState,
                hometown = pbe.Hometown,
                id = (short)pbe.PlayerID,
                last = pbe.LastName,
                major = pbe.Major,
                middle = pbe.MiddleName,
            };

            pe.PlayerSeason.Add(pse);

            return pe;
        }
    }
}
