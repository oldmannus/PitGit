using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Sim;
using JLib.Utilities;

#if false

Terminology:
    Property - a value that can be modified. Can even include race id and so forth (i.e. spell to turn gnome into troll)
        + Property name
        + Value
    Modifier - something that changes the value of a property. 
    Effect - a thing that that happens
    EffectSet - temporary set of modifiers
    Feat - permanent set of modifiers
        + name 
        + description
        + prereqs

    


#endif


namespace Pit
{
    // TODO: reimplement serialization [Serializable]
    public class BS_Combatant : IPropertied
    {
        public BS_PropertySet   Properties { get { return _properties; } }

        public float            Strength { get { return _properties.GetCurValue(BS_PropertyId.Strength); } }
        public float            Quickness { get { return _properties.GetCurValue(BS_PropertyId.Quickness); } }
        public float            Health { get { return _properties.GetCurValue(BS_PropertyId.Health); } }
        public float            Knowledge { get { return _properties.GetCurValue(BS_PropertyId.Knowledge); } }
        public float            Toughness { get { return _properties.GetCurValue(BS_PropertyId.Toughness); } }
        public float            Size { get { return _properties.GetCurValue(BS_PropertyId.Size); } }
        public float            Cost { get { return _properties.GetCurValue(BS_PropertyId.Cost); } }
        public float            ActionPoints { get { return _properties.GetCurValue(BS_PropertyId.ActionPoints); } }


        public string           FirstName { get; private set; }
        public string           LastName { get; private set; }
        public string           FullName { get; private set; }

        public Class            Class { get; private set; }
        public Gender           Gender { get; private set; }
        public Species          Species { get; private set; }

        public List<BS_Action>   Actions = new List<BS_Action>();
        public List<BS_Modifier> Abilities = new List<BS_Modifier>();


        public BS_Team          Team { get; private set; }

        // TODO : Parameterize these by race & class
        public string           VisualPrefabName = "CharacterModels/DarkSkeleton";
        public string           IconImageName;


        // Internals 
        BS_PropertySet          _properties;

        // our model that we drive around the world

        #region Initialization/creation 


        // ----------------------------------------------------------------------------------------------------
        // this sets our values to something south of the value indicated
        public void Randomize(float startValPerPerson)
        // ----------------------------------------------------------------------------------------------------
        {
            string[] models = new string[]
            {
                "CharacterModels/DarkSkeleton"
                ,"CharacterModels/Cyclops"
                ,"CharacterModels/Golem"
                ,"CharacterModels/Scarecrow"
                ,"CharacterModels/Goblin"
                ,"CharacterModels/Goblin2"
                ,"CharacterModels/Goblin3"
                ,"CharacterModels/Troll"
                ,"CharacterModels/Knight"
                ,"CharacterModels/Trog"
                ,"CharacterModels/HumanFemale"
            };

            VisualPrefabName = Rng.RandomArrayElement<string>(models);


            _properties = new BS_PropertySet();

            BS_KeywordSet set = new BS_KeywordSet();
            set.Add(BS_Keyword.CmbtBaseStat);

            for (int i = (int)BS_PropertyId.START_COMBATANT_BASEPROPERTIES + 1; i < (int)BS_PropertyId.END_COMBATANT_BASEPROPERTIES; i++)
            {
                _properties.AddProperty((BS_PropertyId)i);
                _properties.AddPropertyKeyword((BS_PropertyId)i, BS_Keyword.CmbtBaseStat);

                BS_Property prop = _properties.GetProperty((BS_PropertyId)i);
                Dbg.Assert(prop.Flags.HasFlag(BS_Keyword.CmbtBaseStat));
                Dbg.Assert(prop.Flags.HasFlags(set));
            }

            for (int i = (int)BS_PropertyId.START_COMBATANT_SECONDARYPROPERTIES + 1; i < (int)BS_PropertyId.END_COMBATANT_SECONDARYPROPERTIES; i++)
            {
                _properties.AddProperty((BS_PropertyId)i);
            }


            // pick a random race. 
            // TODO add probabilities?
            Species = PT_Game.Data.Species.GetRandom();
            Gender = (Gender)Rng.RandomInt((int)Gender.Count);

            LastName = Species.Language.GenerateSurname();
            FirstName = Species.Language.GenerateFirstName(Gender);
            FullName = FirstName == null ? LastName : (FirstName + " " + LastName);

            IconImageName = GetRandomIconName();

            _properties.SetPropertyBase(BS_PropertyId.Strength, Rng.RandomFloat(PT_Game.Data.Consts.Character_MinStat, PT_Game.Data.Consts.Character_MaxStat));
            _properties.SetPropertyBase(BS_PropertyId.Quickness, Rng.RandomFloat(PT_Game.Data.Consts.Character_MinStat, PT_Game.Data.Consts.Character_MaxStat));
            _properties.SetPropertyBase(BS_PropertyId.Size, Rng.RandomFloat(PT_Game.Data.Consts.Character_MinStat, PT_Game.Data.Consts.Character_MaxStat));
            _properties.SetPropertyBase(BS_PropertyId.Knowledge, Rng.RandomFloat(PT_Game.Data.Consts.Character_MinStat, PT_Game.Data.Consts.Character_MaxStat));
            _properties.SetPropertyBase(BS_PropertyId.Toughness, Rng.RandomFloat(PT_Game.Data.Consts.Character_MinStat, PT_Game.Data.Consts.Character_MaxStat));
            _properties.SetPropertyBase(BS_PropertyId.Discipline, Rng.RandomFloat(PT_Game.Data.Consts.Character_MinStat, PT_Game.Data.Consts.Character_MaxStat));

            GenerateAbilities();
            GenerateActions();

            RecomputeActionPoints();
            RecomputeCost();
        }

