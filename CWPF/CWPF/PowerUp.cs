using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace CWPF
{
    class PowerUp
    {
        protected Rectangle body;
        protected Canvas jonaCanvas;
        protected double x, y;
        protected int power;

        #region Constructures
        public PowerUp(Rectangle body, Canvas jonaCanvas, double y, double x, int power)
        {
            this.body = body;
            this.jonaCanvas = jonaCanvas;
            this.y = y;
            this.x = x;
            this.power = power;
            IniShape();
        }
        #endregion
        #region Properties
        public virtual Rectangle Body
        {
            get { return body; }
            set { this.body = value; }
        }
        public virtual double X
        {
            get { return x; }
            set { this.x = value; }
        }
        public virtual double Y
        {
            get { return y; }
            set { this.Y = value; }
        }
        public virtual double Power
        {
            get { return power; }
            set { this.Power = value; }
        }

        #endregion
        #region Protected method
        protected void IniShape()
        {
            body.Height = 20;
            body.Width = 20;
            body.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000 "));
            body.StrokeThickness = 0.5;
            jonaCanvas.Children.Add(body);
            Canvas.SetLeft(body, x);
            Canvas.SetTop(body, y);

            if (power == 1)
            {
                body.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8856d0"));
            } else if (power == 2)
            {
                body.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dd55e2"));
            }
            else if (power == 3)
            {
                body.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ece819"));
            }
            else if (power == 4)
            {
                body.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            }
        }
        #endregion
    }
}
