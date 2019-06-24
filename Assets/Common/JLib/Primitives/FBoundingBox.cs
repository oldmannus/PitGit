using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;

namespace JLib.Utilities
{
    public struct FBoundingBox
    {
        public FBoundingBox(FVec3 pCenter, FVec3 pSize)
        {
            Center = pCenter;
            Size = pSize;
        }

        ///
        public FVec3 Center { get; set; }///The center of the bounding box.
        public FVec3 Size { get; set; }    //     The total size of the box. This is always twice as large as the extents.
        public FVec3 Extents { get { return Size / 2.0f; } }/// The extents of the box. 
        public FVec3 Max { get { return Center + Extents; } }    //     The maximal point of the box. 
        public FVec3 Min { get { return Center - Extents; } }    //     The minimal point of the box. This is always equal to center-extents.


        public static FBoundingBox MakeCenteredFromHeightAndWidth(int width, int height, int depth)
        {
            FBoundingBox bounds = new FBoundingBox();

            bounds.Center = new FVec3(width * 0.5f, height * 0.5f, depth * 0.5f);
            bounds.Size = new FVec3(width, height, depth); ;

            return bounds;
        }




    }
}
