using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KSULax.Interfaces;

namespace KSULax.Entities
{
    /// <summary>
    /// Holds information related to a game.
    /// </summary>
    public class GameBE : INews
    {
        public int ID { get; set; }
        public int SeasonID { get; set; }
        public DateTime Datetime { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Venue { get; set; }
        public string Detail { get; set; }
        public int AwayTeamScore { get; set; }
        public int HomeTeamScore { get; set; }
        public TeamBE HomeTeam { get; set; }
        public TeamBE AwayTeam { get; set; }
        public string AwayTeamSlug { get; set; }
        public string HomeTeamSlug { get; set; }
        public bool isHome { get; set; }

        string INews.Story
        {
            get
            {
                return (string.IsNullOrEmpty(Detail)) ? string.Empty : Detail;
            }
        }

        string INews.Title
        {
            get
            {
                return (isHome ? HomeTeam.Abr : AwayTeam.Abr) + " "
                    + GameResultBL(HomeTeamScore, AwayTeamScore, isHome) + " "
                    + (isHome ? AwayTeam.Abr : HomeTeam.Abr) + " "
                + (isHome ? HomeTeamScore : AwayTeamScore) + " - "
                + (isHome ? AwayTeamScore : HomeTeamScore) + " "
                + (isHome ? "at home" : "on the road");
            }
        }

        /// <summary>
        /// The NewsType of the current game
        /// </summary>
        /// <returns>NewsType.Game</returns>
        public NewsType getType()
        {
            return NewsType.Game;
        }

        /// <summary>
        /// The GameID
        /// </summary>
        int INews.GameID
        {
            get
            {
                return ID;
            }
        }

        string INews.TitlePath
        {
            get
            {
                return string.Empty;
            }
        }

        private string GameResultBL(int HomeTeamScore, int AwayTeamScore, bool isHome)
        {
            if (HomeTeamScore.Equals(-1) || AwayTeamScore.Equals(-1))
            {
                return "-";
            }

            else if (isHome && (HomeTeamScore > AwayTeamScore))
            {
                return "beats";
            }

            else if (!isHome && (AwayTeamScore > HomeTeamScore))
            {
                return "beats";
            }

            else if (!isHome)
            {
                return "loses to";
            }

            else if (isHome)
            {
                return "loses to";
            }

            else
            {
                return string.Empty;
            }
        }
    }
}
