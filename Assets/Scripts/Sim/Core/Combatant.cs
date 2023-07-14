namespace Pit.Sim
{
    public struct AttackModifier
    {
        public float Accuracy;      // 0-1 of likelihood to hit
        public float MinDmg;
        public float MaxDmg;        // for now, linear
        public float Piercing;      // how much to reduce dmg resist
        public float CritChance;    // 0-1 likelihood
        public float CritMult;      // how much more dmg crits do

        public float PreHitTime;    // time between attack start and attack hit
        public float PostHitTime;   // time after attack hit to being ready to swing again
    }

    public struct DefenseModifier
    {
        public float Dodge;     // opposes Accuracy, 0-1 
        public float DmgResist; // reduces dmg by this much
        public float Health;    
    }

    // combatant is a member of a team 
    public class Combatant
    {
        public AttackModifier Attack;
        public DefenseModifier Defense;
        public Team Team { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }

        int _hireCostHack; // TODO fix this!
        public int ComputeHireCost() 
        {
            return _hireCostHack;
 //###           return (int)(AtkDmg + AtkAcc + Dodge + Armor + HitPoints);  // TODO fix hire cost algorithm
        }

        #region Combat Stats
        public int AtkDmg { get; private set; } 
        public float AtkAcc { get; private set; }
        public float Dodge { get; private set; }
        public float Armor { get; private set; }
        public int HitPoints { get; private set; }
        #endregion
        public Combatant(Team t, SimCreationParams parms, int hireBudget)
        {
            Team = t;
            // TODO make combatants have values
            //        HireCost = ComputeHireCost(); // (int)(hireBudget+0.5f);
            _hireCostHack = hireBudget;
        }

        //public Class            Class { get; private set; }
        //public Gender           Gender { get; private set; }
        //public Species          Species { get; private set; }
    }
}


//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//#if false

//Terminology:
//    Property - a value that can be modified. Can even include race id and so forth (i.e. spell to turn gnome into troll)
//        + Property name
//        + Value
//    Modifier - something that changes the value of a property. 
//    Effect - a thing that that happens
//    EffectSet - temporary set of modifiers
//    Feat - permanent set of modifiers
//        + name 
//        + description
//        + prereqs

//#endif


//namespace Pit.Sim
//{
//    // TODO: reimplement serialization [Serializable]
//    public class Combatant
//    {
//        //public BS_PropertySet   Properties { get { return _properties; } }
//        //public float            Strength { get { return _properties.GetCurValue(BS_PropertyId.Strength); } }
//        //public float            Quickness { get { return _properties.GetCurValue(BS_PropertyId.Quickness); } }
//        //public float            Health { get { return _properties.GetCurValue(BS_PropertyId.Health); } }
//        //public float            Knowledge { get { return _properties.GetCurValue(BS_PropertyId.Knowledge); } }
//        //public float            Toughness { get { return _properties.GetCurValue(BS_PropertyId.Toughness); } }
//        //public float            Size { get { return _properties.GetCurValue(BS_PropertyId.Size); } }
//        //public float            Cost { get { return _properties.GetCurValue(BS_PropertyId.Cost); } }
//        //public float            ActionPoints { get { return _properties.GetCurValue(BS_PropertyId.ActionPoints); } }

//        public string FirstName { get; private set; }
//        public string LastName { get; private set; }
//        public string FullName { get; private set; }

//        //public Class            Class { get; private set; }
//        //public Gender           Gender { get; private set; }
//        //public Species          Species { get; private set; }

//        //public List<BS_Action>   Actions = new List<BS_Action>();
//        //public List<BS_Modifier> Abilities = new List<BS_Modifier>();


//        public Team Team { get; private set; }

//        // TODO : Parameterize these by race & class
//        public string VisualPrefabName = "CharacterModels/DarkSkeleton";
//        public string IconImageName;


//        // Internals 
//        //        BS_PropertySet          _properties;

