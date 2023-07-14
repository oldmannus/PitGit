
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Pit.Utilities;

//namespace Pit.Sim
//{


//    public class SM_SelectionMgr : MonoBehaviour
//    {
//        public enum Mode
//        {
//            Disabled,   // no selection
//            Single,     // only 1 selection/highlight at a time. Setting one unsets the others
//            Multi,      // can support multiple objects. Drag select permitted
//        }

//        [SerializeField]
//        Mode _selectionMode = Mode.Single;

//        [SerializeField]
//        Mode _hilightMode = Mode.Single;


//        protected List<SM_ISelectable> _selected = new List<SM_ISelectable>();
//        protected List<SM_ISelectable> _hilighted = new List<SM_ISelectable>();



//        // ---------------- STANRD UNITY STUFF ----------------------------------------------------

//        protected virtual void Awake()
//        {
//            SceneManager.sceneLoaded += OnSceneLoaded;
//            Events.AddGlobalListener<SM_SelectionRequestEvent>(OnSelectionRequestEvent);
//        }

//        protected virtual void OnDestroy()
//        {
//            Events.RemoveGlobalListener<SM_SelectionRequestEvent>(OnSelectionRequestEvent);
//            SceneManager.sceneLoaded -= OnSceneLoaded;
//        }
//        protected virtual void Start()
//        {
//            Events.SendGlobal(new SM_SelectionMgrReadyEvent() { SelMgr = this });
//        }


//        // -----------------------  Public API ------------------------------------------------


//        // ------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Called most of the time the sim is running to highlight whatever is under the mouse currently.
//        /// If it's part of set, then highlight is updated on selected set. I.e. if we have mouse-dragged 
//        /// to select a bunch of stuff, subsequently doing a mouse-over one of them will highlight all of them
//        /// </summary>
//        /// <returns></returns>
//        public void UpdateHilight(UN_Camera cam)
//        // ------------------------------------------------------------------------------------------------
//        {
//            SM_ISelectable obj = CheckForObjectUnderMouse(cam);
//            if (obj == null)
//            {
//                UnHilightAll();
//            }
//            else if (obj.IsHilighted == false) // the object wasn't hilighted before
//            {
//                UnHilightAll();     // clear out old set

//                // if this object is a member of a set, then hilight all of them
//                if (obj.IsSelected && _selected.Count > 1)
//                {
//                    foreach (var v in _selected)
//                    {
//                        SetHilighted(v);
//                    }
//                }
//                else
//                {
//                    SetHilighted(obj);
//                }
//            }
//            // otherwise we were already hilighted and have nothing to do. 
//        }


//        public SM_ISelectable GetFirstSelected()
//        {
//            return _selected.Count == 0 ? null : _selected.ElementAt(0);
//        }


//        // -----------------------------------------------------------------------------------------
//        /// <summary>
//        /// Sets how we deal with input - ignore, select one, allow drag select of highlights 
//        /// </summary>
//        /// <param name="mode"></param>
//        public void SetHilightMode(Mode mode)
//        // -----------------------------------------------------------------------------------------
//        {
//            _hilightMode = mode;
//        }

//        // -----------------------------------------------------------------------------------------
//        /// <summary>
//        /// Sets how we deal with input - ignore, select one, allow drag select of selections
//        /// </summary>
//        /// <param name="mode"></param>
//        public void SetSelectMode(Mode mode)
//        // -----------------------------------------------------------------------------------------
//        {
//            _selectionMode = mode;
//        }

//        // -----------------------------------------------------------------------------------------
//        /// <summary>
//        /// should be called when we're curious if the mouse is selecting something else
//        /// </summary>
//        /// <param name="mode"></param>
//        public bool SelectSingleUnderMouse(UN_Camera camera)
//        // -----------------------------------------------------------------------------------------
//        {
//            return SetSelected(CheckForObjectUnderMouse(camera));
//        }



//        // ---------------- Private methods --------------------------------------------------------




//        // -----------------------------------------------------------------------------------------
//        /// <summary>
//        /// Clears everything on scene load, as it would be odd to have hilighting carrying over between scenes
//        /// This might need to be changed if we load multiple scenes
//        /// </summary>
//        /// <param name="scene"></param>
//        /// <param name="mode"></param>
//        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//        // -----------------------------------------------------------------------------------------
//        {
//            _selected.Clear();
//            _hilighted.Clear();
//        }

//        void OnSelectionRequestEvent(SM_SelectionRequestEvent ev)
//        {
//            if (Dbg.Assert(ev.Target != null))
//                return;

//            if (CanSelect(ev.Target))
//            {
//                SetSelected(ev.Target);
//            }
//        }

//        void UnHilightAll()
//        {
//            for (int i = 0; i < _hilighted.Count; i++)
//            {
//                _hilighted[i].SetHilighted(false);
//            }
//            _hilighted.Clear();
//        }

//        /// <summary>
//        /// Sets object to be selected. Returns true if something changed
//        /// </summary>
//        /// <param name="obj"></param>
//        bool SetSelected(SM_ISelectable obj)
//        {
//            if (obj != null && obj.IsSelected == false)
//            {
//                SM_SelectionChangedEvent ev = new SM_SelectionChangedEvent();

//                if (_selected.Count != 0)
//                {
//                    for (int i = 0; i < _selected.Count; i++)
//                    {
//                        ev.Who.Add(_selected[i]);
//                        _selected[i].SetSelected(false);
//                    }
//                    _selected.Clear();
//                }

//                _selected.Add(obj);
//                obj.SetSelected(true);

//                ev.NewWho.AddRange(_selected);
//                Events.SendGlobal(ev);

//                return true;
//            }
//            return false;

//        }

//        /// <summary>
//        /// Sets object to be selected
//        /// </summary>
//        /// <param name="obj"></param>
//        void SetHilighted(SM_ISelectable obj)
//        {
//            if (obj != null && obj.IsHilighted == false)
//            {
//                _hilighted.Add(obj);
//                obj.SetHilighted(true);
//            }
//        }




//        // ------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Simply returns the 'best' ISelectable under the mouse
//        /// </summary>
//        /// <returns></returns>
//        SM_ISelectable CheckForObjectUnderMouse(UN_Camera cam)
//        // ------------------------------------------------------------------------------------------------
//        {
//            if (Dbg.Assert(cam != null))
//                return null;

//            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

//            RaycastHit[] hits = Physics.RaycastAll(ray);
//            if (hits == null)
//                return null;

//            int bestNdx = -1;
//            float bestDst = float.MaxValue;
//            SM_ISelectable bestSelect = null;
//            for (int i = 0; i < hits.Length; i++)
//            {

//                if (hits[i].distance < bestDst)
//                {
//                    if (CheckParentsForSelectable(hits[i].transform, ref bestSelect))
//                    {
//                        bestNdx = i;
//                    }
//                }
//            }

//            if (bestNdx < 0)
//                return null;

//            return bestSelect;
//        }

//        bool CheckParentsForSelectable(Transform trans, ref SM_ISelectable select)
//        {
//            SM_ISelectable sel = trans.gameObject.GetComponent<SM_ISelectable>();
//            if (sel != null && CanSelect(sel))
//            {
//                select = sel;
//                return true;
//            }

//            for (int i = 0; i < trans.childCount; i++)
//            {
//                Transform newT = trans.GetChild(i);
//                if (CheckParentsForSelectable(newT, ref select))
//                    return true;
//            }

//            return false;
//        }


//        protected virtual bool CanSelect(SM_ISelectable sel)
//        {
//            return true;
//        }
//    }
//}

