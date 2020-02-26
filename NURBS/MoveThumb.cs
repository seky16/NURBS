// ReSharper disable StyleCop.SA1600

using System.Globalization;

namespace NURBS
{
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            this.DragDelta += this.OnDragDelta;
            this.MouseRightButtonDown += this.OnMouseRightButtonDown;
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (!((this.DataContext as ControlPoint)?.Parent is InputCanvas inputCanvas))
            {
                return;
            }

            inputCanvas.RemovePoint((ControlPoint)this.DataContext);
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            switch (this.DataContext)
            {
                case ControlPoint point:
                    {
                        if (!(point.Parent is InputCanvas inputCanvas))
                        {
                            return;
                        }

                        var left = Canvas.GetLeft(point) + e.HorizontalChange;
                        var top = Canvas.GetTop(point) + e.VerticalChange;

                        if (left < 0)
                        {
                            left = 0;
                        }

                        if (left > inputCanvas.ActualWidth - 4)
                        {
                            left = inputCanvas.ActualWidth - 4;
                        }

                        if (top < 0)
                        {
                            top = 0;
                        }

                        if (top > inputCanvas.ActualHeight - 4)
                        {
                            top = inputCanvas.ActualHeight - 4;
                        }

                        Canvas.SetLeft(point, left);
                        Canvas.SetTop(point, top);

                        inputCanvas.UpdatePoint(point);
                        inputCanvas.RedrawCurve();
                        break;
                    }

                case AxisControlPoint axisControlPoint:
                    {
                        if (!(axisControlPoint.Parent is InputCanvas inputCanvas))
                        {
                            return;
                        }

                        var left = Canvas.GetLeft(axisControlPoint) + e.HorizontalChange;
                        var top = Canvas.GetTop(axisControlPoint) + e.VerticalChange;

                        if (left < 0)
                        {
                            left = 0;
                        }

                        if (left > inputCanvas.ActualWidth - 6)
                        {
                            left = inputCanvas.ActualWidth - 6;
                        }

                        if (top < 0)
                        {
                            top = 0;
                        }

                        if (top > inputCanvas.ActualHeight - 6)
                        {
                            top = inputCanvas.ActualHeight - 6;
                        }

                        Canvas.SetLeft(axisControlPoint, left);
                        Canvas.SetTop(axisControlPoint, top);

                        ((InputWindow)((DockPanel)((Border)inputCanvas.Parent).Parent).Parent).TextBlockAxis.Text = axisControlPoint.X.ToString(CultureInfo.InvariantCulture);
                        inputCanvas.RedrawAxis();
                        break;
                    }
            }
        }
    }
}
