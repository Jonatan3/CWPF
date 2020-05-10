using System;
using System.Diagnostics;
using System.Net.Security;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CWPF
{
    public class Coin
    {
        protected Ellipse shape;
        protected Canvas jonaCanvas;
        protected double x, x_, y, y_, radius, point;
        protected TextBlock coinText;
        protected Random rand = new Random();

        public virtual Ellipse Shape { get; set; }
        public virtual double X { get; set; }
        public virtual double Y { get; set; }
        public virtual int Point { get; set; }

        public Coin(Ellipse shape, Canvas jonaCanvas, double y, double x, double radius, int point)
        {
            this.shape = shape;
            this.jonaCanvas = jonaCanvas;
            this.y = y;
            this.x = x;
            this.radius = radius;
            this.point = point;
            IniShape();
            IniCoinText();
        }


        protected void IniShape()
        {
            shape.Name = "coin";
            shape.Height = radius*2;
            shape.Width = radius*2;
            shape.StrokeThickness = 1;
            shape.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d9a760"));
            shape.Stroke = new SolidColorBrush(Colors.Black);
            jonaCanvas.Children.Add(shape);
            Canvas.SetLeft(shape, x);
            Canvas.SetTop(shape, y);
        }

        void IniCoinText()
        {
            coinText = new TextBlock();
            coinText.FontSize = 10;
            coinText.Inlines.Add(new Bold(new Run("bold")));
            coinText.HorizontalAlignment = HorizontalAlignment.Left;
            coinText.VerticalAlignment = VerticalAlignment.Top;
            x_ = x + radius - coinText.FontSize / 4;
            y_ = y + radius - coinText.FontSize / 2 - shape.StrokeThickness;
            coinText.Text = point.ToString();
            Canvas.SetTop(coinText, y_);
            Canvas.SetLeft(coinText, x_);
            jonaCanvas.Children.Add(coinText);
           
        }
    }
}
