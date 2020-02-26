// ReSharper disable StyleCop.SA1600
namespace NURBS
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media.Media3D;

    public static class Extensions
    {
        public static List<Point3D> ToPoint3DList(this IEnumerable<NurbsPoint> nurbsPointEnum)
        {
            var output = new List<Point3D>();
            foreach (var p in nurbsPointEnum)
            {
                output.Add(p.ToPoint3D());
                output.Add(p.ToPoint3D());
            }

            output.RemoveAt(0);
            output.RemoveAt(output.Count - 1);
            return output;
        }

        public static List<Point4D> ToPoint4DList(this IEnumerable<NurbsPoint> nurbsPointEnum)
        {
            return nurbsPointEnum?.Select(nurbspoint => nurbspoint?.ToPoint4D())?.ToList();
        }

        public static List<List<T>> Transpose<T>(this List<List<T>> source)
        {
            var output = new List<List<T>>();

            for (var c = 0; c < source.First().Count; c++)
            {
                output.Add(source.Select(t => t[c]).ToList());
            }

            return output;
        }
    }
}
