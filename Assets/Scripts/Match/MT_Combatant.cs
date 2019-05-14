using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;
using JLib.Sim;
using JLib.Game;

namespace Pit
{
    /// <summary>
    /// The match combatant is a wrapper around the base combatant and the pawn
    /// This gives us a place for stuff that is match-specific (i.e. doesn't need to be in BS_Combatant)
    /// and game-specific (so it doesn't have to be in SM_Pawn). 
    /// This includes AI, various handlers for event processing and so forth
    /// </summary>
    public class MT_Combatant
    {
        public MT_Team      Team {  get { return _team; } }
        public SM_Pawn      Pawn { get { return _pawn; } }
        public BS_Combatant Base { get { return _base; } }

        public bool IsSelected { get { return _pawn.IsSelected; } }
        public bool IsOut { get; private set; }// has this guy been knocked out, seriously injured, dead, etc. Out forever




        MT_Team _team;
        BS_Combatant _base;
        SM_Pawn _pawn;


        public void Initialize(BS_Combatant cmbt, MT_Team t)
        {
            _team = t;
            _pawn = null;
            _base = cmbt;
            IsOut = false;
        }

        // ---------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------
        public void PlaceInArena(MT_ArenaSpawnPoint sp, Transform attachPt)
        {
            GameObject go = GM_Game.Resources.InstantiateFromResource(Base.VisualPrefabName, attachPt, sp.transform.position, sp.transform.rotation);
            Dbg.Assert(go != null, "ERROR: Failed to instantiate from resource: " + Base.VisualPrefabName);

            _pawn = go.GetComponent<SM_Pawn>();
            if (Dbg.Assert(_pawn != null, "DATA ERROR: Loaded prefab has no pawn component attached: " + Base.VisualPrefabName))
                return;

            _pawn.SetName(Base.FullName);
            _pawn.SetGameParent(this);

            // TODO remove selection projector connecting in combatant.

            GameObject proj = GM_Game.Resources.InstantiateFromResource("CharacterModels/SelectionProjector", go.transform, Vector3.zero, Quaternion.identity);
            proj.transform.localPosition = Vector3.zero;
            _pawn.SetSelection(proj.GetComponent<SM_SelectableComponent>());

        }

        public void Shutdown()
        {
            _base = null;
            _pawn = null;
            _team = null;
        }
    }
}
