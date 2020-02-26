// ReSharper disable StyleCop.SA1600
// ReSharper disable StyleCop.SA1306
// ReSharper disable InconsistentNaming
// ReSharper disable StyleCop.SA1305
// ReSharper disable CompareOfFloatsByEqualityOperator
namespace NURBS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class SpaceLogic
    {
        private static NurbsPoint RotateY(NurbsPoint P0, double theta)
        {
            var radians = theta * Math.PI / 180;
            var x = (P0.X * Math.Cos(radians)) - (P0.Z * Math.Sin(radians));
            var z = (P0.X * Math.Sin(radians)) + (P0.Z * Math.Cos(radians));
            return new NurbsPoint(x, P0.Y, z, P0.Weight);
        }

        private static NurbsPoint MiddlePoint(NurbsPoint P0, double theta)
        {
            var radians = theta * Math.PI / 180;
            var beta = Math.Atan2(P0.Z, P0.X);
            var x = (P0.X * Math.Cos(-beta)) - (P0.Z * Math.Sin(-beta));
            var z = x * Math.Tan(radians / 2);
            var x1 = (x * Math.Cos(beta)) - (z * Math.Sin(beta));
            var z1 = (x * Math.Sin(beta)) + (z * Math.Cos(beta));

            var w1 = P0.Weight * Math.Cos(radians / 2);
            return new NurbsPoint(x1, P0.Y, z1, w1);
        }

        public static List<NurbsPoint> Arc(NurbsPoint P0, int phi, out decimal[] knotVector)
        {
            var arcs = 0;
            knotVector = new decimal[] { };

            if (phi <= 90)
            {
                arcs = 1;
                knotVector = new[] { 0M, 0, 0, 1, 1, 1 };
            }
            else if (phi <= 180)
            {
                arcs = 2;
                knotVector = new[] { 0, 0, 0, 1M / 2, 1M / 2, 1, 1, 1 };
            }
            else if (phi <= 270)
            {
                arcs = 3;
                knotVector = new[] { 0, 0, 0, 1M / 3, 1M / 3, 2M / 3, 2M / 3, 1, 1, 1 };
            }
            else if (phi <= 360)
            {
                arcs = 4;
                knotVector = new[] { 0, 0, 0, 1M / 4, 1M / 4, 2M / 4, 2M / 4, 3M / 4, 3M / 4, 1, 1, 1 };
            }

            var theta = phi / (double)arcs;
            var pointsList = new List<NurbsPoint> { P0 };

            if (arcs >= 1)
            {
                var P1 = SpaceLogic.MiddlePoint(pointsList.LastOrDefault(), theta);
                var P2 = SpaceLogic.RotateY(pointsList.LastOrDefault(), theta);
                pointsList.Add(P1);
                pointsList.Add(P2);
            }

            if (arcs >= 2)
            {
                var P3 = SpaceLogic.MiddlePoint(pointsList.LastOrDefault(), theta);
                var P4 = SpaceLogic.RotateY(pointsList.LastOrDefault(), theta);
                pointsList.Add(P3);
                pointsList.Add(P4);
            }

            if (arcs >= 3)
            {
                var P5 = SpaceLogic.MiddlePoint(pointsList.LastOrDefault(), theta);
                var P6 = SpaceLogic.RotateY(pointsList.LastOrDefault(), theta);
                pointsList.Add(P5);
                pointsList.Add(P6);
            }

            if (arcs >= 4)
            {
                var P7 = SpaceLogic.MiddlePoint(pointsList.LastOrDefault(), theta);
                var P8 = SpaceLogic.RotateY(pointsList.LastOrDefault(), theta);
                pointsList.Add(P7);
                pointsList.Add(P8);
            }

            return pointsList;
        }

        public static NurbsPoint DeBoorSurface(
            List<List<NurbsPoint>> controlNet,
            decimal u,
            decimal[] knotU,
            decimal v,
            decimal[] knotV)
        {
            var cPointsU = controlNet.First();
            var cPointsV = controlNet.Transpose().First();

            var p = knotU.Length - cPointsU.Count - 1;
            var q = knotV.Length - cPointsV.Count - 1;

            u = Math.Round(u, 21);
            v = Math.Round(v, 21);

            var magic = 0;
            switch (u)
            {
                case 0:
                    magic += 1;
                    break;
                case 1:
                    magic += 3;
                    break;
                default:
                    magic += 2;
                    break;
            }

            switch (v)
            {
                case 0:
                    magic += 0;
                    break;
                case 1:
                    magic += 6;
                    break;
                default:
                    magic += 3;
                    break;
            }

            switch (magic)
            {
                case 1:
                    return controlNet.First().First();
                case 2:
                    return NurbsLogic.DeBoor(p, controlNet.First(), u, knotU);
                case 3:
                    return controlNet.First().Last();
                case 4:
                    return NurbsLogic.DeBoor(q, controlNet.Transpose().First(), v, knotV);
                case 5:
                    break;
                case 6:
                    return NurbsLogic.DeBoor(q, controlNet.Transpose().Last(), v, knotV);
                case 7:
                    return controlNet.Last().First();
                case 8:
                    return NurbsLogic.DeBoor(p, controlNet.Last(), u, knotU);
                case 9:
                    return controlNet.Last().Last();
                default:
                    break;
            }

            var c = NurbsLogic.FindSpan(u, knotU);
            var d = NurbsLogic.FindSpan(v, knotV);

            var s = NurbsLogic.CheckMultiplicity(knotU, u);
            var t = NurbsLogic.CheckMultiplicity(knotV, v);

            NurbsPoint point;
            if (u.GreaterThanOrEqualTo(knotU[c]) && u.LessThan(knotU[c + 1]) && v.GreaterThanOrEqualTo(knotV[d]) && v.LessThan(knotV[d + 1]))
            {
                var P = new NurbsPoint[c - s + 1, d - t + 1];
                var Q = new NurbsPoint[c - s + 1];

                for (var i = 0; i < P.GetLength(0); i++)
                {
                    for (var j = 0; j < P.GetLength(1); j++)
                    {
                        P[i, j] = controlNet[j][i];
                    }
                }

                for (var i = 0; i < P.GetLength(0); i++)
                {
                    var Pi = new List<NurbsPoint>();
                    for (var j = 0; j < P.GetLength(1); j++)
                    {
                        Pi.Add(P[i, j]);
                    }

                    Q[i] = NurbsLogic.DeBoor(q, Pi, v, knotV);
                }

                point = NurbsLogic.DeBoor(p, Q.ToList(), u, knotU);
            }
            else
            {
                // ReSharper disable once UnthrowableException
                throw new IndexOutOfRangeException("u not in [u_c, u_c+1) or v not in [u_d, u_d+1)");
            }

            return point;
        }
    }
}
