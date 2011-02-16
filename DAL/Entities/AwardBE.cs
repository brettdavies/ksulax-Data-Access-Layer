using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSULax.Entities
{
    public class AwardBE
    {
        public int AwardID { get; set; }
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}