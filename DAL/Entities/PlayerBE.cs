using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSULax.Entities
{
    public class PlayerBE
    {
        public int PlayerID { get; set; }
        public int SeasonID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Hometown { get; set; }
        public string HomeState { get; set; }
        public string HighSchool { get; set; }
        public string Major { get; set; }
        public string Bio { get; set; }
        public int JerseyNum { get; set; }
        public string Position { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string ClassYr { get; set; }
        public string EligibilityYr { get; set; }
        
        /// <summary>
        /// True if the player is club president that season.
        /// </summary>
        public bool President { get; set; }

        /// <summary>
        /// True if the player is a club officer that season.
        /// </summary>
        public bool Officer { get; set; }

        /// <summary>
        /// True if the player is a team captain that season.
        /// </summary>
        public bool Captain { get; set; }
    }
}