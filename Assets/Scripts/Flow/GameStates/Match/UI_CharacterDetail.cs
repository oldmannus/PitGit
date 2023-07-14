//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//namespace Pit.Flow
//{

//    public class UI_CharacterDetail : MonoBehaviour
//    {
//        [SerializeField]
//        Text _nameText = null;
//        [SerializeField]
//        Text _classSpecies = null;
//        [SerializeField]
//        UI_PropertyListPanel _baseStats = null;

//        Combatant _combatant;

//        // Use this for initialization
//        protected override void Start()
//        {
//            Dbg.Assert(_baseStats != null);
//            Dbg.Assert(_nameText != null);
//            Dbg.Assert(_classSpecies != null);
//            Events.AddGlobalListener<SM_SelectionChangedEvent>(OnSelectionSet);
//        }
//        protected override void OnDestroy()
//        {
//            Events.AddGlobalListener<SM_SelectionChangedEvent>(OnSelectionSet);
//        }

//        void OnSelectionSet(SM_SelectionChangedEvent ev )
//        {
//            SM_Pawn pawn = ev.Get<SM_Pawn>();
//            if (pawn != null)
//            {
//                Combatant comb = pawn.GameParent as Combatant;
//                if (comb != null)
//                {
//                    SetCombatant(comb);
//                }
//            }
//        }



//        // Update is called once per frame
//        protected override void Update()
//        {

//        }

//        public void SetCombatant(Combatant cmbt)
//        {
//            _combatant = cmbt;
//            _baseStats.SetPropertySet(_combatant.Properties);
//            RefreshUI();
//        }


//        public void RefreshUI()
//        {
//            UN.SetText(_nameText, _combatant.FullName);
//            UN.SetText(_classSpecies, string.Format("{0} {1}", _combatant.Species.DisplayName, _combatant.Class.Id));
//        }
//    }
//}