//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace Pit.Sim
//{
//    public class SM_SelectableEvent : GameEvent
//    {
//        public List<SM_ISelectable> Who = new List<SM_ISelectable>();
//    }



//    public class SM_SelectionChangedEvent : SM_SelectableEvent
//    {
//        public List<SM_ISelectable> NewWho = new List<SM_ISelectable>();


//        public T Get<T>() where T : class, SM_ISelectable
//        {
//            if (NewWho.Count == 0)
//                return null;


//            // even if we select many, we only want the first
//            return NewWho[0] as T;
//        }
//    }a

//    public class SM_SelectionRequestEvent : GameEvent
//    {
//        public SM_ISelectable Target;
//    }


//    public class SM_SelectionMgrReadyEvent : GameEvent
//    {
//        public SM_SelectionMgr SelMgr;
//    }

//}
