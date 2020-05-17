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
        protected double x, x_, y, y_, radius;
        protected int point;
        protected TextBlock coinText;
        protected Random rand = new Random();

        #region Properties
        public virtual Ellipse Shape {
            get { return shape; }
            set { this.shape = value; } 
        }
        public virtual double X {
            get { return x; } 
            set { this.x = value; }
        }
        public virtual double Y {
            get { return y; }
            set { this.Y = value; }
        }
        public virtual int Point {
            get { return point; }
            set { this.point = value; }
        }
        public virtual TextBlock CoinText
        {
            get { return coinText; }
            set { this.coinText.Text = value.ToString(); }
        }
        #endregion
        #region Constructor 
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
        #endregion
        #region Private methods
        private void IniShape()
        {
            shape.Name = "coin";
            shape.Height = radius*2;
            shape.Width = radius*2;
            shape.StrokeThickness = 0.5;
            shape.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d9a760"));
            shape.Stroke = new SolidColorBrush(Colors.Black);
            jonaCanvas.Children.Add(shape);
            Canvas.SetLeft(shape, x);
            Canvas.SetTop(shape, y);
        }

        private void IniCoinText()
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
        #endregion
    }
}