//        // our model that we drive around the world

//        #region Initialization/creation 


//        // ----------------------------------------------------------------------------------------------------
//        // this sets our values to something south of the value indicated
//        public void Randomize(float startValPerPerson)
//        // ----------------------------------------------------------------------------------------------------
//        {
//            string[] models = new string[]
//            {  // TO DO fix hard coded model selection
//                "Character/LowPoly_Lancer/Lancer"
//                //"CharacterModels/DarkSkeleton"
//                //,"CharacterModels/Cyclops"
//                //,"CharacterModels/Golem"
//                //,"CharacterModels/Scarecrow"
//                //,"CharacterModels/Goblin"
//                //,"CharacterModels/Goblin2"
//                //,"CharacterModels/Goblin3"
//                //,"CharacterModels/Troll"
//                //,"CharacterModels/Knight"
//                //,"CharacterModels/Trog"
//                //,"CharacterModels/HumanFemale"
//            };

//            VisualPrefabName = Rng.RandomArrayElement<string>(models);


//            _properties = new BS_PropertySet();

//            BS_KeywordSet set = new BS_KeywordSet();
//            set.Add(BS_Keyword.CmbtBaseStat);

//            for (int i = (int)BS_PropertyId.START_COMBATANT_BASEPROPERTIES + 1; i < (int)BS_PropertyId.END_COMBATANT_BASEPROPERTIES; i++)
//            {
//                _properties.AddProperty((BS_PropertyId)i);
//                _properties.AddPropertyKeyword((BS_PropertyId)i, BS_Keyword.CmbtBaseStat);

//                BS_Property prop = _properties.GetProperty((BS_PropertyId)i);
//                Dbg.Assert(prop.Flags.HasFlag(BS_Keyword.CmbtBaseStat));
//                Dbg.Assert(prop.Flags.HasFlags(set));
//            }

//            for (int i = (int)BS_PropertyId.START_COMBATANT_SECONDARYPROPERTIES + 1; i < (int)BS_PropertyId.END_COMBATANT_SECONDARYPROPERTIES; i++)
//            {
//                _properties.AddProperty((BS_PropertyId)i);
//            }


//            // pick a random race. 
//            // TODO add probabilities?
//            Species = Game.Data.Species.GetRandom();
//            Gender = (Gender)Rng.RandomInt((int)Gender.Count);

//            LastName = Species.Language.GenerateSurname();
//            FirstName = Species.Language.GenerateFirstName(Gender);
//            FullName = FirstName == null ? LastName : (FirstName + " " + LastName);

//            IconImageName = GetRandomIconName();

//            // ### PJS TODO removed for v2
//            //_properties.SetPropertyBase(BS_PropertyId.Strength, Rng.RandomFloat(Game.Data.Consts.Character_MinStat, Game.Data.Consts.Character_MaxStat));
//            //_properties.SetPropertyBase(BS_PropertyId.Quickness, Rng.RandomFloat(Game.Data.Consts.Character_MinStat, Game.Data.Consts.Character_MaxStat));
//            //_properties.SetPropertyBase(BS_PropertyId.Size, Rng.RandomFloat(Game.Data.Consts.Character_MinStat, Game.Data.Consts.Character_MaxStat));
//            //_properties.SetPropertyBase(BS_PropertyId.Knowledge, Rng.RandomFloat(Game.Data.Consts.Character_MinStat, Game.Data.Consts.Character_MaxStat));
//            //_properties.SetPropertyBase(BS_PropertyId.Toughness, Rng.RandomFloat(Game.Data.Consts.Character_MinStat, Game.Data.Consts.Character_MaxStat));
//            //_properties.SetPropertyBase(BS_PropertyId.Discipline, Rng.RandomFloat(Game.Data.Consts.Character_MinStat, Game.Data.Consts.Character_MaxStat));

//            GenerateAbilities();
//            GenerateActions();

