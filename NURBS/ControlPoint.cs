// ReSharper disable StyleCop.SA1600
namespace NURBS
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class ControlPoint : ContentControl
    {
        public ControlPoint()
        {
            this.Width = 4;
            this.Height = 4;
            this.Template = Application.Current.FindResource("DesignerItemTemplate") as ControlTemplate;
            var eli = new Ellipse
                          {
                              IsHitTestVisible = false,
                              Fill = Brushes.Red
                          };
            this.Content = eli;
        }

        public uint Id { get; set; }

        public double X => Canvas.GetLeft(this) + (this.Width / 2);

        public double Y => Canvas.GetTop(this) + (this.Height / 2);

        public double Weight { get; set; } = 1d;

        public NurbsPoint GetNurbsPoint => new NurbsPoint(this.X, this.Y, 0, this.Weight);
    }
}
