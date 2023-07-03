using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using JLib.Utilities;

namespace JLib.Grid
{
    public class WorldGrid : GameGrid
    {
        FVec3 _minWC;
        FVec3 _maxWC;
        FVec3 _centerWC;

        FVec3 _tileSizeWC;    // how big each tile is in world coordinates

        /*
         * SetCenter/SetBounds
         * 
         * WorldToGrid
         * GridToWorld (bottom, corners, etc
         * Cells from world raycast
         * 
         */

        public void Initialize(FVec3 tileSize, FVec3 corner, IVec3 numTiles)
        {
            base.Initialize(numTiles);

            FVec3 size = numTiles * tileSize;
            _minWC = corner;
            _maxWC = corner + size;
            _centerWC = _minWC + (size / 2.0f);

            _tileSizeWC = tileSize;
        }

        /// <summary>
        /// This will return coordinates off the grid possibly
        /// </summary>
        /// <param name="worldCoord"></param>
        /// <returns></returns>
        public IVec3 WorldToGrid(FVec3 worldCoord)
        {
            return new IVec3( (worldCoord-_minWC)/_tileSizeWC);
        }

        public void GridToWorld(IVec3 gridPos, out FVec3 min, out FVec3 max)
        {
            min = GridToWorldMin(gridPos);
            max = GridToWorldMax(gridPos);
        }

        public FVec3 GridToWorldMax(IVec3 gridPos)
        {
            return GridToWorldMin(gridPos + IVec3.One);
        }

        public void SnapToGrid(ref FVec3 pos)
        {
            pos = GridToWorldMin(WorldToGrid(pos));
            pos.x += _tileSizeWC.x * 0.5f;
            pos.z += _tileSizeWC.z * 0.5f;
            // TODO: make orientable
        }

        public FVec3 SnapToGrid(FVec3 pos)
        {
            pos = GridToWorldMin(WorldToGrid(pos));
            pos.x += _tileSizeWC.x * 0.5f;
            pos.z += _tileSizeWC.z * 0.5f;
            return pos;
            // TODO: make orientable
        }


        public FVec3 GridToWorldMin(IVec3 gridPos)
        {
            FVec3 worldPos = gridPos * _tileSizeWC;

            worldPos += _minWC;
            return worldPos;
        }

        /// <summary>
        /// This clamps to positions on the grid
        /// </summary>
        /// <param name="worldCoord"></param>
        /// <returns></returns>
        public IVec3 WorldToGridClamped(FVec3 worldCoord)
        {
            return IVec3.Clamp0To(WorldToGrid(worldCoord), GridTileCount);
        }
    }

}
