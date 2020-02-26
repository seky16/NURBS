// ReSharper disable StyleCop.SA1600
// ReSharper disable ArrangeStaticMemberQualifier
namespace NURBS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class InputCanvas : Canvas
    {
        private uint _pointId;

        public InputCanvas()
        {
            this.Background = Brushes.White;
            Canvas.SetLeft(this.AxisControlPoint, 100);
            Canvas.SetTop(this.AxisControlPoint, 100);
            this.Children.Add(this.AxisControlPoint);

            this.Axis = new Line
                           {
                               Stroke = Brushes.LightSteelBlue,
                               IsHitTestVisible = false
                           };
            this.RedrawAxis();
            this.Children.Add(this.Axis);
            this.KnotVector = new[] { 0M };
        }

        public int Degree { get; set; }

        public AxisControlPoint AxisControlPoint { get; } = new AxisControlPoint();

        public BindingList<ControlPoint> ControlPoints { get; private set; } = new BindingList<ControlPoint>();

        public List<NurbsPoint> ControlPointsNurbsList
        {
            get
            {
                return this.ControlPoints.Select(controlPoint => controlPoint.GetNurbsPoint).ToList();
            }
        }

        public Line Axis { get; }

        public decimal[] KnotVector { get; set; }

        public void RedrawAxis()
        {
            this.Axis.X1 = this.AxisControlPoint.X;
            this.Axis.Y1 = 0;
            this.Axis.X2 = this.AxisControlPoint.X;
            this.Axis.Y2 = 10000d; // this.ActualHeight;
        }

        public void RemovePoint(ControlPoint point)
        {
            if (!this.ControlPoints.Contains(point))
            {
                // ReSharper disable once UnthrowableException
                throw new Exception();
            }

            this.Children.Remove(point);
            this.ControlPoints.Remove(point);

            var border = (Border)this.Parent;
            var dockPanel = (DockPanel)border.Parent;
            var inputWindow = (InputWindow)dockPanel.Parent;
            if (this.ControlPoints.Count < 2)
            {
                this.Degree = 0;
                inputWindow.TbDegree.Text = this.Degree.ToString();
            }
            else if (this.Degree >= this.ControlPoints.Count)
            {
                this.Degree = this.ControlPoints.Count - 1;
                inputWindow.TbDegree.Text = this.Degree.ToString();
            }

            inputWindow.ResetKnotVector();
            this.RedrawCurve();
        }

        public void UpdatePoint(ControlPoint point)
        {
            var list = this.ControlPoints;
            var p = list.FirstOrDefault(x => x.Id == point.Id);
            var index = list.IndexOf(p);
            list.RemoveAt(index);
            list.Insert(index, point);
            this.ControlPoints = list;
        }

        public void RedrawCurve()
        {
            this.Children.Clear();
            this.Children.Add(this.AxisControlPoint);
            this.Children.Add(this.Axis);

            foreach (var cp in this.ControlPoints)
            {
                this.Children.Add(cp);
            }

            if (this.ControlPoints.Count == 1)
            {
                return;
            }

            var curvePoints = NurbsLogic.GetCurvePoints(this.Degree, this.ControlPointsNurbsList, this.KnotVector);

            if (curvePoints == null)
            {
                return;
            }

            for (var i = 0; i < curvePoints.Count - 1; i++)
            {
                var firstPoint = curvePoints[i];
                var secondPoint = curvePoints[i + 1];
                var line = new Line { Stroke = Brushes.Aqua, IsHitTestVisible = false, X1 = firstPoint.X, Y1 = firstPoint.Y, X2 = secondPoint.X, Y2 = secondPoint.Y };
                this.Children.Add(line);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.CreatePoint(e.GetPosition(this).X, e.GetPosition(this).Y);
            ((InputWindow)Application.Current.MainWindow)?.ResetKnotVector();
            this.RedrawCurve();
        }

        private void CreatePoint(double x, double y)
        {
            var point = new ControlPoint();
            Canvas.SetLeft(point, x);
            Canvas.SetTop(point, y);
            this.Children.Add(point);
            point.Id = this._pointId;
            this._pointId++;
            this.ControlPoints.Add(point);
        }
    }
}