using UnityEngine;
using JLib.Sim;
using JLib.Utilities;
using System.Collections.Generic;

namespace Pit
{
    public class MT_SelectedTerrainPointEvent : GameEvent
    {
        public Vector3 Where;
    }



    /// <summary>
    /// This is the mode for when the selected is a combatant on the player's team. 
    /// </summary>
    public class MT_InputModeSelectTerrainPoint : MT_InputMode
    {
        public override bool CanSelectionChange { get { return false; } }   
        public override bool CheckForHilightChange { get { return false; } }


        Vector3 _lastPointDrawn = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        bool _done = false; // we might have frames between selection and changing mode, so clear it out

        public override void OnEnable()
        {
            _done = false;
            base.OnEnable();
        }



        public override bool Update()
        {
            if (_done)
                return true;

            // returns true if we're done with update this frame
            if (base.Update())
                return true;

            if (Input.GetMouseButtonDown(0))        // ### TODO : Change to input mapping
            {
                Vector3 where = Vector3.zero;
                if (PT_Game.Match.Arena.FindTerrainPointUnderMouse(PT_Game.Match.MainCamera, ref where))
                {
                    Events.SendGlobal(new MT_SelectedTerrainPointEvent() { Where = where });
                    return true;
                }
            }

            return false;
        }
    }
}
