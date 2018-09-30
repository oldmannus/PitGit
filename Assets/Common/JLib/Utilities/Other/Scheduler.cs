using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace JLib.Utilities
{
    [Serializable]
    public class Scheduler
    {

        // -------------------------------------------------------------------------------------------
        public class DelegateTask
        // -------------------------------------------------------------------------------------------
        {
            int _interval;
            int _lastUpdate;
            DoItDelegate _whatToDo;
            object _what;

            
            public string Name { get; private set; }

            public delegate bool DoItDelegate(string name, object what);


            public bool Active;

            public bool Update(int updateId)
            {
                if (updateId - _lastUpdate < _interval)
                    return false;
                _lastUpdate = updateId;
                return _whatToDo(Name, _what);
            }

            public DelegateTask(string name, int interval)
            {
                Name = name;
                _interval = interval;
            }

            public DelegateTask(string name, int interval, DoItDelegate dlg, object w)
            {
                Name = name;
                _interval = interval;
                _what = w;
                _whatToDo = dlg;
            }

            protected void SetDelegate( DoItDelegate dlg )
            {
                _whatToDo = dlg;
            }

            public void SetUpdateInterval(int interval)
            {
                _interval = interval;
            }

        }


        // -------------------------------------------------------------------------------------------
        public abstract class BaseTask : DelegateTask
        // -------------------------------------------------------------------------------------------
        {
            public BaseTask(string name, int interval) : base( name, interval)
            {
                SetDelegate(DoIt);
            }

            protected abstract bool Process();

            public bool DoIt(string name, object o)
            {
                return Process();
            }

        }

        List<DelegateTask>          _tasks = new List<DelegateTask>();
        List<DelegateTask>          _deadTasks = new List<DelegateTask>();
        int                 _curUpdate;



        // ******************************* PUBLIC METHODS *******************************************************************

        public Scheduler.DelegateTask AddTask(string name, int interval, DelegateTask.DoItDelegate dlg, object what )
        {
            Debug.Assert(_tasks.Find(x => x.Name == name) == null);
            DelegateTask t = new DelegateTask(name, interval, dlg, what);
            t.Active = true;
            _tasks.Add(t);
            return t;
        }

        public void AddTask(DelegateTask dlg )
        {
            Debug.Assert(_tasks.Find(x => x.Name == dlg.Name) == null);
            dlg.Active = true;
            _tasks.Add(dlg);
        }

        public void RemoveTask(string name)
        {
            DelegateTask t = _tasks.Find(x => x.Name == name);
            t.Active = false;
            RemoveTask(t);
        }

        public bool HasTask( string name )
        {
            return _tasks.Find(x => x.Name == name) != null; 
        }

        public void RemoveTask(DelegateTask t)
        {
            Debug.Assert(t != null);
            t.Active = false;
            _deadTasks.Add(t);
        }


        public void Update()
        {
            ClearDeadTasks();

            _curUpdate++;

            foreach (DelegateTask t in _tasks)
            {
                // in the event that doing a task invalidates a task, check for that
                if (_deadTasks.Contains(t))
                    continue;


                bool isFinished = t.Update(_curUpdate);

                if (isFinished)
                {
                    _deadTasks.Add(t);
                }
            }
        }

        void ClearDeadTasks()
        {
            foreach (DelegateTask t in _deadTasks)
                _tasks.Remove(t);

            _deadTasks.Clear();
        }

    }
}
