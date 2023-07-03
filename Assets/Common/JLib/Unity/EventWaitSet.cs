
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLib.Utilities
{
    public class EventWaitSet
    {
        List<IEventWaiter> _events = new List<IEventWaiter>();
        float _timeout;
        float _timeStarted;


        public EventWaitSet(float timeout = 20.0f)
        {
            _timeout = timeout;
        }

        public EventWaiter<T> Add<T>() where T : GameEvent
        {
            EventWaiter<T> ew = new EventWaiter<T>();
            _events.Add(ew);
            ew.Start();
            return ew;
        }


        public void Reset()
        {
            for (int i = 0; i < _events.Count; i++)
            {
                _events[i].Reset();
            }
        }

        public IEnumerator WaitForEvent(bool start = false)
        {
            _timeStarted = Time.time;

            while ((HasTimedOut == false) && GetEvent() == null)
            {
                yield return null;
            }

            for (int i = 0; i < _events.Count; i++)
            {
                _events[i].Stop();
            }
        }

        bool HasTimedOut
        {
            get
            {
                return _timeout > 0.0f &&
                    (Time.time - _timeStarted) > _timeout;
            }
        }

        public GameEvent GetEvent()
        {
            for (int i = 0; i < _events.Count; i++)
            {
                if (_events[i].Event != null)
                    return _events[i].Event;
            }

            return null;
        }
    }
}
