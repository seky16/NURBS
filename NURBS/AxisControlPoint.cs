// ReSharper disable StyleCop.SA1600
namespace NURBS
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class AxisControlPoint : ContentControl
    {
        public AxisControlPoint()
        {
            this.Width = 6;
            this.Height = 6;
            this.Template = Application.Current.FindResource("DesignerItemTemplate") as ControlTemplate;
            var eli = new Ellipse
                          {
                              IsHitTestVisible = false,
                              Fill = Brushes.LightSteelBlue
                          };
            this.Content = eli;
        }

        public double X => Canvas.GetLeft(this) + (this.Width / 2);
    }
}
