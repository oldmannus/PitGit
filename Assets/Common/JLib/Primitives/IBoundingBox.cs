using System.Collections;

namespace JLib.Utilities
{
    public struct IBoundingBox
    {

        public IBoundingBox(IVec3 pCenter, IVec3 pSize)
        {
            Center = pCenter;
            Size = pSize;
        }

        ///
        public IVec3 Center { get; set; }///The center of the bounding box.
        public IVec3 Size { get; set; }    //     The total size of the box. This is always twice as large as the extents.
        public IVec3 Extents { get { return Size / 2; } }/// The extents of the box. 
        public IVec3 Max { get { return Center + Extents; } }    //     The maximal point of the box. 
        public IVec3 Min { get { return Center - Extents; } }    //     The minimal point of the box. This is always equal to center-extents.


        public static IBoundingBox MakeCenteredFromHeightAndWidth(int width, int height, int depth)
        {
            IBoundingBox bounds = new IBoundingBox();

            IVec3 center;
            center.x = width / 2;
            center.y = height / 2;
            center.z = depth / 2;

            bounds.Center = center;

            IVec3 size;
            size.x = center.x * 2;  // seems like we just divided, why multiply, because this rounds our given dimensions down
            size.y = center.y * 2;  // seems like we just divided, why multiply, because this rounds our given dimensions down
            size.z = center.z * 2;  // seems like we just divided, why multiply, because this rounds our given dimensions down

            bounds.Size = size;

            return bounds;
        }




        ////
        //// Summary:
        ////     ///
        ////     The closest point on the bounding box.
        ////     ///
        ////
        //// Parameters:
        ////   point:
        ////     Arbitrary point.
        ////
        //// Returns:
        ////     ///
        ////     The point on the bounding box or inside the bounding box.
        ////     ///
        //public IFVec3 ClosestPoint(IFVec3 point);
        ////
        //// Summary:
        ////     ///
        ////     Is point contained in the bounding box?
        ////     ///
        ////
        //// Parameters:
        ////   point:
        //public bool Contains(IFVec3 point);
        ////
        //// Summary:
        ////     ///
        ////     Grows the Bounds to include the point.
        ////     ///
        ////
        //// Parameters:
        ////   point:
        //public void Encapsulate(IFVec3 point);
        ////
        //// Summary:
        ////     ///
        ////     Grow the bounds to encapsulate the bounds.
        ////     ///
        ////
        //// Parameters:
        ////   bounds:
        //public void Encapsulate(Bounds bounds);
        //public override bool Equals(object other);
        ////
        //// Summary:
        ////     ///
        ////     Expand the bounds by increasing its size by amount along each side.
        ////     ///
        ////
        //// Parameters:
        ////   amount:
        //public void Expand(IFVec3 amount);
        ////
        //// Summary:
        ////     ///
        ////     Expand the bounds by increasing its size by amount along each side.
        ////     ///
        ////
        //// Parameters:
        ////   amount:
        //public void Expand(float amount);

        ////
        //// Summary:
        ////     ///
        ////     Does another bounding box intersect with this bounding box?
        ////     ///
        ////
        //// Parameters:
        ////   bounds:
        //public bool Intersects(Bounds bounds);
        ////
        //// Summary:
        ////     ///
        ////     Sets the bounds to the min and max value of the box.
        ////     ///
        ////
        //// Parameters:
        ////   min:
        ////
        ////   max:
        //public void SetMinMax(IFVec3 min, IFVec3 max);
        ////
        //// Summary:
        ////     ///
        ////     The smallest squared distance between the point and this bounding box.
        ////     ///
        ////
        //// Parameters:
        ////   point:
        //public float SqrDistance(IFVec3 point);
        ////
        //// Summary:
        ////     ///
        ////     Returns a nicely formatted string for the bounds.
        ////     ///
        ////
        //// Parameters:
        ////   format:
        //public override string ToString()
        //{

        //}
        ////
        //// Summary:
        ////     ///
        ////     Returns a nicely formatted string for the bounds.
        ////     ///
        ////
        //// Parameters:
        ////   format:
        //public string ToString(string format);

        ////public static bool operator ==(Bounds lhs, Bounds rhs);
        ////public static bool operator !=(Bounds lhs, Bounds rhs);
    }
}