using System;
using System.Collections.Generic;
using System.Text;
using JLib.Utilities;

namespace JLib.Grid
{
    public class AdjacencyGridLayer : GridLayer<IVec3[]>
    {
        UInt64 _obstacleTypeMask = 0;

        public void Initialize(IVec3 size, Grid parent, UInt64 obstacleTypeMask)
        {
            base.Initialize(size, parent);
            _obstacleTypeMask = obstacleTypeMask;
        }

        public void BuildAdjacency(GridLayer<DirectionFlag> baseDirections, GridLayer<UInt64> obstacleLayer)
        {
            _grid.DoLayerOpPos(this, baseDirections, obstacleLayer, (pos, dest, baseDir, obstacles) =>
            {
                // go through adjacent squares and figure out if we can go there
                IVec3[] baseOffsets = Offsets3d.GetOffsets(baseDir);
                DirectionFlag directions = baseDir;
                for (int i = 0; i < baseOffsets.Length; i++)
                {
                    IVec3 newPos = pos + baseOffsets[i];
                    UInt64 obstacleAtNewPos = obstacleLayer.Get(newPos);
                    if ((obstacleAtNewPos & _obstacleTypeMask) == _obstacleTypeMask) // if blocked
                    {
                        directions &= ~Offsets3d.OffsetToDirection(baseOffsets[i]);
                    }
                }

                return Offsets3d.GetOffsets(directions);
            });
        }
    }
}

