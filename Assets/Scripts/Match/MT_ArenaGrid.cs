using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JLib.Utilities;
namespace Pit
{

    // information about a given spot on map. 
    public struct MT_ArenaVoxel
    {
        [Flags]
        public enum Flags
        {
            None = 0,
            BlockMove  = (1<<0),
            BlocksLOS = (1<<1),
            BlocksFlight = (1<<2)
        }

        Flags _flags;

        public void SetFlag(Flags r) { _flags |= r; }
        public void ClearFlag(Flags r) { _flags &= ~r; }

        public bool BlocksMove { get { return _flags.HasFlag(Flags.BlockMove); } }
    }

    public class MT_ArenaGrid : MonoBehaviour
    {
        [SerializeField,Min(0.1f)]
        float _tileWidth = 1;
        [SerializeField,Min(0.1f)]
        float _tileHeight = 2;

         [SerializeField]
        int _extraVertCount = 5;

        MT_ArenaVoxel[,,] _voxels;
        Bounds _arenaBounds;

        IVec3 _dimensions;
        Vector3 _topLeftGridInWC;

      
        private void Awake()
        {
            _arenaBounds = GetBounds(gameObject);

            Vector3 dimensions = _arenaBounds.max - _arenaBounds.min;
            int xc = ((int)(dimensions.x / _tileWidth)) + 1;
            int yc = ((int)(dimensions.y / _tileHeight)) + _extraVertCount;
            int zc = ((int)(dimensions.z / _tileWidth)) + 1;

            _dimensions.Set(xc, yc, zc);

            _topLeftGridInWC = new Vector3( _arenaBounds.center.x - xc * _tileWidth * 0.5f, 
                                            _arenaBounds.min.y, 
                                            _arenaBounds.center.z - zc * _tileWidth * 0.5f);


            _voxels = new MT_ArenaVoxel[xc, yc, zc];
        }

        public void SnapToGrid(ref Vector3 worldCoord)
        {
            // adjust to box coordinates+

            worldCoord -= _topLeftGridInWC;
       
            int xc = ((int)(worldCoord.x / _tileWidth)) ;
            int yc = ((int)(worldCoord.y / _tileHeight));
            int zc = ((int)(worldCoord.z / _tileWidth));

            worldCoord.Set( _topLeftGridInWC.x + xc * _tileWidth,
                            _topLeftGridInWC.y + yc * _tileHeight,
                            _topLeftGridInWC.z + zc * _tileWidth);
        }


        public static Bounds GetBounds(GameObject obj)
        {
            Bounds bounds = new Bounds();
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

            if (renderers.Length > 0)
            {
                //Find first enabled renderer to start encapsulate from it
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.enabled)
                    {
                        bounds = renderer.bounds;
                        break;
                    }
                }
                //Encapsulate for all renderers
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.enabled)
                    {
                        bounds.Encapsulate(renderer.bounds);
                    }
                }
            }
            return bounds;
        }
   }
}
