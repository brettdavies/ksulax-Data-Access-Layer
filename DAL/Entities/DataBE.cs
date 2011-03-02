using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSULax.Entities
{
    public class TeamRankingBE
    {
        public short pollSource { get; set; }
        public DateTime Date { get; set; }
        public string Datestr { get; set; }
        public short Rank { get; set; }
        public string Url { get; set; }
    }
}