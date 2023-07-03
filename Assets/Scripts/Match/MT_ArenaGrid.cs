using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JLib.Utilities;
using JLib.Grid;

namespace Pit
{

    public class MT_ArenaGrid : MonoBehaviour
    {
        [SerializeField]
        Material[] _gridMat = null;

        [SerializeField]
        GameObject[] _gameObjPt = null; 

        [SerializeField,Min(0.1f)]
        float _tileWidth = 1;
        [SerializeField,Min(0.1f)]
        float _tileHeight = 2;

         [SerializeField]
        int _extraVertCount = 5;

        Bounds _arenaBounds;

        //IVec3 _dimensions;
        //Vector3 _topLeftGridInWC;
        WorldGrid _grid = new WorldGrid();

        GridLayer<GameObject> _debugGrid = null;

        // TODO fix this conversion
        FVec3 conv(Vector3 v)
        {
            return new FVec3(v.x, v.y, v.z);
        }

        Vector3 conv(FVec3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        private void Awake()
        {
            _arenaBounds = GetBounds(gameObject);

            _arenaBounds.min = new Vector3(-25, 0, -25);
            _arenaBounds.max = new Vector3(25, 0, 25);


            Vector3 dimensions = _arenaBounds.max - _arenaBounds.min;
            int xc = ((int)(dimensions.x / _tileWidth)) + 1;
            int yc = ((int)(dimensions.y / _tileHeight)) + _extraVertCount;
            int zc = ((int)(dimensions.z / _tileWidth)) + 1;


            //_voxels = new MT_ArenaVoxel[xc, yc, zc];
            FVec3 tileSizeInWC = new FVec3(_tileWidth, _tileHeight, _tileWidth);
            _grid.Initialize(tileSizeInWC, conv(_arenaBounds.min), new IVec3(xc, yc, zc));

            _debugGrid = _grid.CreateLayerOfTileType<GameObject>();

            Vector3 offset = new Vector3(_tileWidth* 0.5f, 0.0f, _tileWidth * 0.5f);

            _grid.DoLayerOpPos(_debugGrid, (pos, tile) =>
            {
                if (pos.y == 0)
                {
                    tile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    tile.transform.parent = this.gameObject.transform;

                    FVec3 fv = _grid.GridToWorldMin(pos);
                    tile.transform.position = conv(_grid.GridToWorldMin(pos)) + offset;
                    //tile.transform.position = tile.transform.position + new Vector3(0.5f, 0.0f, 0.5f);
                    tile.transform.rotation = Quaternion.AngleAxis(90, new Vector3(1,0,0));

                    Material mat = _gridMat[((((pos.x % 2) == 0) ? 1 : 0) + pos.z) % 2];
                    tile.GetComponent<MeshRenderer>().material = mat;

                }
                return tile;
            });

            _gameObjPt[0].transform.position = conv(_grid.GridToWorldMin(new IVec3(0, 0, 0)));
            _gameObjPt[1].transform.position = conv(_grid.GridToWorldMax(new IVec3(0, 0, 0)));
        }

        public IVec3 WorldToGrid(Vector3 vec)
        {
            return _grid.WorldToGrid(conv(vec));
        }

        // TODO move this to WorldGrid
        public void SnapToGrid(ref Vector3 worldCoord)
        {
            worldCoord = conv(_grid.SnapToGrid(conv(worldCoord)));

            //// adjust to box coordinates+

            //worldCoord -= _topLeftGridInWC;

            //int xc = ((int)(worldCoord.x / _tileWidth));
            //int yc = ((int)(worldCoord.y / _tileHeight));
            //int zc = ((int)(worldCoord.z / _tileWidth));

            //worldCoord.Set(_topLeftGridInWC.x + xc * _tileWidth,
            //                _topLeftGridInWC.y + yc * _tileHeight,
            //                _topLeftGridInWC.z + zc * _tileWidth);
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
