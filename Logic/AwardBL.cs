using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSULax.Entities;
using System.Data.Objects;
using KSULax.Dal;

namespace KSULax.Logic
{
    public class AwardBL
    {
        private KSULaxEntities _entities;

        public AwardBL(KSULaxEntities entitity) { _entities = entitity; }

        public List<AwardBE> AwardsByPlayerID(int playerID)
        {
            var awards = ((from a in _entities.PlayerAwardSet
                           where a.player_id == playerID
                           select a) as ObjectQuery<PlayerAwardEntity>)
                         .Include("Award");

            var result = new List<AwardBE>();

            foreach (PlayerAwardEntity pae in awards)
            {
                result.Add(GetEntity(pae));
            }

            return result;
        }

        private AwardBE GetEntity(PlayerAwardEntity pae)
        {
            if (null == pae)
            {
                return null;
            }

            var result = new AwardBE
            {
               AwardID = pae.award_id,
               Date = pae.date,
               Name = pae.Award.name,
               PlayerID = pae.player_id
            };
            return result;
        }
    }
}
