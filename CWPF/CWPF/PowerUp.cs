using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #endregion

        public PowerUp(Rectangle body, Canvas jonaCanvas, double y, double x)
        {
            this.body = body;
            this.jonaCanvas = jonaCanvas;
            this.y = y;
            this.x = x;
            IniShape();
        }

        protected void IniShape()
        {
            body.Height = 20;
            body.Width = 20;
            body.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8856d0"));
            body.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000 "));
            body.StrokeThickness = 0.5;
            jonaCanvas.Children.Add(body);
            Canvas.SetLeft(body, x);
            Canvas.SetTop(body, y);
        }
    }
}
