using UnityEngine;
using JLib.Sim;
using JLib.Utilities;
using JLib.Unity;
using System.Collections;
using System.Collections.Generic;

namespace Pit
{
    public class MT_SelectedPathEvent : GameEvent
    {
        public Vector3 Where;
        public List<Vector3> Path;

    }


    // TODO path line shouldn't be per-pawn. Can just have one global one


    /// <summary>
    /// This is the mode for when the selected is a combatant on the player's team. 
    /// </summary>
    public class MT_InputModeSelectPath : MT_InputMode
    {
        public override bool CanSelectionChange { get { return false; } }   
        public override bool CheckForHilightChange { get { return false; } }


        Vector3 _lastPointDrawn = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        bool _done = false; // we might have frames between selection and changing mode, so clear it out

        MT_Combatant _who;
        UnityEngine.AI.NavMeshAgent _nma;
        Vector3 _curTerrainPoint;
        Vector3 _prevTerrainPoint;

        UnityEngine.AI.NavMeshPath _path;

        List<Vector3> _pointList = new List<Vector3>(); 

        const float _distBeforeRecalc = 0.5f;

        int _lineId = -1;

        // -------------------------------------------------------------------------
        public override void OnEnable()
        // -------------------------------------------------------------------------
        {
            _path = new UnityEngine.AI.NavMeshPath();

            _who = _params[0] as MT_Combatant;
            if (!Dbg.Assert(_who != null))
            {
                _nma = _who.Pawn.GetComponent<UnityEngine.AI.NavMeshAgent>();
            }
            _done = false;
            base.OnEnable();
        }

        // -------------------------------------------------------------------------
        public override void OnDisable()
        // -------------------------------------------------------------------------
        {

            _lineId = PT_Game.Match.Widgets.HidePath(_lineId);
            _done = false;
            base.OnDisable();
        }


        // -------------------------------------------------------------------------
        public override bool Update()
        // -------------------------------------------------------------------------
        {
            if (base.Update() || _done)
                return true;

            Vector3 newPt = Vector3.zero;
            if (PT_Game.Match.Arena.FindTerrainPointUnderMouse(PT_Game.Match.MainCamera, ref newPt))
            {
                _prevTerrainPoint = _curTerrainPoint;
                _curTerrainPoint = newPt;
            }
            else
            {
                _lineId = PT_Game.Match.Widgets.HidePath(_lineId);
                return false;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Break();
            }

            if (Input.GetMouseButtonDown(0))        // ### TODO : Change to input mapping
            {
                _done = true;
                Events.SendGlobal(new MT_SelectedPathEvent() { Where = _curTerrainPoint, Path = _pointList });
                return true;
            }

            if ((_curTerrainPoint - _prevTerrainPoint).sqrMagnitude > _distBeforeRecalc)   // haven't moved far
            {
                _nma.CalculatePath(_curTerrainPoint, _path);
            }

            switch (_nma.pathStatus)
            {
                case UnityEngine.AI.NavMeshPathStatus.PathComplete:
                    SetLine(_path);
                    break;

                case UnityEngine.AI.NavMeshPathStatus.PathPartial:
                    break;
                case UnityEngine.AI.NavMeshPathStatus.PathInvalid:
                    break;
            }
            return false;
        }


        void SetLine( UnityEngine.AI.NavMeshPath path)
        {
            _pointList.Clear();
            for (int i = 0; i < path.corners.Length; i++)
            {
                _pointList.Add( path.corners[i]);
            }

            _lineId = PT_Game.Match.Widgets.ShowPath(_pointList, _lineId);
        }
    }
}
