using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pit.Utilities
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

            FVec3 center;
            center.x = width / 2;
            center.y = height / 2;
            center.z = depth / 2;

            bounds.Center = center;

            FVec3 size;
            size.x = center.x * 2;  // seems like we just divided, why multiply, because this rounds our given dimensions down
            size.y = center.y * 2;  // seems like we just divided, why multiply, because this rounds our given dimensions down
            size.z = center.z * 2;  // seems like we just divided, why multiply, because this rounds our given dimensions down

            bounds.Size = size;

            return bounds;
        }




    }
}
