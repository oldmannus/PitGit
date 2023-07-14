////From http://www.willrmiller.com/?p=87 plus modifications by Evelyn Leigh and Jason Snyder
//using System;
//using System.Collections.Generic;

//namespace Framework.
//{

//    /// <summary>
//    /// Base class of events send through the event system
//    /// </summary>
//    [Serializable]
//    public class GameEvent
//    {
//    }


//    /// <summary>
//    /// Basically static class to manage listeners
//    /// </summary>
//    [Serializable]
//    public class Events
//    {
//        // delegate definition for users 
//        public delegate void EventDelegate<T>(T e) where T : GameEvent;



//        // ***************** GLOBAL FUNCTIONS ***************************************************************************


//        // ---------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// adds a function that listens for the given event when the event is sent globally
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="del"></param>
//        // ---------------------------------------------------------------------------------------------------------
//        public static void AddGlobalListener<T>(Events.EventDelegate<T> del) where T : GameEvent
//        {
//            AddListener(del, Internal._globalDelegates);
//        }

//        // ---------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// removes a function that listens for the given event when the event is sent globally
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="del"></param>
//        // ---------------------------------------------------------------------------------------------------------

//        public static void RemoveGlobalListener<T>(Events.EventDelegate<T> del) where T : GameEvent
//        {
//            RemoveListener(del, Internal._globalDelegates);
//        }

//        // ---------------------------------------------------------------------------------------------------------
//        // Sends the given message ONLY to global delegates. Not the object-based ones
//        // ---------------------------------------------------------------------------------------------------------
//        public static void SendGlobal(GameEvent e)
//        {
//            Send(e, Internal._globalDelegates);
//        }


//        // ***************** OBJECT FUNCTIONS ***************************************************************************

//        // ---------------------------------------------------------------------------------------------------------
//        /// <summary>
//        ///  adds a function that listens for the given event when the event is sent to this object
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="del"></param>
//        /// <param name="o"></param>
//        public static void AddObjectListener<T>(Events.EventDelegate<T> del, object o) where T : GameEvent
//            // ---------------------------------------------------------------------------------------------------------
//        {
//            AddListener<T>(del, Internal.GetOrCreateDelegates(o));
//        }

//        // ---------------------------------------------------------------------------------------------------------
//        /// <summary>
//        ///  removes a function that listens for the given event when the event is sent to this object
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="del"></param>
//        /// <param name="o"></param>
//        public static void RemoveObjectListener<T>(Events.EventDelegate<T> del, object o) where T : GameEvent
//            // ---------------------------------------------------------------------------------------------------------
//        {
//            DelegateSet delegateSet = Internal.GetDelegates(o);
//            Dbg.Assert(delegateSet != null);   // odd otherwise
//            RemoveListener(del, delegateSet);

//            if (delegateSet.Lookup.Count == 0)
//            {
//                Internal.RemoveDelegates(o);
//            }
//        }


//        // ---------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Sends given event just to the target object
//        /// </summary>
//        /// <param name="e"></param>
//        /// <param name="o"></param>
//        public static void SendObject(GameEvent e, object o)
//        // ---------------------------------------------------------------------------------------------------------
//        {
//            Send(e, Internal.GetDelegates(o));
//        }


//        // ************************* INTERNAL STRUCTURES **************************************************************************

//        // -----------------------------------------------------------------------------------
//        sealed class Bounce
//        // -----------------------------------------------------------------------------------
//        {
//            public Queue<GameEvent> Queue { get; private set; }
//            public bool HasResult { get; private set; }

//            public static Bounce Continue(Queue<GameEvent> q) { return new Bounce() { Queue = q }; }
//            public static Bounce End() { return new Bounce() { HasResult = true }; }
//        }


//        // -----------------------------------------------------------------------------------
//        static class Trampoline
//        // -----------------------------------------------------------------------------------
//        {
//            public static void Start(Func<Queue<GameEvent>, DelegateSet, Bounce> action, DelegateSet delegateSet)

//            {
//                Bounce bounce = Bounce.Continue(delegateSet.Queue);
//                while (true)
//                {
//                    if (bounce.HasResult)
//                    {
//                        break;
//                    }

//                    bounce = action(bounce.Queue, delegateSet);
//                }
//            }
//        }


//        // ************************* INTERNAL DATA **************************************************************************
//        private delegate void EventDelegate(GameEvent e);

//        [Serializable]
//        class DelegateSet
//        {
//            public Dictionary<System.Delegate, EventDelegate> Lookup = new Dictionary<System.Delegate, EventDelegate>();
//            public Dictionary<System.Type, EventDelegate> Delegates = new Dictionary<System.Type, EventDelegate>();
//            public Queue<GameEvent> Queue = new Queue<GameEvent>();

