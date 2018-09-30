using UnityEngine;
using JLib.Sim;
using JLib.Utilities;

namespace Pit
{
    /// <summary>
    /// This this basically blocks input
    /// TODO : Make it so that UI is disabled too
    /// </summary>
    public class MT_InputModeNone : MT_InputMode
    {
        public override bool EscapeReturnsToStart { get { return false; } }
        public override bool CanSelectionChange { get { return false; } }   
        public override bool CheckForHilightChange { get { return false; } }

    }

}
