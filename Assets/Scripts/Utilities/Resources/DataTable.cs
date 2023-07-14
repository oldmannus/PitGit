//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//

//namespace Dominion.Data
//{
//    public class MakeNewObjEvent<T> : GameEvent where T : NameableSimObj
//    {
//        public object Data;
//    }

//    public class OnNewObjectCreatedEvent<T> : GameEvent where T : NameableSimObj
//    {
//        public T NewObject;

//        public OnNewObjectCreatedEvent(T thing)
//        {
//            NewObject = thing;
//        }
//    }

//    public class NameableSimObj : SimObj 
//    {
//        public NameableSimObj( )
//        {

//        }

//        public virtual void CreateNew(object nameBuilder, object data)
//        {
//        }
//    }

//    public class DataTable<T> : SimObjTable<T> where T : NameableSimObj, new()
//    {
//        object _nameBuilder;

//        public virtual void Initialize(object nameBuilder)
//        {
//            _nameBuilder = nameBuilder;
//            Events.AddGlobalListener<MakeNewObjEvent<T>>(OnMakeNewObjEvent);
//        }
//        public virtual void Shutdown()
//        {
//            Events.RemoveGlobalListener<MakeNewObjEvent<T>>(OnMakeNewObjEvent);
//        }

//        void OnMakeNewObjEvent(MakeNewObjEvent<T> ev )
//        {
//            T newThing = MakeNew();
//            newThing.CreateNew(_nameBuilder, ev.Data);

//            Events.SendGlobal(new OnNewObjectCreatedEvent<T>(newThing));
//        }
//    }
//}
