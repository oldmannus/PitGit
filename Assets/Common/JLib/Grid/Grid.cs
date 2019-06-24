using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JLib.Utilities;

namespace JLib.Grid
{
    /// <summary>
    /// Grid encapsulates a grid like world
    /// Stuff to have
    ///     - list of objects in the world
    ///             maybe? might make too game-specific. Might not be useful
    ///     - each tile has an array of costs to places one can go from this spot
    ///     
    /// TODO:
    ///     add logic to do functions on subsets of grid
    ///     add cellular autamata functions
    ///     add async locks & functions
    ///     add file loading
    
    [Serializable]
    public class Grid
    {
        IVec3 _size;
        UInt64 _highestLayerId = 1;

        public int XCount { get { return _size.x; } }
        public int YCount { get { return _size.y; } }
        public int ZCount { get { return _size.z; } }
        public IVec3 GridTileCount { get { return _size; } }

        public virtual void Initialize(IVec3 size)
        {
            _size = size;
        }

        //public T GetLayer<T>(UInt64 id) where T : class, IGridLayer
        //{
        //    return _layers[id] as T;
        //}

        //public GridLayer<T> GetLayerOfType<T>(UInt64 id)
        //{
        //    return _layers[id] as GridLayer<T>;
        //}



        public T CreateLayer<T>() where T : class, IGridLayer, new()
        {
            return AddLayer(new T()) as T;
        }

        public GridLayer<T> CreateLayerOfTileType<T>()
        {
            return AddLayer(new GridLayer<T>()) as GridLayer<T>;
        }

        public IGridLayer AddLayer(IGridLayer newLayer)
        {
            newLayer.Initialize(_size, this);
            newLayer.Id = _highestLayerId++;
            //_layers.Add(_highestLayerId, newLayer);

            return newLayer;
        }

        public void RemoveLayer(UInt64 layerId)
        {
            //_layers.Remove(layerId);
        }


        //public void SetValue<T>(UInt64 layerId, IVec3 pos, T value)
        //{
        //    GridLayer<T> layer = _layers[layerId] as GridLayer<T>;
        //    layer.Set(pos, value);
        //}

        //public T GetValue<T>(UInt64 layerId, IVec3 pos)
        //{
        //    GridLayer<T> layer = _layers[layerId] as GridLayer<T>;
        //    return layer.Get(pos);
        //}

        DirectionFlag SetDirectionFlag(DirectionFlag value, DirectionFlag flag, bool b)
        {
            if (b)
                return value | flag;
            else
                return (value &= ~flag);
        }

       

        public delegate Dest LayerOp0<Dest>(Dest d);
        public delegate Dest LayerOp1<Dest, P1>(Dest d, P1 a);
        public delegate Dest LayerOp2<Dest, P1, P2>(Dest d, P1 a, P2 b);
        public delegate Dest LayerOp3<Dest, P1, P2, P3>(Dest d, P1 a, P2 b, P3 c);

        public delegate Dest LayerOp0Pos<Dest>(IVec3 p, Dest d);
        public delegate Dest LayerOp1Pos<Dest, P1>(IVec3 p, Dest d, P1 a);
        public delegate Dest LayerOp2Pos<Dest, P1, P2>(IVec3 p, Dest d, P1 a, P2 b);
        public delegate Dest LayerOp3Pos<Dest, P1, P2, P3>(IVec3 p, Dest d, P1 a, P2 b, P3 c);


