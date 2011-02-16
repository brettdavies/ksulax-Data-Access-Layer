using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSULax.Entities
{
    public class TeamBE
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Abr { get; set; }
        public string Slug { get; set; }
        public string TeamURL { get; set; }
        public string Mascot { get; set; }
    }
}
