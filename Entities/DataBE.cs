using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSULax.Entities
{
    public class TeamRankingBE
    {
        public short pollSource { get; set; }
        public DateTime date { get; set; }
        public string datestr { get; set; }
        public short rank { get; set; }
        public string rankUrl { get; set; }
    }
}