//        }

//        Dictionary<object, DelegateSet> _objectDelegates = new Dictionary<object, DelegateSet>();
//        DelegateSet _globalDelegates = new DelegateSet();

//        static Events _instance;
//        static Events Internal
//        {
//            get
//            {
//                if (_instance == null)
//                {
//                    _instance = new Events();
//                }
//                return _instance;
//            }
//        }


//        // ------------------------------------------------------------------------------------------------------------------------------------------------
//        static void AddListener<T>(EventDelegate<T> del, DelegateSet delegateSet) where T : GameEvent
//            // ------------------------------------------------------------------------------------------------------------------------------------------------
//        {
//            // Early-out if we've already registered this delegate
//            if (delegateSet.Lookup.ContainsKey(del))
//                return;

//            // Create a new non-generic delegate which calls our generic one.
//            // This is the delegate we actually invoke.
//            EventDelegate internalDelegate = (e) => del((T)e);
//            delegateSet.Lookup[del] = internalDelegate;

//            EventDelegate tempDel;
//            if (delegateSet.Delegates.TryGetValue(typeof(T), out tempDel))
//            {
//                delegateSet.Delegates[typeof(T)] = tempDel += internalDelegate;
//            }
//            else
//            {
//                delegateSet.Delegates[typeof(T)] = internalDelegate;
//            }
//        }


//        // -----------------------------------------------------------------------------------
//        static void RemoveListener<T>(EventDelegate<T> del, DelegateSet delegateSet) where T : GameEvent
//            // -----------------------------------------------------------------------------------
//        {
//            EventDelegate internalDelegate;
//            if (delegateSet.Lookup.TryGetValue(del, out internalDelegate))
//            {
//                EventDelegate tempDel;
//                if (delegateSet.Delegates.TryGetValue(typeof(T), out tempDel))
//                {
//                    tempDel -= internalDelegate;
//                    if (tempDel == null)
//                    {
//                        delegateSet.Delegates.Remove(typeof(T));
//                    }
//                    else
//                    {
//                        delegateSet.Delegates[typeof(T)] = tempDel;
//                    }
//                }

//                delegateSet.Lookup.Remove(del);
//            }

//        }

//        // -----------------------------------------------------------------------------------
//        static void Send(GameEvent e, DelegateSet delegateSet)
//        // -----------------------------------------------------------------------------------
//        {
//            if (delegateSet == null)
//            {
//                return; // no one listening for this event, so ignore it
//            }
//            delegateSet.Queue.Enqueue(e);

//            if (delegateSet.Queue.Count == 1)
//            {
//                Trampoline.Start(ProcessQueue, delegateSet);
//            }
//        }


//        // ******************************* ACCESS FOR OBJECT DELEGATE MANAGEMENT **************************************************************

//        // -----------------------------------------------------------------------------------
//        DelegateSet GetDelegates(object obj)
//        // -----------------------------------------------------------------------------------
//        {
//            DelegateSet delegateSet;
//            if (_objectDelegates.TryGetValue(obj, out delegateSet) == false)
//            {
//                return null;
//            }
//            return delegateSet;
//        }

//        // -----------------------------------------------------------------------------------
//        DelegateSet GetOrCreateDelegates(object obj)
//        // -----------------------------------------------------------------------------------
//        {
//            DelegateSet delegateSet;
//            if (_objectDelegates.TryGetValue(obj, out delegateSet) == false)
//            {
//                delegateSet = new DelegateSet();
//                _objectDelegates.Add(obj, delegateSet);
//            }
//            return delegateSet;
//        }

//        // -----------------------------------------------------------------------------------
//        void RemoveDelegates(object obj)
//        // -----------------------------------------------------------------------------------
//        {
//            _objectDelegates.Remove(obj);
//        }




//        // ************************** QUEUE CALLBACKS - called from Trampoline/bounce ******************************************************************************


//        // -----------------------------------------------------------------------------------
//        static Bounce ProcessQueue(Queue<GameEvent> q, DelegateSet delegateSet)
//        // -----------------------------------------------------------------------------------
//        {
//            if (q.Count == 0)
//            {
//                return Bounce.End();
//            }
//            else
//            {
//                ProcessEvent(q.Peek(), delegateSet);
//                q.Dequeue();
//                return Bounce.Continue(q);
//            }
//        }


//        // -----------------------------------------------------------------------------------
//        static void ProcessEvent(GameEvent e, DelegateSet delegateSet)
//        // -----------------------------------------------------------------------------------
//        {
//            EventDelegate del;
//            if (delegateSet.Delegates.TryGetValue(e.GetType(), out del))
//            {
//                del.Invoke(e);
//            }
//        }
//    }
//}