using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSULax.Entities
{
    /// <summary>
    /// Holds stats from a game.
    /// </summary>
    public class GameStatBE
    {
        public int GameID { get; set; }
        public int SeasonID { get; set; }
        public int PlayerID { get; set; }
        public int Assists { get; set; }
        public int Goals { get; set; }
        public int Saves { get; set; }
        public int GoalsAgainst { get; set; }
    }
}
