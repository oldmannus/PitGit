using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pit.Sim
{


    public abstract class CalendarEntry
    {
        public CalendarDate Date;

        public CalendarEntry(CalendarDate date)
        {
            Date = date;
        }

        public abstract void Execute();
    }

    public class CalendarMatchEntry : CalendarEntry
    {
        public Match Match;

        public CalendarMatchEntry(Match match) : base(match.Params.When)
        {
            Match = match;
        }

        public override void Execute()
        {
            Match.AutoPlay();
        }
    }



    public class CalendarEntryList
    {
        public List<CalendarEntry> Entries = new();
    }
}