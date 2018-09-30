using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;

namespace JLib.Game
{
    public interface IDisplayable
    {
        GM_DisplayInfo About { get; }
        // todo: icon
        // todo: tooltip
    }

    public interface IDisplayInfo 
    {
        string DisplayName { get; set; }
        string Tooltip { get; set; }
    }

    public class GM_DisplayInfo : IDisplayInfo
    {
        public string DisplayName { get; set; }
        public string Tooltip { get; set; }
        public Sprite Icon { get; set; }


    }
}