//            RecomputeActionPoints();
//            RecomputeCost();
//        }

//        string GetRandomIconName()
//        {
//            return "Anime" + Rng.RandomInt(1, 13);        // TODO Fix icons so they key off race & class
//        }


//        void SanityCheck()
//        {
//            BS_Property prop = _properties.GetProperty(BS_PropertyId.Health);
//            Dbg.Assert(prop.Flags.HasFlag(BS_Keyword.CmbtBaseStat) == false);
//        }
//        // ----------------------------------------------------------------------------------------------------
//        void RecomputeCost()
//        // ----------------------------------------------------------------------------------------------------
//        {
//            float cost = (Strength + Quickness + Size + Knowledge + Toughness); // TODO : make cost algorithm more robust

//            _properties.SetPropertyBase(BS_PropertyId.Cost, cost);
//        }

//        // ----------------------------------------------------------------------------------------------------
//        void RecomputeActionPoints()
//        // ----------------------------------------------------------------------------------------------------
//        {
//            float cost = (Strength + Quickness + Size + Knowledge + Toughness); // TODO : make cost algorithm more robust. it's deeply silly now

//            _properties.SetPropertyBase(BS_PropertyId.ActionPoints, cost);
//        }


//        // ----------------------------------------------------------------------------------------------------
//        public void SetTeam(Team team)
//        // ----------------------------------------------------------------------------------------------------
//        {
//            Team = team;
//        }

//        // ----------------------------------------------------------------------------------------------------
//        void AddRaceAbilities()
//        // ----------------------------------------------------------------------------------------------------
//        {

//        }
//        // ----------------------------------------------------------------------------------------------------
//        void AddClassAbilities()
//        // ----------------------------------------------------------------------------------------------------
//        {

//        }

//        void GenerateAbilities()
//        {
//            Abilities.Clear();
//            AddRaceAbilities();
//            AddClassAbilities();
//        }

//        // ----------------------------------------------------------------------------------------------------
//        void GenerateActions()
//        // ----------------------------------------------------------------------------------------------------
//        {
//            Actions.Clear();

//            // TODO : In the future, build actions from race + class + whatever
//            Actions.Add(new BS_ActionMove());
//            //ActionTemplates.Clear();

//            //BS_MeleeAttackAction maa = new BS_MeleeAttackAction(1, 5);
//            //ActionTemplates.Add(maa);
//        }



//        #endregion



//        #region Status functions

//        public float GetPropertyRatio(BS_PropertyId id)
//        {
//            BS_Property iprop = _properties.GetProperty(id);

//            float curVal = iprop.CurValue;
//            float baseVal = iprop.BaseValue;

//            return (baseVal == 0) ? 0 : curVal / baseVal;
//        }

//        public float GetPropertyRatioClamped(BS_PropertyId id)
//        {
//            return Mathf.Clamp01(GetPropertyRatio(id));
//        }

//        #endregion




//#if false
//        #region not used yet

//        // game loop calls
//        public void BeginTurn() { APRemaining = Current.ActionPoints; }
//        public void StartAction(MT_ActionInstance action) { }
//        public void EndAction(MT_ActionInstance action) { }
//        public void EndTurn() { }

//        public Combatant FindNearestOpponent()
//        {
//            return null;
//        }

//        // this is the meat of the AI. or maybe the player too? 
//        public MT_ActionInstance SelectAction()
//        {
//            //Combatant target = FindNearestOpponent();
//            //float distance = (target.Position - Position).Length;
//            //if (distance < 3)       // #### TODO HACK HACK FIX FOR ACTION RANGE
//            //{

//            // //   return new MT_MeleeAttackActionInstance( new MT_)
//            //}
//            //else
//            //{

//            //}

//            return null;


//        }

//        #endregion
//#endif


//    }
//}


//namespace Pit.Sim
//{
//    public class Combatant
//    { }
//}
