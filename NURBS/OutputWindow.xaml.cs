// ReSharper disable StyleCop.SA1600
// ReSharper disable InconsistentNaming
// ReSharper disable StyleCop.SA1300
// ReSharper disable StyleCop.SA1306

// ReSharper disable StyleCop.SA1305

using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace NURBS
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    using HelixToolkit.Wpf;

    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    // ReSharper disable once InheritdocConsiderUsage
    public partial class OutputWindow : Window
    {
        public OutputWindow(int degree, List<NurbsPoint> controlPoints, int angle, decimal[] dCanvasKnotVector)
        {
            this.InitializeComponent();

            this.Degree = degree;
            this.ControlPoints = controlPoints;
            this.Angle = angle;
            this.CurveKnotVector = dCanvasKnotVector;

            this.Render();
        }

        private decimal[] CurveKnotVector { get; set; }

        private int Angle { get; }

        private List<NurbsPoint> ControlPoints { get; }

        private int Degree { get; }

        private void Render()
        {
            this.HelixView.Children.Clear();
            var controlNet = new List<List<NurbsPoint>>();
            var circleKnotVector = new decimal[1];

            foreach (var cp in this.ControlPoints)
            {
                controlNet.Add(SpaceLogic.Arc(cp, this.Angle, out circleKnotVector));
            }

            if (this.MenuItemAxis.IsChecked)
            {
                var axis = new LinesVisual3D
                {
                    Color = Colors.Red,
                    Points = new Point3DCollection(new List<Point3D>
                    {
                        new Point3D(0, -1000d, 0),
                        new Point3D(0, 1000d, 0)
                    }),
                    Thickness = 1
                };
                this.HelixView.Children.Add(axis);
            }

            if (this.MenuItemNet.IsChecked)
            {
                this.DrawControlNet(controlNet);
            }

            if (this.MenuItemCurve.IsChecked)
            {
                var points =
                    new Point3DCollection(NurbsLogic.GetCurvePoints(this.Degree, this.ControlPoints, CurveKnotVector));
                var curve = new LinesVisual3D {Color = Colors.BlueViolet, Points = points, Thickness = 1};
                this.HelixView.Children.Add(curve);
            }

            if (this.MenuItemPar.IsChecked)
            {
                this.DrawParallels(controlNet, circleKnotVector);
            }

            if (this.MenuItemMer.IsChecked)
            {
                this.DrawMeridians(controlNet, circleKnotVector);
            }
        }

        private void DrawControlNet(List<List<NurbsPoint>> controlNet)
        {
            foreach (var row in controlNet)
            {
                var points = new Point3DCollection(row.ToPoint3DList());
                var polyline = new LinesVisual3D { Color = Colors.DarkGreen, Points = points, Thickness = 1 };
                this.HelixView.Children.Add(polyline);
            }

            foreach (var column in controlNet.Transpose())
            {
                var points = new Point3DCollection(column.ToPoint3DList());
                var polyline = new LinesVisual3D { Color = Colors.DarkGreen, Points = points, Thickness = 1 };
                this.HelixView.Children.Add(polyline);
            }
        }

        private void DrawParallels(List<List<NurbsPoint>> controlNet, decimal[] circleKnotVector)
        {
            var knotU = circleKnotVector;
            var knotV = this.CurveKnotVector;

            var stepV = 1M / (controlNet.Count * 10);

            for (var v = 0M; v.LessThanOrEqualTo(1M); v += stepV)
            {
                var curvePoints = new List<Point3D>();

                var stepU = 1M / (controlNet.First().Count * 10);

                for (var u = 0M; u.LessThanOrEqualTo(1M); u += stepU)
                {
                    var point = SpaceLogic.DeBoorSurface(controlNet, u, knotU, v, knotV).ToPoint3D();
                    curvePoints.Add(point);
                    curvePoints.Add(point);
                }

                curvePoints.RemoveAt(curvePoints.Count - 1);
                curvePoints.RemoveAt(0);

                var pts = new Point3DCollection(curvePoints);
                var curve = new LinesVisual3D { Color = Colors.Blue, Points = pts, Thickness = 1 };
                this.HelixView.Children.Add(curve);
            }
        }

        private void DrawMeridians(List<List<NurbsPoint>> controlNet, decimal[] circleKnotVector)
        {
            var knotU = this.CurveKnotVector;
            var knotV = circleKnotVector;

            controlNet = controlNet.Transpose();

            var stepV = 1M / (controlNet.Count * 10);

            for (var v = 0M; v.LessThanOrEqualTo(1M); v += stepV)
            {
                var curvePoints = new List<Point3D>();

                var stepU = 1M / (controlNet.First().Count * 10);

                for (var u = 0M; u.LessThanOrEqualTo(1M); u += stepU)
                {
                    var point = SpaceLogic.DeBoorSurface(controlNet, u, knotU, v, knotV).ToPoint3D();
                    curvePoints.Add(point);
                    curvePoints.Add(point);
                }

                curvePoints.RemoveAt(curvePoints.Count - 1);
                curvePoints.RemoveAt(0);

                var pts = new Point3DCollection(curvePoints);
                var curve = new LinesVisual3D { Color = Colors.Blue, Points = pts, Thickness = 1 };
                this.HelixView.Children.Add(curve);
            }
        }

        private void MenuExport_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Portable Network Graphics (*.png)|*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                this.HelixView.Viewport.Export(saveFileDialog.FileName, Brushes.WhiteSmoke);
            }
        }

        private void MenuHelp_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("help!");
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e) => this.Render();
    }
}