        string GetRandomIconName()
        {
            return "Anime" + Rng.RandomInt(1, 13);        // TODO Fix icons so they key off race & class
        }


        void SanityCheck()
        {
            BS_Property prop = _properties.GetProperty(BS_PropertyId.Health);
            Dbg.Assert(prop.Flags.HasFlag(BS_Keyword.CmbtBaseStat) == false);
        }
        // ----------------------------------------------------------------------------------------------------
        void RecomputeCost()
        // ----------------------------------------------------------------------------------------------------
        {
            float cost = (Strength + Quickness + Size + Knowledge + Toughness); // TODO : make cost algorithm more robust

            _properties.SetPropertyBase(BS_PropertyId.Cost, cost);  
        }

        // ----------------------------------------------------------------------------------------------------
        void RecomputeActionPoints()
        // ----------------------------------------------------------------------------------------------------
        {
            float cost = (Strength + Quickness + Size + Knowledge + Toughness); // TODO : make cost algorithm more robust. it's deeply silly now

            _properties.SetPropertyBase(BS_PropertyId.ActionPoints, cost);
        }


        // ----------------------------------------------------------------------------------------------------
        public void SetTeam(BS_Team team)
        // ----------------------------------------------------------------------------------------------------
        {
            Team = team;
        }

        // ----------------------------------------------------------------------------------------------------
        void AddRaceAbilities()
        // ----------------------------------------------------------------------------------------------------
        {

        }
        // ----------------------------------------------------------------------------------------------------
        void AddClassAbilities()
        // ----------------------------------------------------------------------------------------------------
        {

        }

        void GenerateAbilities()
        {
            Abilities.Clear();
            AddRaceAbilities();
            AddClassAbilities();
        }

        // ----------------------------------------------------------------------------------------------------
        void GenerateActions()
        // ----------------------------------------------------------------------------------------------------
        {
            Actions.Clear();

            // TODO : In the future, build actions from race + class + whatever
            Actions.Add(new BS_ActionMove());
            //ActionTemplates.Clear();

            //BS_MeleeAttackAction maa = new BS_MeleeAttackAction(1, 5);
            //ActionTemplates.Add(maa);
        }



        #endregion



        #region Status functions

        public float GetPropertyRatio(BS_PropertyId id)
        {
            BS_Property iprop = _properties.GetProperty(id);

            float curVal = iprop.CurValue;
            float baseVal = iprop.BaseValue;

            return (baseVal == 0) ? 0 : curVal / baseVal;
        }

        public float GetPropertyRatioClamped(BS_PropertyId id)
        {
            return Mathf.Clamp01(GetPropertyRatio(id));
        }

        #endregion




#if false
        #region not used yet

        // game loop calls
        public void BeginTurn() { APRemaining = Current.ActionPoints; }
        public void StartAction(MT_ActionInstance action) { }
        public void EndAction(MT_ActionInstance action) { }
        public void EndTurn() { }

        public BS_Combatant FindNearestOpponent()
        {
            return null;
        }

        // this is the meat of the AI. or maybe the player too? 
        public MT_ActionInstance SelectAction()
        {
            //BS_Combatant target = FindNearestOpponent();
            //float distance = (target.Position - Position).Length;
            //if (distance < 3)       // #### TODO HACK HACK FIX FOR ACTION RANGE
            //{

            // //   return new MT_MeleeAttackActionInstance( new MT_)
            //}
            //else
            //{

            //}

            return null;


        }

        #endregion
#endif


    }
}