        public void DoLayerOp<Dest>(GridLayer<Dest> d, LayerOp0<Dest> op)
        {
            IVec3 pos;
            for (pos.x = 0; pos.x < XCount; pos.x++)
                for (pos.y = 0; pos.y < YCount; pos.y++)
                    for (pos.z = 0; pos.z < ZCount; pos.z++)
                        d.Set(pos, op(d.Get(pos)));
        }
        public void DoLayerOp<Dest, P1>(GridLayer<Dest> d, GridLayer<P1> p1, LayerOp1<Dest, P1> op)
        {
            IVec3 pos;
            for (pos.x = 0; pos.x < XCount; pos.x++)
                for (pos.y = 0; pos.y < YCount; pos.y++)
                    for (pos.z = 0; pos.z < ZCount; pos.z++)
                        d.Set(pos, op(d.Get(pos), p1.Get(pos)));
        }
        public void DoLayerOp<Dest, P1, P2>(GridLayer<Dest> d, GridLayer<P1> p1, GridLayer<P2> p2, LayerOp2<Dest, P1, P2> op)
        {
            IVec3 pos;
            for (pos.x = 0; pos.x < XCount; pos.x++)
                for (pos.y = 0; pos.y < YCount; pos.y++)
                    for (pos.z = 0; pos.z < ZCount; pos.z++)
                        d.Set(pos, op(d.Get(pos), p1.Get(pos), p2.Get(pos)));
        }
        public void DoLayerOp<Dest, P1, P2, P3>(GridLayer<Dest> d, GridLayer<P1> p1, GridLayer<P2> p2, GridLayer<P3> p3, LayerOp3<Dest, P1, P2, P3> op)
        {
            IVec3 pos;
            for (pos.x = 0; pos.x < XCount; pos.x++)
                for (pos.y = 0; pos.y < YCount; pos.y++)
                    for (pos.z = 0; pos.z < ZCount; pos.z++)
                        d.Set(pos, op(d.Get(pos), p1.Get(pos), p2.Get(pos), p3.Get(pos)));
        }

        public void DoLayerOpPos<Dest>(GridLayer<Dest> d, LayerOp0Pos<Dest> op)
        {
            IVec3 pos;
            for (pos.x = 0; pos.x < XCount; pos.x++)
                for (pos.y = 0; pos.y < YCount; pos.y++)
                    for (pos.z = 0; pos.z < ZCount; pos.z++)
                        d.Set(pos, op(pos, d.Get(pos)));
        }
        public void DoLayerOpPos<Dest, P1>(GridLayer<Dest> d, GridLayer<P1> p1, LayerOp1Pos<Dest, P1> op)
        {
            IVec3 pos;
            for (pos.x = 0; pos.x < XCount; pos.x++)
                for (pos.y = 0; pos.y < YCount; pos.y++)
                    for (pos.z = 0; pos.z < ZCount; pos.z++)
                        d.Set(pos, op(pos, d.Get(pos), p1.Get(pos)));
        }
        public void DoLayerOpPos<Dest, P1, P2>(GridLayer<Dest> d, GridLayer<P1> p1, GridLayer<P2> p2, LayerOp2Pos<Dest, P1, P2> op)
        {
            IVec3 pos;
            for (pos.x = 0; pos.x < XCount; pos.x++)
                for (pos.y = 0; pos.y < YCount; pos.y++)
                    for (pos.z = 0; pos.z < ZCount; pos.z++)
                        d.Set(pos, op(pos, d.Get(pos), p1.Get(pos), p2.Get(pos)));
        }
        public void DoLayerOpPos<Dest, P1, P2, P3>(GridLayer<Dest> d, GridLayer<P1> p1, GridLayer<P2> p2, GridLayer<P3> p3, LayerOp3Pos<Dest, P1, P2, P3> op)
        {
            IVec3 pos;
            for (pos.x = 0; pos.x < XCount; pos.x++)
                for (pos.y = 0; pos.y < YCount; pos.y++)
                    for (pos.z = 0; pos.z < ZCount; pos.z++)
                        d.Set(pos, op(pos, d.Get(pos), p1.Get(pos), p2.Get(pos), p3.Get(pos)));
        }
        

        // this is a utility function to set the accessibility flags based simply on what edges are available
        public void BuildBaseAccessibility(GridLayer<DirectionFlag> layer)
        {
            DirectionFlag flags;
            IVec3 pos;
            for (pos.x = 0; pos.x < XCount; pos.x++)
            {
                //flags = DirectionFlag.Diagonal;
                flags = 0;
                flags = SetDirectionFlag(flags, DirectionFlag.NegX, pos.x != 0);
                flags = SetDirectionFlag(flags, DirectionFlag.PosX, pos.x != XCount - 1);

                for (pos.y = 0; pos.y < YCount; pos.y++)
                {
                    flags = SetDirectionFlag(flags, DirectionFlag.NegY, pos.y != 0);
                    flags = SetDirectionFlag(flags, DirectionFlag.PosY, pos.y != YCount - 1);

                    for (pos.z = 0; pos.z < ZCount; pos.z++)
                    {
                        flags = SetDirectionFlag(flags, DirectionFlag.NegZ, pos.z != 0);
                        flags = SetDirectionFlag(flags, DirectionFlag.PosZ, pos.z != ZCount - 1);

                        layer.Set(pos, flags);
                    }
                }
            }
        }
    }
}