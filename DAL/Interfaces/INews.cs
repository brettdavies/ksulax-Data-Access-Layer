using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSULax.Interfaces
{
    public interface INews
    {
        DateTime Datetime { get; }
        string Story { get; }
        string Title { get; }
        string TitlePath { get; }
        int SeasonID { get; }
        int GameID { get; }

        /// <summary>
        /// The NewsType of the current item
        /// </summary>
        /// <returns>Game or Story</returns>
        NewsType getType();
    }

    public enum NewsType
    {
        Game
        ,Story
    }
}