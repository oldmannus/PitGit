using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;

namespace Pit
{
    public class SpeciesDefBuilder : TableObjectBuilder
    {
        public string NameId;
        public string DisplayName;
        public string Language;
        public int StrBase;
        public int StrDelta;
        public int QuickBase;
        public int QuickDelta;
        public int SizeBase;
        public int SizeDelta;
        public int KnowledgeBase;
        public int KnowledgeDelta;
        public int ToughBase;
        public int ToughDelta;
     }



    public class Species : TableObject<string>
    {
        public string DisplayName;

        public int StrBase;
        public int StrDelta;
        public int QuickBase;
        public int QuickDelta;
        public int SizeBase;
        public int SizeDelta;
        public int KnowledgeBase;
        public int KnowledgeDelta;
        public int ToughBase;
        public int ToughDelta;

        public UT_Language Language;


        public Species()
        {

        }

        public override bool BuildFrom(TableObjectBuilder tbuilder)
        {
            SpeciesDefBuilder builder = tbuilder as SpeciesDefBuilder;

            Id = builder.NameId;
            StrBase = builder.StrBase;
            StrDelta = builder.StrDelta;
            QuickBase = builder.QuickBase;
            QuickDelta = builder.QuickDelta;
            SizeBase = builder.SizeBase;
            SizeDelta = builder.SizeDelta;
            KnowledgeBase = builder.KnowledgeBase;
            KnowledgeDelta = builder.KnowledgeDelta;
            ToughBase = builder.ToughBase;
            ToughDelta = builder.ToughDelta;

            DisplayName = builder.DisplayName;

            Language = PT_Game.Data.Languages.GetOrLoadLanguage(builder.Language);

            return true;

        }
    }

    public class PT_DBSpecies : TableObjectManager<SpeciesDefBuilder, Species, string>
    {
    }


}