//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace Pit.Sim
//{
//    /// <summary>
//    /// An InputMode is designed to handle all of the input logic for a given 'mode' of input. 
//    /// For example, we might be in the default 'mode' where clicking on a pawn means we select it.
//    /// Or we might be in targeting "mode" where clicking on a pawn means it's the target of an action
//    /// </summary>
//    public abstract class SM_InputMode
//    {
//        protected SM_IInputMgr _inputMgr;

//        protected Object[] _params = null;


//        public virtual void Awake(SM_IInputMgr im)
//        {
//            Dbg.Assert(im != null);
//            _inputMgr = im;
//        }

//        public void SetParams(Object[] p)
//        {
//            _params = p;
//        }


//        public virtual void OnEnable() { }
//        public virtual void OnDisable() { }

//        public virtual void OnDestroy() { }

//        // the bool is for nested classes, so a parent class can tell the child class it doesn't have to run
//        public virtual bool Update() { return false; }
//    }
//}
