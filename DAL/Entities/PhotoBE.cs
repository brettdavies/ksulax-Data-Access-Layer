using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSULax.Entities
{
    public class PhotoGalleryBE
    {
        public int GameID { get; set; }
        public string GalleryURL { get; set; }
        public string GalleryName { get; set; }
        public string PhotographerCompany { get; set; }
        public string PhotographerName { get; set; }
        public string PhotographerURL { get; set; }
    }
}