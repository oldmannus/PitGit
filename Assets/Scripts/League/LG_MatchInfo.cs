using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Utilities;

namespace Pit
{


    /// <summary>
    /// Defines a match, either before or after
    /// </summary>
    public class LG_MatchInfo
    {

        // used for calendar stuff
        public int TeamAId;
        public int TeamBId;
        public int Day;
        public int ArenaNdx;

        public BS_MatchResult Result = null;

        // note that these are populated when the match starts to run and some are cleaned out afterwards
        public List<BS_Team> Teams = new List<BS_Team>();
        



    }
}
