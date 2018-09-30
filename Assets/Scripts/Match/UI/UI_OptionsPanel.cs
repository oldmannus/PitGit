using UnityEngine;
using System.Collections;

namespace Pit
{

    public class UI_OptionsPanel : PT_MonoBehaviour
    {

        public void OnEndTurn()
        {
            // TODO : warn if lots of AP are left
            PT_Game.Match.EndTurn();
        }

        public void OnConcedeMatch()
        {

        }

        public void OnStats()
        {

        }

        public void OnOptions()
        {

        }
    }
}