using Pit.Utilities;


namespace Pit.Sim
{
    public class LG_SimMatchStartedEvent : MatchEvent
    {
        public LG_SimMatchStartedEvent(Match match) : base(match) { }
    }

    public class SimMatchEndedEvent : MatchEvent
    {
        public SimMatchEndedEvent(Match match) : base(match) { }
    }



    public abstract class LG_DayEvent : GameEvent
    {
        public int Day;
    }
    public class LG_DayStartedEvent : LG_DayEvent { }
    public class LG_DayEndedEvent : LG_DayEvent { }





}
