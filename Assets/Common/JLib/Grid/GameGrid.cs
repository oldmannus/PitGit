using System;
using System.Collections.Generic;
using System.Text;

namespace JLib.Grid
{
    public class GameGrid : NavigationGrid
    {
        [Flags]
        enum TileProps
        {
            StairsUp = 1<<0,
            StairsDown = 1<<1,

        }
    }
}
