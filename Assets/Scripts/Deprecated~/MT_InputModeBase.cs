//using UnityEngine;
//using Pit.Utilities;

//namespace Pit
//{

//    /// <summary>
//    /// This is the mode for when the selected is a combatant on the player's team. 
//    /// </summary>
//    public class MT_InputModeBase: MT_InputMode
//    {
//        Vector3 _lastPointDrawn = new Vector3(float.MinValue, float.MinValue, float.MinValue);

//        //protected override void OnSelectionChanged(SM_SelectionChangedEvent ev)
//        //{
//        //    //### TODO check to see if this is someone that the local player can muck with
//        //    //bool isHumanTeam = false;

//        //    //// if we are de-selecting, or selecting non-human team then we don't care
//        //    //if (ev.NewWho.Count == 0 && isHumanTeam == false)
//        //    //{
//        //    //    _inputMgr.QueueInputMode<MT_InputModeBase>();
//        //    //}
//        //}

//        public override bool Update()
//        {
//            // returns true if we're done with update this frame
//            if (base.Update())
//                return true;

//            UpdateHilight(Game.Match.MainCamera);

//            if (Input.GetMouseButtonDown(0))
//            {
//                SM_ISelectable sel = CheckForObjectUnderMouse(Game.Match.MainCamera);
//                if (sel != null)
//                {
                    
//                }
//            }

//            MT_Combatant c = Game.UI.Match.SelectedPCCombatant;
//            Dbg.Assert(c != null);

//            if (c.IsOut == false)
//            { 
//                int numKeyDown = GetNumberKeyDown();
//                if (numKeyDown >= 0)
//                {
//                    if (c.Base.Actions != null && numKeyDown <= c.Base.Actions.Count)
//                    {
//                        Game.Match.StartAction(c.Base.Actions[numKeyDown - 1], c); // TODO : replace with event?
//                        return true;
//                    }
//                }
//            }

//            return false;
//        }


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
//            //SM_ISelectable obj = CheckForObjectUnderMouse(cam);
//            //if (obj == null)
//            //{
//            //    UnHilightAll();
//            //}
//            //else if (obj.IsHilighted == false) // the object wasn't hilighted before
//            //{
//            //    UnHilightAll();     // clear out old set

//            //    // if this object is a member of a set, then hilight all of them
//            //    if (obj.IsSelected && _selected.Count > 1)
//            //    {
//            //        foreach (var v in _selected)
//            //        {
//            //            SetHilighted(v);
//            //        }
//            //    }
//            //    else
//            //    {
//            //        SetHilighted(obj);
//            //    }
//            //}
//            // otherwise we were already hilighted and have nothing to do. 
//        }
//    }



//    //        if (Input.GetMouseButtonDown(0))
//    //        {
//    //            // otherwise, we clicked on a point, which should cause the player to move
//    //            // #### TODO: issue command to current target to move to location!!!


//    //            // didn't do some input already, look for point on ground
//    //            Vector3 where = Vector3.zero;
//    //            if (PT_Game.Match.Arena.FindTerrainPointUnderMouse(PT_Game.Match.MainCamera, ref where))
//    //            {

//    //                if ((where - _lastPointDrawn).sqrMagnitude > PT_Game.Data.Consts.DistNavDrawUpdate)
//    //                {
//    //                    _lastPointDrawn = where;

//    //                    SM_ISelectable sel = PT_Game.Match.SelectionMgr.GetFirstSelected();

//    //                    if (sel is SM_Pawn)
//    //                    {
//    //                        MT_MoveToEvent ev = new MT_MoveToEvent();
//    //                        ev.Who = sel as SM_Pawn;
//    //                        ev.Start = ev.Who.transform.position;
//    //                        ev.End = where;
//    //                        Events.SendGlobal(ev);
//    //                    }
//    //                    else
//    //                    {
//    //                        Dbg.LogError("Err, trying to give orders to non-pawn");
//    //                    }

//    //                    //Events.SendGlobal(new MT_DrawNavPathBetween())
//    //                }


//    //                return true;
//    //            }
//    //        }

//    //        return false;// not changed. yet
//    //    }
//    //}


//}
