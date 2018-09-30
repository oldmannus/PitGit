#if false
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Sim;
using JLib.Utilities;


namespace Pit
{

    public enum MT_ArenaTileLayer
    {
        Base,
        Decoration,
        Scar,
        Transient
    }

    [Flags]
    public enum MT_ArenaTileStatus
    {
        None,
        BlocksSight,
        BlocksMovement,
    }


    /// <summary>
    /// Has enough information to build an arena
    /// </summary>
    public class MT_ArenaDescriptor
    {
        public IVec3 Dimensions;
        public string Name;
        public float TileSizeInWC;


        //public delegate bool TileInitCallback(MT_ArenaDescriptor desc,);
        //public TileInitCallback InitTileCallback;

        public virtual bool InitTileCallback(MT_ArenaTile tile)
        {
            // TODO: add stuff here
            return true;
        }

        public virtual bool IsWithinBounds( IVec3 loc)
        {
            if (loc.x < 0 || loc.y < 0 || loc.z < 0)
                return false;

            if (loc.x >= Dimensions.x || loc.y >= Dimensions.y || loc.z >= Dimensions.z)
                return false;

            return true;
        }


        
        public bool GetSpawnPoint( int teamNdx, int cbtIndex, out IVec3 where)
        {
            //TODO: make spawn point be more map based
            // fix all around.

            if (teamNdx == 0)
                where = new IVec3(0, 0, 0);
            else
                where = new IVec3(Dimensions.x - 1, Dimensions.y - 1, Dimensions.z - 1);

            return true;
        }
    }


    public class MT_ArenaTileVisual
    {
        public object PlatformVisual;
        public MT_ArenaTileLayer Layer;
    }


    public class MT_ArenaTile
    {
        public List<MT_ArenaTileVisual> Visuals;
        public IVec3 Position;

        public void Sort()
        {
            Visuals.Sort((x, y) => ((int)x.Layer).CompareTo((int)y.Layer));
        }

    }


    public class SetMapEvent : GameEvent
    {
        public MT_Arena Arena;
    }




    /// <summary>
    /// This is a playspace based around a 3d grid of tiles. The tiles can have lists of visual objects within
    /// </summary>
    public class MT_Arena : SM_Space
    {
        MT_ArenaDescriptor _desc = null;
        //MT_ArenaTile[,,] _tiles;


        // Tile size in world is how many meters per square. Rest of sim works in meters
        public IVec3 Dimensions { get { return _desc.Dimensions; } }
        public MT_ArenaDescriptor Description {  get { return _desc; } }



        // -------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates the arena
        /// </summary>
        /// <param name="data"></param>
        public void Configure(object data)
        // -------------------------------------------------------------------------------------------
        {
            _desc = data as MT_ArenaDescriptor;

#if false
            _tiles = new MT_ArenaTile[(int)Dimensions.x, (int)Dimensions.y, (int)Dimensions.z];
            this.Bounds.Center = FVec3.zero;
            this.Bounds.Size = Dimensions.ToFVec3();



            MT_ArenaTile curTile = new MT_ArenaTile();
            for (int x = 0; x < Dimensions.x; x++)
            {
                for (int y = 0; y < Dimensions.y; y++)
                {
                    for (int z = 0; z < Dimensions.z; z++)
                    {
                        if (curTile == null)
                            curTile = new MT_ArenaTile();

                        if (_desc.InitTileCallback(curTile))
                        {
                            _tiles[x, y, z] = curTile;
                            curTile = null;
                        }

                    }
                }
            }
#endif

            Events.SendGlobal(new SetMapEvent() { Arena = this });
        }



        void SortVisuals(IVec3 tileNdx)
        {
//###            MT_ArenaTile tile = _tiles[tileNdx.x, tileNdx.y, tileNdx.z];
        }




    }
}
#endif