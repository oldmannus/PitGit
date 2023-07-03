#define DEBUGON

using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JLib.Utilities;

namespace JLib.Grid
{


    #region Structs


    // this returns info about our grid
    public abstract class GridInfoAccessor
    {
        public abstract int GetGridXCount();
        public abstract int GetGridYCount();
        public abstract int GetGridZCount();

        public abstract IVec3[] GetOffsetsFrom(IVec3 pos);
        public abstract float[] GetCostsFrom(IVec3 pos);

    }

    #endregion

    #region Enum
    public enum PathFinderNodeType
    {
        Start   = 1,
        End     = 2,
        Open    = 4,
        Close   = 8,
        Current = 16,
        Path    = 32
    }

    public enum HeuristicFormula
    {
        Manhattan           = 1,
        MaxDXDY             = 2,
        DiagonalShortCut    = 3,
        Euclidean           = 4,
        EuclideanNoSQR      = 5,
        Custom1             = 6
    }
    #endregion

    #region Delegates
    public delegate void PathFinderDebugHandler(int fromX, int fromY, int x, int y, PathFinderNodeType type, float totalCost, float cost);
    #endregion

    public class PathFinder : IPathFinder
    {
      //  [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint="RtlZeroMemory")]
      //  public unsafe static extern bool ZeroMemory(byte* destination, int length);

        #region Events
        public event PathFinderDebugHandler PathFinderDebug;
        #endregion

        #region Variables Declaration
        private GridInfoAccessor                mGrid                   = null;
        private PriorityQueueB<PathNode>        mOpen                   = new PriorityQueueB<PathNode>(new ComparePFNode());
        private Path                            mClose                  = new Path();
        private bool                            mStop                   = false;
        private bool                            mStopped                = true;
        private int                             mHoriz                  = 0;
        private HeuristicFormula                mFormula                = HeuristicFormula.Manhattan;
        private int                             mHEstimate              = 2;
        private bool                            mPunishChangeDirection  = false;
        private bool                            mTieBreaker             = false;
        private bool                            mHeavyDiagonals         = false;
        private int                             mSearchLimit            = 2000;
        private double                          mCompletedTime          = 0;
        private bool                            mDebugProgress          = false;
        private bool                            mDebugFoundPath         = false;
        #endregion

        #region Constructors
        public PathFinder(GridInfoAccessor grid)
        {
            if (grid == null)
                throw new Exception("Grid cannot be null");

            mGrid = grid;
        }
        #endregion

        #region Properties
        public bool Stopped
        {
            get { return mStopped; }
        }

        public HeuristicFormula Formula
        {
            get { return mFormula; }
            set { mFormula = value; }
        }

        public bool HeavyDiagonals
        {
            get { return mHeavyDiagonals; }
            set { mHeavyDiagonals = value; }
        }

        public int HeuristicEstimate
        {
            get { return mHEstimate; }
            set { mHEstimate = value; }
        }

        public bool PunishChangeDirection
        {
            get { return mPunishChangeDirection; }
            set { mPunishChangeDirection = value; }
        }

        public bool TieBreaker
        {
            get { return mTieBreaker; }
            set { mTieBreaker = value; }
        }

        public int SearchLimit
        {
            get { return mSearchLimit; }
            set { mSearchLimit = value; }
        }

        public double CompletedTime
        {
            get { return mCompletedTime; }
            set { mCompletedTime = value; }
        }

        public bool DebugProgress
        {
            get { return mDebugProgress; }
            set { mDebugProgress = value; }
        }

        public bool DebugFoundPath
        {
            get { return mDebugFoundPath; }
            set { mDebugFoundPath = value; }
        }
        #endregion

        #region Methods
        public void FindPathStop()
        {
            mStop = true;
        }

