
namespace Pit.Utilities
{
    public struct Sphere
    {
        public FVec3 Center;
        public float Radius;


        public Sphere(FVec3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }



        public static Sphere Merge(Sphere bs1, Sphere bs2)
        {
            FVec3 deltaVec = bs1.Center - bs2.Center;
            float dist = deltaVec.Length;

            float tworadius = dist + bs1.Radius + bs2.Radius;
            float radius = tworadius / 2;

            deltaVec.Normalize();
            FVec3 startPt = bs1.Center - (deltaVec * bs1.Radius);
            FVec3 center = startPt + deltaVec * radius;


            return new Sphere(center, radius);
        }
    }
}
