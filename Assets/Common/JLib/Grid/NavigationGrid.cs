using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using JLib.Utilities;

namespace JLib.Grid
{
    public class NavigationGridAccessor : GridInfoAccessor
    {
        protected NavigationGrid _grid;
        AdjacencyGridLayer _adjacencyGridLayer;
        AdjacentCostGridLayer _adjacentCostGridLayer;

        public void Initialize(NavigationGrid grid, AdjacencyGridLayer adjacency, AdjacentCostGridLayer cost)
        {
            _grid = grid;
            _adjacencyGridLayer = adjacency;
            _adjacentCostGridLayer = cost;
        }

        public override int GetGridXCount()
        {
            return _grid.XCount;
        }
        public override int GetGridYCount()
        {
            return _grid.YCount;
        }
        public override int GetGridZCount()
        {
            return _grid.ZCount;
        }

        public override IVec3[] GetOffsetsFrom(IVec3 pos)
        {
            return _adjacencyGridLayer.Get(pos);
        }
        public override float[] GetCostsFrom(IVec3 pos)
        {
            return _adjacentCostGridLayer.Get(pos);
        }
    }

    public class MovementType
    {
        public int Id;
        public UInt64 Mask { get { return ((UInt64)1) << Id; } }
    }

    [Serializable]
    public class NavigationGrid : Grid
    {
        [JsonProperty]
        GridLayer<UInt64> _staticObstacleGrid;

        [JsonProperty]
        GridLayer<UInt64> _finalObstacleGrid;

        [JsonProperty]
        GridLayer<DirectionFlag> _gridBorderDirectionFlags;

        [JsonProperty]
        Dictionary<int, AdjacencyGridLayer> _adjacencyLayers = new Dictionary<int, AdjacencyGridLayer>();

        [JsonProperty]
        Dictionary<int, AdjacentCostGridLayer> _adjCostLayers = new Dictionary<int, AdjacentCostGridLayer>();

        [JsonProperty]
        List<CostGridLayer> _costLayers = new List<CostGridLayer>();


        /// <summary>
        /// Init the grid to the given size. 
        /// </summary>
        /// <param name="numTiles"></param>
        public override void Initialize(IVec3 numTiles)
        {
            base.Initialize(numTiles);

            _staticObstacleGrid = CreateLayerOfTileType<UInt64>();
            _finalObstacleGrid = CreateLayerOfTileType<UInt64>();

            _gridBorderDirectionFlags = CreateLayerOfTileType<DirectionFlag>(); // which directions one can go from current location, hedged by edges
            BuildBaseAccessibility(_gridBorderDirectionFlags);
        }

        /// <summary>
        /// Adds a cost layer to our system. This will be scanned when
        /// building navigation for a given movement type
        /// </summary>
        /// <param name="affects"></param>
        /// <param name="defaultCost"></param>
        /// <returns></returns>
        public CostGridLayer CreateCostLayer(UInt64 affects, float defaultCost = 1.0f)
        {
            CostGridLayer layer = CreateLayer<CostGridLayer>();
           
            DoLayerOp(layer, (d) => { return defaultCost; });
            layer.LayersAffected = affects;

            _costLayers.Add(layer);
            return layer;
        }


        /// <summary>
        /// Add a type of navigation to our navigation grid. Note that the id has to be unique
        /// </summary>
        /// <param name="id"></param>
        public void AddNavigationType(MovementType id)
        {
            AdjacencyGridLayer navLayer = CreateLayer<AdjacencyGridLayer>();
            navLayer.Initialize(GridTileCount, this, id.Mask);
            _adjacencyLayers.Add(id.Id, navLayer);

            AdjacentCostGridLayer costLayer = CreateLayer<AdjacentCostGridLayer>();
            costLayer.Initialize(GridTileCount, this);
            _adjCostLayers.Add(id.Id, costLayer);
        }

        /// <summary>
        /// Tells system to build the nav layer for the given registered movement type
        /// </summary>
        /// <param name="type"></param>
        public void BuildNavLayer(MovementType type)
        {
            AdjacencyGridLayer adjLayer = _adjacencyLayers[type.Id];
            AdjacentCostGridLayer costLayer = _adjCostLayers[type.Id];

            adjLayer.BuildAdjacency(_gridBorderDirectionFlags, _finalObstacleGrid);
           
            
            CostGridLayer accumCostGridLayer = CreateLayer<CostGridLayer>();
            bool foundCostLayer = false;
            foreach (CostGridLayer other in _costLayers)
            {
                if ((other.LayersAffected & type.Mask) == type.Mask)
                {
                    DoLayerOp(accumCostGridLayer, other, (d, src) => { return d + src; });
                    foundCostLayer = true;
                }
            }
            if (!foundCostLayer)
            {
                Debug.Print("No cost layer found! Pathfinding will fail");
            }
            costLayer.BuildCosts(adjLayer, accumCostGridLayer);
        }

        // synchronous for now
        public PathFindResult FindPath(IVec3 start, IVec3 end, MovementType type)
        {
            PathFindResult result = new PathFindResult();

            NavigationGridAccessor pathAccessor = new NavigationGridAccessor();
            pathAccessor.Initialize(this, _adjacencyLayers[type.Id], _adjCostLayers[type.Id]);

            PathFinder finder = new PathFinder(pathAccessor);

            Path path = finder.FindPath(start, end);

            result.SetResult(path, path == null ? PathFindResult.Result.Failed : PathFindResult.Result.Succeeded);


            return result;
        }

    }
}
