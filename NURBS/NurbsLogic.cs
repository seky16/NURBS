// ReSharper disable StyleCop.SA1600
namespace NURBS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media.Media3D;

    public static class NurbsLogic
    {
        public static decimal[] GenerateKnotVector(int nplus1, int p)
        {
            if (p + 1 > nplus1 || nplus1 == 0)
            {
                var foo = new[] { 0M };
                return foo;
            }

            var knotVectorLength = nplus1 + p + 1;

            var output = new decimal[knotVectorLength];

            var j = 1M;
            for (var i = 0; i < knotVectorLength; i++)
            {
                if (i < p + 1)
                {
                    output[i] = 0M;
                }
                else if (i < nplus1)
                {
                    output[i] = j / (nplus1 - p);
                    j++;
                }
                else
                {
                    output[i] = 1M;
                }
            }

            return output;
        }

        public static int FindSpan(decimal u, decimal[] knotVector)
        {
            var i = 0;
            while (i < knotVector.Length && u.GreaterThanOrEqualTo(knotVector[i + 1]) && u.LessThan(1M))
            {
                i++;
            }

            return i;
        }

        public static int CheckMultiplicity(decimal[] knotVector, decimal knot) => knotVector.Count(u => knot.AlmostEquals(u));

        public static NurbsPoint DeBoor(int p, List<NurbsPoint> controlPoints, decimal u, decimal[] knotVector)
        {
            if (u.AlmostEquals(0M))
            {
                return controlPoints.First();
            }

            if (u.AlmostEquals(1M))
            {
                return controlPoints.Last();
            }

            var k = FindSpan(u, knotVector);
            var s = NurbsLogic.CheckMultiplicity(knotVector, u);

            NurbsPoint point;

            if (u.GreaterThanOrEqualTo(knotVector[k]) && u.LessThan(knotVector[k + 1]))
            {
                var h = p - s;

            if (h == 0)
            {
                return controlPoints[k - s];
            }

            var points = new List<Point4D>[h + 1];
            points[0] = controlPoints.ToPoint4DList();

                for (var r = 1; r <= h; r++)
                {
                    /*points[r] = new List<Point4D>(k - s + 1);
                    for (var i = 0; i <= k - s + 1; i++)
                    {
                        points[r].Add(new Point4D(-1, -1, -1, -1));
                    }*/

                    points[r] = controlPoints.ToPoint4DList();

                    for (var i = k - p + r; i <= k - s; i++)
                    {
                        if (points[r - 1][i - 1] == null || points[r - 1][i] == null)
                        {
                            continue;
                        }

                        // ReSharper disable once InconsistentNaming
                        var a_ir = (double)((u - knotVector[i]) / (knotVector[i + p - r + 1] - knotVector[i]));

                        var x = ((1 - a_ir) * points[r - 1][i - 1].X) + (a_ir * points[r - 1][i].X);
                        var y = ((1 - a_ir) * points[r - 1][i - 1].Y) + (a_ir * points[r - 1][i].Y);
                        var z = ((1 - a_ir) * points[r - 1][i - 1].Z) + (a_ir * points[r - 1][i].Z);
                        var weight = ((1 - a_ir) * points[r - 1][i - 1].Weight) + (a_ir * points[r - 1][i].Weight);

                        points[r][i] = new Point4D(x, y, z, weight);
                    }
                }

                point = points[h][k - s].ToNurbsPoint();
            }
            else
            {
                // ReSharper disable once UnthrowableException
                throw new IndexOutOfRangeException("u not in [u_k, u_k+1)");
            }

            return point;
        }

        public static List<Point3D> GetCurvePoints(int p, List<NurbsPoint> controlPoints, decimal[] knotVector)
        {
            var steps = controlPoints.Count * 20;
            return GetCurvePoints(p, controlPoints, knotVector, steps);
        }

        public static List<Point3D> GetCurvePoints(int p, List<NurbsPoint> controlPoints, decimal[] knotVector, int steps)
        {
            if (p < 2)
            {
                if (controlPoints.Count == 0)
                {
                    return null;
                }
                return controlPoints.ToPoint3DList();
            }

            var curvePoints = new List<Point3D> { controlPoints.First().ToPoint3D() };

            var step = 1M / steps;

            for (var u = 0M; u < 1; u += step)
            {
                var point = DeBoor(p, controlPoints, u, knotVector).ToPoint3D();
                curvePoints.Add(point);
                curvePoints.Add(point);
            }

            curvePoints.Add(controlPoints.Last().ToPoint3D());
            return curvePoints;
        }
    }
}
