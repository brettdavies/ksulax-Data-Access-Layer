using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KSULax.Interfaces;

namespace KSULax.Entities
{
    public class NewsBE : INews
    {
        private string _title;
        private string _titlePath;
        private string _story;

        /// <summary>
        /// Date for a news story
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Title of a news story
        /// </summary>
        public string Title
        {
            get
            {
                return _title.Trim();
            }
            set
            {
                _title = value;
            }
        }

        /// <summary>
        /// Path element of a news story
        /// </summary>
        public string TitlePath
        {
            get
            {
                return _titlePath.Trim().ToLower();
            }

            set
            {
                _titlePath = value;
            }
        }
        
        /// <summary>
        /// Text of news story
        /// </summary>
        public string Story
        {
            get
            {
                return _story.Trim();
            }
            set
            {
                _story = value;
            }
        }

        /// <summary>
        /// Author of a news story
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Source of a news story
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// URL of the source of a news story
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// The NewsType of the current news story
        /// </summary>
        /// <returns>NewsType.Story</returns>
        public NewsType getType()
        {
            return NewsType.Story;
        }

        int INews.SeasonID
        {
            get
            {
                return -1;
            }
        }

        int INews.GameID
        {
            get
            {
                return -1;
            }
        }
    }
}