// ReSharper disable StyleCop.SA1600
namespace NURBS
{
    using System.Windows.Media.Media3D;

    public class Point4D
    {
        public Point4D(double x, double y, double z, double weight)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Weight = weight;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public double Weight { get; set; }

        public Point3D ToPoint3D()
        {
            var x = this.X / this.Weight;
            var y = this.Y / this.Weight;
            var z = this.Z / this.Weight;
            return new Point3D(x, y, z);
        }

        public NurbsPoint ToNurbsPoint()
        {
            var x = this.X / this.Weight;
            var y = this.Y / this.Weight;
            var z = this.Z / this.Weight;
            return new NurbsPoint(x, y, z, this.Weight);
        }
    }
}
