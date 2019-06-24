using System;
using System.Collections.Generic;
using System.Text;

namespace JLib.Grid
{
    /// <summary>
    /// How much it costs by default to enter a square
    /// </summary>
    public class CostGridLayer : GridLayer<float>
    {
        public UInt64 LayersAffected;
    }


    // in the future, this might be more complex to compute the cost to enter based on different directions
    public class AdjacentCostGridLayer : GridLayer<float[]>
    {
        public void BuildCosts(AdjacencyGridLayer adjacentLayer, CostGridLayer costLayer)
        {
            _grid.DoLayerOpPos(this, adjacentLayer, (pos, dest, dir) =>
            {
                dest = new float[dir.Length];

                for (int i = 0; i < dest.Length; i++)
                {
                    if (Offsets3d.IsDiagonal(dir[i]))
                    {
                        dest[i] = 1.41f * costLayer.Get(pos + dir[i]);  // sqrt(2)
                    }
                    else
                    {
                        dest[i] = 1.0f * costLayer.Get(pos + dir[i]);
                    }
                }

                return dest;
            });
        }
    }
}
