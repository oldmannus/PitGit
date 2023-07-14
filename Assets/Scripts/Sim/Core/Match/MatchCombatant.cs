using UnityEngine;
using Pit.Utilities;


namespace Pit.Sim
{
    /// <summary>
    /// The match combatant is a wrapper around the base combatant and the pawn
    /// This gives us a place for stuff that is match-specific (i.e. doesn't need to be in Combatant)
    /// and game-specific (so it doesn't have to be in SM_Pawn). 
    /// This includes AI, various handlers for event processing and so forth
    /// </summary>
    public class MatchCombatant
    {
        public MatchTeam      Team {  get { return _team; } }
        public Pawn      Pawn { get { return _pawn; } }
        public Combatant Base { get { return _base; } }

        public bool IsSelected { get { return _pawn.IsSelected; } }
        public bool IsOut { get; private set; }// has this guy been knocked out, seriously injured, dead, etc. Out forever

        public float Initiative = 0;

        public void ComputeInitiative()
        {
            Initiative = Rng.RandomFloat(); // TODO more robust initiative values
        }


        MatchTeam _team;
        Combatant _base;
        Pawn _pawn;

        // TODO in the future, add modifiers on the base
        public float Dodge => _base.Dodge;
        public float Accuracy => _base.AtkAcc;
        public float AtkDmg => _base.AtkDmg;
        public float Armor => _base.Armor;

        int _hitPoints; 

        public void Initialize(Combatant cmbt, MatchTeam t)
        {
            _team = t;
            _pawn = null;
            _base = cmbt;
            IsOut = false;

            _hitPoints = _base.HitPoints;
        }

        public void TakeDamage(int dmg)
        {
            _hitPoints -= dmg;
            IsOut = _hitPoints <= 0;
        }


        // ---------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------
        public void PlaceInArena(MT_ArenaSpawnPoint sp, Transform attachPt)
        {
            // V2 for all
            //GameObject go = null;   //### PJS TODO FIX for v2 Game.Resources.InstantiateFromResource(Base.VisualPrefabName, attachPt, sp.transform.position, sp.transform.rotation);
            //Dbg.Assert(go != null, "ERROR: Failed to instantiate from resource: " + Base.VisualPrefabName);

            //_pawn = go.GetComponent<Pawn>();
            //if (Dbg.Assert(_pawn != null, "DATA ERROR: Loaded prefab has no pawn component attached: " + Base.VisualPrefabName))
            //    return;

            //_pawn.SetName(Base.FullName);
            //_pawn.SetGameParent(this);

            // TO DO remove selection projector connecting in combatant.

            throw new System.NotImplementedException();
            // v2 removal
            //GameObject proj = Game.Resources.InstantiateFromResource("CharacterModels/SelectionProjector", go.transform, Vector3.zero, Quaternion.identity);
            //proj.transform.localPosition = Vector3.zero;
            //_pawn.SetSelection(proj.GetComponent<SM_SelectableComponent>());

        }

        public void Shutdown()
        {
            _base = null;
            _pawn = null;
            _team = null;
        }
    }
}
