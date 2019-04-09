
/*
 *  "Details" are for things where one can mouse over or select an item and it show a name, description and icon of the thing,
 */

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;

namespace JLib.Game
{

    // Interface
    public interface IDetailable
    {
        string DisplayName { get; set; }
        string Tooltip { get; set; }
        Sprite Icon { get; set; }
    }

    public class GM_Detailable : IDetailable
    {
        public string DisplayName { get; set; }
        public string Tooltip { get; set; }
        public Sprite Icon { get; set; }
    }
}
