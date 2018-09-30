using JLib.Utilities;



namespace Pit
{
    public class LG_SimMatchStartedEvent : MatchInfoEvent
    {
        public LG_SimMatchStartedEvent(BS_MatchParams match) : base(match) { }
    }

    public class LG_SimMatchEndedEvent : MatchInfoEvent
    {
        public LG_SimMatchEndedEvent(BS_MatchParams match) : base(match) { }
    }

    

    public abstract class LG_DayEvent : GameEvent
    {
        public int Day;
    }
    public class LG_DayStartedEvent : LG_DayEvent { }
    public class LG_DayEndedEvent : LG_DayEvent { }


    public class LG_NewLeagueInitializationFinishedEvent : GameEvent
    {

    }


}
