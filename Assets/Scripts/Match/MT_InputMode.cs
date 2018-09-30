using System.Collections.Generic;
using UnityEngine;
using JLib.Sim;
using JLib.Utilities;
using JLib.Unity;
using UnityEngine.SceneManagement;


namespace Pit
{
    public class MT_InputModeCanceledEvent : GameEvent
    {
    }

    /// <summary>
    /// All input modes in the match are to derive from this. It encapsulates common functionality
    /// </summary>
    public class MT_InputMode : SM_InputMode
    {
        public virtual bool CanSelectionChange { get { return true; } }   // ### TODO: fix it so that the selection "can change" checks this
        public virtual bool CheckForHilightChange { get { return true; } }
        public virtual bool EscapeReturnsToStart { get { return true; } }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        public override bool Update()
        {
            base.Update();

            // return false, as we're changing states and the rest of the input should be ignored
            if (EscapeReturnsToStart && Input.GetKeyUp(KeyCode.Escape))
            {
                Events.SendGlobal(new MT_InputModeCanceledEvent());
                _inputMgr.QueueInputMode<MT_InputModeBase>();
                return true;
            }

            return false;
        }

        public int GetNumberKeyDown()
        {
            for (int i = 0; i < _numberKeys.Length; i++)
            {
                if (Input.GetKeyDown(_numberKeys[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        static KeyCode[] _numberKeys = new KeyCode[]
        {
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9
        };





        // -----------------------------------------------------------------------------------------
        /// <summary>
        /// Clears everything on scene load, as it would be odd to have hilighting carrying over between scenes
        /// This might need to be changed if we load multiple scenes
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        // -----------------------------------------------------------------------------------------
        {
            _hilighted.Clear();
        }

        protected void UnHilightAll()
        {
            for (int i = 0; i < _hilighted.Count; i++)
            {
                _hilighted[i].SetHilighted(false);
            }
            _hilighted.Clear();
        }


        // ------------------------------------------------------------------------------------------------
        /// <summary>
        /// Simply returns the 'best' ISelectable under the mouse
        /// </summary>
        /// <returns></returns>
        protected SM_ISelectable CheckForObjectUnderMouse(UN_Camera cam)
        // ------------------------------------------------------------------------------------------------
        {
            if (Dbg.Assert(cam != null))
                return null;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray);
            if (hits == null)
                return null;

            int bestNdx = -1;
            float bestDst = float.MaxValue;
            SM_ISelectable bestSelect = null;
            for (int i = 0; i < hits.Length; i++)
            {

                if (hits[i].distance < bestDst)
                {
                    if (CheckParentsForSelectable(hits[i].transform, ref bestSelect))
                    {
                        bestNdx = i;
                    }
                }
            }

            if (bestNdx < 0)
                return null;

            return bestSelect;
        }

        protected virtual bool CanSelect( SM_ISelectable sel)
        {
            return true;
        }

        protected bool CheckParentsForSelectable(Transform trans, ref SM_ISelectable select)
        {
            SM_ISelectable sel = trans.gameObject.GetComponent<SM_ISelectable>();
            if (sel != null && CanSelect(sel))
            {
                select = sel;
                return true;
            }

            for (int i = 0; i < trans.childCount; i++)
            {
                Transform newT = trans.GetChild(i);
                if (CheckParentsForSelectable(newT, ref select))
                    return true;
            }

            return false;
        }


        protected bool CheckParentsForPawn(Transform trans, ref SM_Pawn select)
        {
            SM_Pawn sel = trans.gameObject.GetComponent<SM_Pawn>();
            if (sel != null /* && CanSelect(sel) */ ) //### TODO add team logic 
            {
                select = sel;
                return true;
            }

            for (int i = 0; i < trans.childCount; i++)
            {
                Transform newT = trans.GetChild(i);
                if (CheckParentsForPawn(newT, ref select))
                    return true;
            }

            return false;
        }


        // ******************** SELECTION STUFF ************************************************


        protected List<SM_ISelectable> _hilighted = new List<SM_ISelectable>();



    }

}