        public Path FindPath(IVec3 start, IVec3 end )
        {
            Profiler.Start("Pathfinder.FindPath");

            HighResolutionTime.Start();

            PathNode parentNode;
            bool found  = false;
            int gridX = mGrid.GetGridXCount();
            int gridY = mGrid.GetGridYCount();
            int gridZ = mGrid.GetGridZCount();

            mStop       = false;
            mStopped    = false;
            mOpen.Clear();
            mClose.Clear();

            #if DEBUGON
            if (mDebugProgress && PathFinderDebug != null)
                PathFinderDebug(0, 0, start.x, start.y, PathFinderNodeType.Start, -1, -1);
            if (mDebugProgress && PathFinderDebug != null)
                PathFinderDebug(0, 0, end.x, end.y, PathFinderNodeType.End, -1, -1);
#endif
            parentNode.G         = 0;
            parentNode.H         = mHEstimate;
            parentNode.F         = parentNode.G + parentNode.H;
            parentNode.Pos       = start;
            parentNode.ParentPos = parentNode.Pos;
            mOpen.Push(parentNode);

            while(mOpen.Count > 0 && !mStop)
            {
                parentNode = mOpen.Pop();

                //#if DEBUGON  // commente
                //if (mDebugProgress && PathFinderDebug != null)
                //    PathFinderDebug(0, 0, parentNode.X, parentNode.Y, PathFinderNodeType.Current, -1, -1);
                //#endif

                if (parentNode.Pos == end)
                {
                    mClose.Add(parentNode);
                    found = true;
                    break;
                }

                if (mClose.Count > mSearchLimit)
                {
                    mStopped = true;
                    Profiler.Stop("Pathfinder.FindPath");
                    return null;
                }

                // PJS TODO
                //if (mPunishChangeDirection)
                //    mHoriz = (parentNode.PosX - parentNode.PX); 

                //Lets calculate each successors

                IVec3[] offsets = mGrid.GetOffsetsFrom(parentNode.Pos);
                float[] costs = mGrid.GetCostsFrom(parentNode.Pos);

                for (int i=0; i< offsets.Length; i++)
                {
                    PathNode newNode = new PathNode();
                    //newNode.Pos.x = parentNode.X + direction[i, 0];
                    //newNode.Y = parentNode.Y + direction[i, 1];
                    //newNode.Z = 0;  //### FIX FOR PATHFINDING TO WORK IN Z
                    newNode.Pos = parentNode.Pos + offsets[i];

                    Debug.Print("Testing node: " + newNode.Pos + "," + offsets[i] + "\n");

                    float newG = parentNode.G + costs[i];


                    if (newG == parentNode.G)
                    {
                        //Unbrekeable
                        continue;
                    }

                    if (mPunishChangeDirection)
                    {
                        if ((newNode.Pos.x - parentNode.Pos.x) != 0)
                        {
                            if (mHoriz == 0)
                                newG += 20;
                        }
                        if ((newNode.Pos.y - parentNode.Pos.y) != 0)
                        {
                            if (mHoriz != 0)
                                newG += 20;

                        }
                        if ((newNode.Pos.z - parentNode.Pos.z) != 0)
                        {
                            if (mHoriz != 0)
                                newG += 20;

                        }
                    }

                    int     foundInOpenIndex = -1;
                    for(int j=0; j<mOpen.Count; j++)
                    {
                        if (mOpen[j].Pos == newNode.Pos)
                        {
                            foundInOpenIndex = j;
                            break;
                        }
                    }
                    if (foundInOpenIndex != -1 && mOpen[foundInOpenIndex].G <= newG)
                        continue;

                    int     foundInCloseIndex = -1;
                    for(int j=0; j<mClose.Count; j++)
                    {
                        if (mClose[j].Pos == newNode.Pos)
                        {
                            foundInCloseIndex = j;
                            break;
                        }
                    }
                    if (foundInCloseIndex != -1 && mClose[foundInCloseIndex].G <= newG)
                        continue;

                    newNode.ParentPos = parentNode.Pos;
                    newNode.G       = newG;

                    switch(mFormula)
                    {
                        default:
                        case HeuristicFormula.Manhattan:
                            newNode.H       = mHEstimate * (Math.Abs(newNode.Pos.x - end.x) + 
                                                            Math.Abs(newNode.Pos.y - end.y) + 
                                                            Math.Abs(newNode.Pos.z - end.z));
                            break;
                        case HeuristicFormula.MaxDXDY:
                            newNode.H       = mHEstimate * (Math.Max(Math.Abs(newNode.Pos.z - end.z), (Math.Max(Math.Abs(newNode.Pos.x - end.x), Math.Abs(newNode.Pos.y - end.y)))));
                            break;
                        case HeuristicFormula.DiagonalShortCut:
                            int h_diagonal  = Math.Min(Math.Abs(newNode.Pos.z - end.z), Math.Min(Math.Abs(newNode.Pos.x - end.x), Math.Abs(newNode.Pos.y - end.y)));
                            int h_straight  = (Math.Abs(newNode.Pos.x - end.x) + Math.Abs(newNode.Pos.y - end.y) + Math.Abs(newNode.Pos.z - end.z));
                            newNode.H       = (mHEstimate * 2) * h_diagonal + mHEstimate * (h_straight - 2 * h_diagonal);
                            break;
                        case HeuristicFormula.Euclidean:
                            newNode.H       = (int) (mHEstimate * Math.Sqrt(Math.Pow((newNode.Pos.x - end.x) , 2) + Math.Pow((newNode.Pos.y - end.y), 2) + +Math.Pow((newNode.Pos.z - end.z), 2)));
                            break;
                        case HeuristicFormula.EuclideanNoSQR:
                            newNode.H       = (int) (mHEstimate * (Math.Pow((newNode.Pos.x - end.x) , 2) + Math.Pow((newNode.Pos.y - end.y), 2) + Math.Pow((newNode.Pos.z - end.z), 2)));
                            break;
                        //case HeuristicFormula.Custom1:
                        //    IVec3 dxy = new IVec3();
                        //    dxy.Set(Math.Abs(end.x - newNode.Pos.x), Math.Abs(end.y - newNode.Pos.y), Math.Abs(end.z - newNode.Pos.z));   
                        //    int Orthogonal  = Math.Abs(dxy.x - dxy.y);
                        //    int Diagonal    = Math.Abs(((dxy.x + dxy.y) - Orthogonal) / 2);
                        //    newNode.H       = mHEstimate * (Diagonal + Orthogonal + dxy.x + dxy.y);
                        //    break;
                    }
                    //if (mTieBreaker)
                    //{
                    //    float d1 = (parentNode.Pos - end).Magnitude;
                    //    int dx1 = parentNode.X - end.x;
                    //    int dy1 = parentNode.Y - end.y;
                    //    int dx2 = start.x - end.x;
                    //    int dy2 = start.y - end.y;
                    //    int cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                    //    newNode.H = (int) (newNode.H + cross * 0.001);
                    //}
                    newNode.F       = newNode.G + newNode.H;

                    //#if DEBUGON
                    //if (mDebugProgress && PathFinderDebug != null)
                    //    PathFinderDebug(parentNode.X, parentNode.Y, newNode.Pos.x, newNode.Y, PathFinderNodeType.Open, newNode.F, newNode.G);
                    //#endif
                    

                    //It is faster if we leave the open node in the priority queue
                    //When it is removed, all nodes around will be closed, it will be ignored automatically
                    //if (foundInOpenIndex != -1)
                    //    mOpen.RemoveAt(foundInOpenIndex);

                    //if (foundInOpenIndex == -1)
                        mOpen.Push(newNode);
                }

                mClose.Add(parentNode);

                //#if DEBUGON
                //if (mDebugProgress && PathFinderDebug != null)
                //    PathFinderDebug(0, 0, parentNode.X, parentNode.Y, PathFinderNodeType.Close, parentNode.F, parentNode.G);
                //#endif
            }

            mCompletedTime = HighResolutionTime.GetTime();
            if (found)
            {
                PathNode fNode = mClose[mClose.Count - 1];
                for(int i=mClose.Count - 1; i>=0; i--)
                {
                    if (fNode.ParentPos == mClose[i].Pos || i == mClose.Count - 1)
                    {
                        //#if DEBUGON
                        //if (mDebugFoundPath && PathFinderDebug != null)
                        //    PathFinderDebug(fNode.X, fNode.Y, mClose[i].X, mClose[i].Y, PathFinderNodeType.Path, mClose[i].F, mClose[i].G);
                        //#endif
                        fNode = mClose[i];
                    }
                    else
                        mClose.RemoveAt(i);
                }
                mStopped = true;
                mClose.RemoveAt(0); // we have duplicate path information
                PathNode endPFN = new PathNode();
                endPFN.ParentPos = end;
                mClose.Add(endPFN);
                Profiler.Stop("Pathfinder.FindPath");
                return mClose;
            }
            mStopped = true;
            Profiler.Stop("Pathfinder.FindPath");
            return null;
        }
        #endregion

        #region Inner Classes
        internal class ComparePFNode : IComparer<PathNode>
        {
            #region IComparer Members
            public int Compare(PathNode x, PathNode y)
            {
                if (x.F > y.F)
                    return 1;
                else if (x.F < y.F)
                    return -1;
                return 0;
            }
            #endregion
        }
        #endregion
    }
}
