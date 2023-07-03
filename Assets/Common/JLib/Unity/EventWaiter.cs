
using System.Collections;
using System.Linq;
using System.Text;

namespace JLib.Utilities
{
    public abstract class IEventWaiter
    {
        public abstract void Start();
        public abstract void Stop();
        public abstract void Reset();
        public abstract GameEvent Event { get; }
    };


    /// <summary>
    /// A lot of times, we want to listen for an event and then return. doing that in a coroutine is a pain as you have to go out of the coroutine and set up 
    /// callbacks and whatnot. This encapsulates all of that, with the added bonus of providing a timeout. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventWaiter<T> : IEventWaiter where T : GameEvent
    {
        T _event = null;

        public override GameEvent Event { get { return _event; } }

        public override void Start()
        {
            Events.AddGlobalListener<T>(OnEvent);
            Reset();
        }

        public override void Reset()
        {
            _event = null;
        }

        public override void Stop()
        {
            Events.RemoveGlobalListener<T>(OnEvent);
        }

        protected virtual void OnEvent(T ev)
        {
            _event = ev;
        }
    }
}
