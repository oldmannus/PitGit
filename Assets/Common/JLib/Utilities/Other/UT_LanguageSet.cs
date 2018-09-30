using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Game;

namespace JLib.Utilities
{

    // put this somewhere else
    public enum Gender
    {
        Male,
        Female,
        Both,
        Neither,

        Count
    }



    public class UT_Language : UT_TableStringSet
    {
        public enum StringListId
        {
            LastNames,
            MaleNames,
            FemaleNames,
            UngenderedNames,

            Count
        }

        class ListElement
        {
            StringListId Id;
            public List<string> Strings { get; private set; }
            public string GetRandom()
            {
                return IsValid() ? Rng.RandomListElement<string>(Strings) : null;
            }
            public bool IsValid() { return Strings != null; }

            public ListElement(UT_TableStringSet tbl, StringListId id)
            {
                Id = id;
                Strings = tbl.GetStrings(id.ToString());
                if (Strings != null && Strings.Count == 0)      // to avoid confusion, if we have nothing, we get null
                    Strings = null; 
            }
        }


        List<ListElement> _stringLists;



        // more to be added later

        public override void Initialize( string resourceName )
        {
            base.Initialize(resourceName);
            _stringLists = new List<ListElement>();
            for (int i = 0; i < (int)StringListId.Count; i++)
            {
                _stringLists.Add(new ListElement(this, (StringListId)i));
            }
        }

        public string GetRandomString(StringListId id)
        {
            return _stringLists[(int)id].GetRandom();
        }

        void AddNonNull( string val, List<string> list)
        {
            if (val != null)
                list.Add(val);
        }

        public string GenerateSurname()
        {
            return GetRandomString(StringListId.LastNames);
        }

        // --------------------------------------------------------------------------------
        public string GenerateFullName(Gender gender)
        // --------------------------------------------------------------------------------
        {
            string firstName = GenerateFirstName(gender);
            string lastName = GenerateSurname();


            string final = "";
            if (firstName != null)
                final += firstName;

            if (final.Length > 0)
                final += " ";

            final += lastName;

            return final;
        }

        public string GenerateFirstName( Gender gender )
        { 
            string femaleName = GetRandomString(StringListId.FemaleNames);
            string maleName = GetRandomString(StringListId.MaleNames);
            string ungenderedName  = GetRandomString(StringListId.UngenderedNames);

            List<string> possibleNames = new List<string>();
            if (gender != Gender.Female)                                 AddNonNull(femaleName, possibleNames);
            else if (gender != Gender.Male)                              AddNonNull(maleName, possibleNames);
            else if (gender != Gender.Male && gender != Gender.Female)   AddNonNull(ungenderedName, possibleNames);

            // if we have no possible names, then (hopefully it's just because ungendered is empty
            // so throw in both of the others 
            if (possibleNames.Count == 0)
            {
                AddNonNull(femaleName, possibleNames);
                AddNonNull(maleName, possibleNames);
                AddNonNull(ungenderedName, possibleNames);
            }

            return Rng.RandomListElement<string>(possibleNames);
        }
    }
}
