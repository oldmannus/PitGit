using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pit.Utilities;

namespace Pit.Sim
{
    public class Calendar : BehaviourSingleton<Calendar>
    {
        Dictionary<CalendarDate, CalendarEntryList> Events = new();

        public CalendarDate Today { get; private set; }

        // ----------------------------------------------------------------------
        /// <summary>
        /// Adds single event
        /// </summary>
        /// <param name="entry"></param>
        // ----------------------------------------------------------------------
        public void AddEvent(CalendarEntry entry)
        {
            if (Events.TryGetValue(entry.Date, out CalendarEntryList eventsOnDate))
            {
                eventsOnDate.Entries.Add(entry);
            }
            else
            {
                CalendarEntryList eventOnDateList = new();
                eventOnDateList.Entries.Add(entry);
                Events.Add(entry.Date, eventOnDateList);
            }
        }
        // ----------------------------------------------------------------------
        /// <summary>
        /// executes all events on today
        /// </summary>
        /// <param name="entry"></param>
        // ----------------------------------------------------------------------
        public bool DoADay()
        {
            if (!Events.TryGetValue(Today.Increment(), out CalendarEntryList entries))
            {
                return false;
            }

            int count = entries.Entries.Count;
            for (int i = 0; i < count; i++)
            {
                CalendarEntry entry = entries.Entries[i];
                entry.Execute();
            }
            return true;
        }
        // ----------------------------------------------------------------------
        /// <summary>
        /// checks to make sure no team is playing twice on the same day
        /// </summary>
        /// <param name="entry"></param>
        // ----------------------------------------------------------------------
        public void ValidateMatchConsistency()
        {
            HashSet<int> playingToday = new();
            foreach (var v in Events)
            {
                CalendarEntryList entries = v.Value;
                int day = v.Key.Day;

                playingToday.Clear();
                
                foreach (var entry in entries.Entries)
                {
                    if (entry is CalendarMatchEntry)
                    {
                        CalendarMatchEntry cme = (CalendarMatchEntry)entry;
                        foreach (var teamId in cme.Match.TeamIds)
                        {
                            Dbg.Assert(!playingToday.Contains(teamId.Id));

                            playingToday.Add(teamId.Id);
                        }
                    }
                }
            }
        }

        public void DoAllDays()
        {
            while (DoADay()) ;
        }


        public void ScheduleRoundRobin(CalendarDate startDate, int numTeams, int numRounds)
        {
            Match m;
            List<Match> unAssignedMatches = new();

            int day;
            for (int home = 0; home < numTeams; home++)
            {
                day = 0;
                for (int away = home + 1; away < numTeams; away++, day++)
                {
                    m = new();
                    m.Initialize(home, away, day);

                    Dbg.Log($"Match:  {home} {away} : {day}");
                    AddEvent(new CalendarMatchEntry(m));
                }
            }

            ValidateMatchConsistency();
        }
    }


    public struct CalendarDate
    {
        public int Day { get; private set; }


        public CalendarDate( int day )
        {
            Day = day;
        }
        public CalendarDate Increment() { Day++; return this; }
    }
}