// ReSharper disable StyleCop.SA1600
// ReSharper disable InheritdocConsiderUsage

using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace NURBS
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class InputWindow
    {
        private OutputWindow _outputWindow;

        public InputWindow()
        {
            this.InitializeComponent();
            this.DataGridPoints.ItemsSource = this.DCanvas.ControlPoints;
        }

        public void ResetKnotVector()
        {
            this.DCanvas.KnotVector = NurbsLogic.GenerateKnotVector(this.DCanvas.ControlPoints.Count, this.DCanvas.Degree);
            this.TbKnotVector.Text = string.Join(" ", this.DCanvas.KnotVector.Select(u => System.Math.Round(u, 4)));
        }

        private void DataGridPoints_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "X":
                case "Y":
                    e.Column.IsReadOnly = true;
                    break;
                case "Weight":
                    e.Column.IsReadOnly = false;
                    break;
                default:
                    e.Column.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void DataGridPoints_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var input = ((TextBox)e.EditingElement).Text.Replace(",", ".");
            var parsed = double.TryParse(input, out var result);
            if (e.Column.Header.ToString() != "Weight" || !parsed || result < 0)
            {
                MessageBox.Show(
                    "Weight must be a non-negative number.", ////" between 0 and 1",
                    "Wrong input!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                e.Cancel = true;
                return;
            }

            ((TextBox)e.EditingElement).Text = result.ToString(CultureInfo.InvariantCulture);
        }

        private void DataGridPoints_OnCurrentCellChanged(object sender, EventArgs e)
        {
            this.DCanvas.RedrawCurve();
        }

        private void ButErase_OnClick(object sender, RoutedEventArgs e)
        {
            this.DCanvas.Children.Clear();
            this.DCanvas.ControlPoints.Clear();
            this.DCanvas.Children.Add(this.DCanvas.AxisControlPoint);
            this.DCanvas.Children.Add(this.DCanvas.Axis);
            this.TbDegree.Text = "0";
            this.ResetKnotVector();
        }

        private void ButRotate_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.DCanvas.Degree == 0)
            {
                MessageBox.Show(
                    "Degree cannot be 0.",
                    "Unable to rotate!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                e.Handled = false;
                return;
            }

            this._outputWindow?.Close();

            var list = new List<NurbsPoint>();
            foreach (var point in this.DCanvas.ControlPointsNurbsList)
            {
                var newPoint = point;
                newPoint.X -= this.DCanvas.AxisControlPoint.X;
                list.Add(newPoint);
            }

            this._outputWindow = new OutputWindow((int)this.DCanvas.Degree, list, this.UpDownAngle.Value ?? 360, this.DCanvas.KnotVector);
            this._outputWindow.Show();
        }

        private void TbDegree_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var valid = int.TryParse(this.TbDegree.Text, out var degree);

            if (this.DCanvas != null && degree >= this.DCanvas.ControlPoints.Count && degree != 0)
            {
                MessageBox.Show(
                    "Degree must be lower than control points count.",
                    "Wrong input!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                e.Handled = false;
                return;
            }

            if (valid && this.DCanvas != null)
            {
                this.DCanvas.Degree = degree;
                this.ResetKnotVector();
                this.DCanvas.RedrawCurve();
            }
        }

        private void KnotVectorEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new KnotVectorDialog(this.DCanvas.KnotVector, this.DCanvas.Degree);
            var changed = dialog.ShowDialog() ?? false;

            if (changed)
            {
                this.DCanvas.KnotVector = dialog.KnotVector;
                this.TbKnotVector.Text = string.Join(" ", this.DCanvas.KnotVector.Select(u => System.Math.Round(u, 4)));
                this.DCanvas.RedrawCurve();
            }
        }

        private void TbDegree_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+"); // regex that matches disallowed text
            var allowed = !regex.IsMatch(e.Text);
            e.Handled = !allowed;
        }

        private void BtnExport_OnClick(object sender, RoutedEventArgs e)
        {
            var canvas = this.DCanvas;
            var rect = new Rect(canvas.RenderSize);
            var rtb = new RenderTargetBitmap((int)rect.Right,
                (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);
            //endcode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            //save to memory stream
            var ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Portable Network Graphics (*.png)|*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllBytes(saveFileDialog.FileName, ms.ToArray());
            }
        }

        private void BtnHelp_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("help!");
        }
    }
}
