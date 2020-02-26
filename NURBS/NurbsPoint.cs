// ReSharper disable StyleCop.SA1600
namespace NURBS
{
    using System.Windows.Media.Media3D;

    public class NurbsPoint
    {
        public NurbsPoint(double x, double y, double z, double weight)
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

        public Point4D ToPoint4D()
        {
            var x = this.X * this.Weight;
            var y = this.Y * this.Weight;
            var z = this.Z * this.Weight;
            return new Point4D(x, y, z, this.Weight);
        }

        public Point3D ToPoint3D()
        {
            var x = this.X;
            var y = this.Y;
            var z = this.Z;
            return new Point3D(x, y, z);
        }
    }
}
