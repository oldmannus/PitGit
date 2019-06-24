using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Sim;
using JLib.Utilities;
using JLib.Unity;

namespace Pit
{


    public class SetMapEvent : GameEvent
    {
        public MT_Arena Arena;
    }


    /// <summary>
    /// This should be placed in an object in a unity scene that is used for the 'arena'. 
    /// It encapsulates all of the various information about the 'space' used for the combat, 
    ///    like spawn points and such
    /// </summary>
    public class MT_Arena : SM_Space
    {
        [SerializeField]
        public List<MT_ArenaSpawnPoint> _spawnPoints = null;

        /// <summary>
        /// Set in editor
        /// </summary>
        public UI_WidgetMgr Widgets = null;


        public GameObject           PawnRoot; // set in editor
        public LG_ArenaDescriptor   Description { get { return _desc; } }


        // -----------------------------------------------------------------------------------------
        /// <summary>
        /// this takes the X & Z coordinates of the given point and fills in the Y of the terrain point
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool FindTerrainPointAt(ref Vector3 where)
        // -----------------------------------------------------------------------------------------
        {
            Vector3 origin = where;
            origin.y += 1000.0f;

            Vector3 direction = Vector3.zero;
            direction.y = -1.0f;
            
            RaycastHit hitInfo;
            if (!Physics.Raycast(origin, direction, out hitInfo, 2000.0f, _terrainLayerMask))
                return false;

            where = hitInfo.point;
            return true;
        }

        // -----------------------------------------------------------------------------------------
        public bool FindTerrainPointUnderMouse(UN_Camera cam, ref Vector3 where)
        // -----------------------------------------------------------------------------------------
        {
            if (Dbg.Assert(cam != null))
                return false;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (!Physics.Raycast(ray, out hitInfo, 300, _terrainLayerMask))
                return false;

            where = hitInfo.point;
            return true;
        }




        // -------------------------------------------------------------------------------------------
        public MT_ArenaSpawnPoint GetUnusedSpawnPoint(int teamNdx)
        // -------------------------------------------------------------------------------------------
        {
            for (int i = 0; i < _spawnPoints.Count; i++)
            {
                if (_spawnPoints[i].TeamNdx == teamNdx && _spawnPoints[i].Used == false)
                {
                    return _spawnPoints[i]; // note we don't set used flag. That's up to the caller
                }
            }
            return null;
        }


        // -------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the arena descriptor. Later we can use this to configure the map
        /// </summary>
        /// <param name="data"></param>
        public void Configure(object data)
        // -------------------------------------------------------------------------------------------
        {
            _desc = data as LG_ArenaDescriptor;

            // TODO add more options to descriptor, to turn stuff in arena off and on (traps, etc)

            Events.SendGlobal(new SetMapEvent() { Arena = this });
        }
        public MT_ArenaGrid Grid { get; private set; }
        

        #region Internals

        LG_ArenaDescriptor _desc = null;
        int _terrainLayerMask;
        
        protected override void Start()
        {
            _terrainLayerMask = LayerMask.GetMask("Terrain");
            Grid = GetComponent<MT_ArenaGrid>();

            foreach (var sp in _spawnPoints)
            {
                Vector3 position = sp.gameObject.transform.position;
               Grid.SnapToGrid(ref position);
                sp.gameObject.transform.position = position;
            }

            //_spawnPoints = new List<MT_ArenaSpawnPoint>();

            //MT_ArenaSpawnPoint[] pts = GetComponentsInChildren<MT_ArenaSpawnPoint>();
            //if (pts == null)
            //{
            //    Dbg.LogError("No Spawn points on map. Invalid data");
            //}
            //else
            //{
            //    _spawnPoints.AddRange(pts);
            //}


            // do this last, as it sends messages
            base.Start();
        }
        #endregion


    }
}