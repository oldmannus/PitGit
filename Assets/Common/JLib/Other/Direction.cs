using System;
using System.Collections.Generic;

using System.Text;

namespace JLib.Utilities
{
    public enum Direction
    {
        None,
        NW,
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        Count
    }

    [Serializable]
    public class AroundPt
    {
        public IVec2      Pt;
        public Direction   Dir;

        public AroundPt(Direction d)
        {
            Dir = d;
            Pt = AllAround.Get(d);
        }
    }


    static class AllAround
    {

        static Direction[] invertedDirection = new Direction[(int)Direction.Count + 1]
        {
            Direction.None,
            Direction.SE,
            Direction.S,
            Direction.SW,
            Direction.W,
            Direction.NW,
            Direction.N,
            Direction.NE,
            Direction.E,
            Direction.Count
        };


        public static Direction InvertDirection(Direction dir)
        {
            return invertedDirection[(int)dir];
        }


        public static IVec2 Get(Direction d)
        {
            switch (d)
            {
                case Direction.None:
                    return new IVec2(0,0);
                case Direction.NW:
                    return new IVec2(-1,-1);
                case Direction.N:
                    return new IVec2(0,-1);
                case Direction.NE:
                    return new IVec2(1,-1);
                case Direction.E:
                    return new IVec2(1,0);
                case Direction.SE:
                    return new IVec2(1,1);
                case Direction.S:
                    return new IVec2(0, 1);
                case Direction.SW:
                    return new IVec2(-1,1);
                case Direction.W:
                    return new IVec2(-1,0);
            }

            Dbg.Assert(false);
            return new IVec2(0,0);
        }

        public static AroundPt[] allAroundAll = new AroundPt[9] 
        { 
            new AroundPt(Direction.None),    
            new AroundPt(Direction.NW),    
            new AroundPt(Direction.N),    
            new AroundPt(Direction.NE),    
            new AroundPt(Direction.E),    
            new AroundPt(Direction.SE),    
            new AroundPt(Direction.S),    
            new AroundPt(Direction.SW),    
            new AroundPt(Direction.W),    
        };

        static AroundPt[] allAroundNE = new AroundPt[4] 
        { 
            new AroundPt(Direction.None),    
            new AroundPt(Direction.S),    
            new AroundPt(Direction.SW),    
            new AroundPt(Direction.W),    
        };

        static AroundPt[] allAroundN = new AroundPt[6] 
        { 
            new AroundPt(Direction.None),    
            new AroundPt(Direction.E),    
            new AroundPt(Direction.SE),    
            new AroundPt(Direction.S),    
            new AroundPt(Direction.SW),    
            new AroundPt(Direction.W),    
        };

        static AroundPt[] allAroundNW = new AroundPt[4] 
        { 
            new AroundPt(Direction.None),    
            new AroundPt(Direction.E),    
            new AroundPt(Direction.SE),    
            new AroundPt(Direction.S),   
        };


        static AroundPt[] allAroundW = new AroundPt[6] 
        { 
            new AroundPt(Direction.None),    
            new AroundPt(Direction.N),    
            new AroundPt(Direction.NE),    
            new AroundPt(Direction.E),    
            new AroundPt(Direction.SE),    
            new AroundPt(Direction.S),    
        };

        static AroundPt[] allAroundSW = new AroundPt[4] 
        { 
            new AroundPt(Direction.None),    
            new AroundPt(Direction.N),    
            new AroundPt(Direction.NE),    
            new AroundPt(Direction.E),    
        };

        static AroundPt[] allAroundS = new AroundPt[6] 
        { 
            new AroundPt(Direction.None),    
            new AroundPt(Direction.NW),    
            new AroundPt(Direction.N),    
            new AroundPt(Direction.NE),    
            new AroundPt(Direction.E),    
            new AroundPt(Direction.W),           
        };

        static AroundPt[] allAroundSE = new AroundPt[4] 
        { 
            new AroundPt(Direction.None),    
            new AroundPt(Direction.W),          
            new AroundPt(Direction.NW),    
            new AroundPt(Direction.N),    
        };

        static AroundPt[] allAroundE = new AroundPt[6] 
        { 
            new AroundPt(Direction.None),    
            new AroundPt(Direction.N),    
            new AroundPt(Direction.S),    
            new AroundPt(Direction.SW),    
            new AroundPt(Direction.W),    
            new AroundPt(Direction.NW),    
        };

        
        public static AroundPt[] Get(int x, int y, int xLen, int yLen)
        {
            if (x == 0)
            {
                if (y == 0)
                    return allAroundNW;
                if (y == yLen-1)
                    return allAroundSW;

                return allAroundW;
            }
            else if (x == xLen-1)
            {
                if (y == 0)
                    return allAroundNE;
                else if (y == yLen-1)
                    return allAroundSE;

                return allAroundE;
            }
            else if (y == 0)
            {
                return allAroundN;
            }
            else if (y == yLen-1)
            {
                return allAroundS;
            }

            return allAroundAll;
        }

        // TODO: Double check this is right. not sure, after merging 2 forms of it

        public static int GetDirection(int dx, int dy)
        {
            if (dy < 0)
            {
                if (dx < 0)
                    return (int)Direction.N;
                else if (dx == 0)
                    return (int)Direction.NE;
                else
                    return (int)Direction.E;

            }
            else if (dy > 0)
            {
                if (dx < 0)
                    return (int)Direction.W;
                else if (dx == 0)
                    return (int)Direction.SW;
                else
                    return (int)Direction.S;
            }
            else
            {
                if (dx < 0)
                    return (int)Direction.NW;
                else
                    return (int)Direction.SE;
            }
        }

        // figures out what dir you're going from one square to another
        public static int GetDirection(int x1, int y1, int x2, int y2)
        {
            return GetDirection(x2 - x1, y2 - y1);
        }
    }
}
