using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KSULax.Entities;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using KSULax.Dal;

namespace KSULax.Logic
{
    public class PhotoBL
    {
        private KSULaxEntities _entities;

        public PhotoBL(KSULaxEntities entitity) { _entities = entitity; }

        public List<PhotoGalleryBE> PhotoGalleriesBySeason(int seasonID)
        {
            var galleries = ((from pg in _entities.PhotoGallerySet
                              where pg.Game.game_season_id == seasonID
                              orderby pg.Game.game_date
                              orderby pg.url
                              select pg) as ObjectQuery<PhotoGalleryEntity>)
                              .Include("Photographer")
                              .Include("Game");

            var result = new List<PhotoGalleryBE>();

            foreach (PhotoGalleryEntity pge in galleries)
            {
                result.Add(GetEntity(pge));
            }
            return result;
        }

         public List<PhotoGalleryBE> PhotoGalleriesByGame(int gameID)
         {
             var galleries = ((from pg in _entities.PhotoGallerySet
                               where pg.Game.game_season_id == gameID
                               orderby pg.Game.game_date
                               select pg) as ObjectQuery<PhotoGalleryEntity>)
                           .Include("Photographer")
                           .Include("Game")
                           .Take(1);

             var result = new List<PhotoGalleryBE>();

             foreach (PhotoGalleryEntity pge in galleries)
             {
                 result.Add(GetEntity(pge));
             }
             return result;
         }

        /// <summary>
        /// Converts a PhotoGalleryEntity object to a PhotoGallery object
        /// </summary>
        /// <param name="photoE">PhotoGalleryEntity to convert</param>
        /// <returns></returns>
        private PhotoGalleryBE GetEntity(PhotoGalleryEntity pge)
        {
            if (null == pge)
            {
                return null;
            }

            var result = new PhotoGalleryBE
            {
                GameID = pge.Game.id,
                GalleryURL = pge.url,
                PhotographerCompany = pge.Photographer.company,
                PhotographerName = pge.Photographer.name,
                PhotographerURL = pge.Photographer.url,
                GalleryName = pge.text
            };
            return result;
        }
    }
}