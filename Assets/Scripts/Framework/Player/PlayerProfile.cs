using System;
using System.Collections.Generic;
using System.Text;
using Dbg = Pit.Utilities.Dbg;

namespace Pit.Framework
{
    [Serializable]
    public class GM_Profile 
    {
        public string ProfileName { get { return _profileName; } set { _profileName = value; } }
        public string PlayerName { get { return _playerName; } set { _playerName = value; } }

        string _profileName;
        string _playerName;



        protected virtual bool Write()
        {
            Dbg.Assert(string.IsNullOrEmpty(_profileName) == false);


            //### do stuff

            return true;

        }

        protected virtual bool Read(string profileName)
        {

            // ### do stuff

            return true;


        }
        
    }
}
