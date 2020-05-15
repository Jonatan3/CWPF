using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CWPF
{
    class BouncingBob
    {
        protected Ellipse body;
        protected double vertSpeed, x, y;
        protected Canvas jonaCanvas;

        #region Constructures
        public BouncingBob(Ellipse body, Canvas jonaCanvas,double y, double x)
        {
            this.body = body;
            this.jonaCanvas = jonaCanvas;
            this.x = x;
            this.y = y;
            IniBody();
        }
        #endregion
        #region Properties

        public virtual Ellipse Body
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
            set { this.y = value; }
        }
        public virtual double VertSpeed
        {
            get { return vertSpeed; }
            set { this.vertSpeed = value; }
        }

        #endregion
        #region Private methods
        protected void IniBody()
        {
            body.Name = "bouncingBob";
            body.Height = 25;
            body.Width = 25;
            body.StrokeThickness = 1;
            body.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#da5f49"));
            body.Stroke = new SolidColorBrush(Colors.Black);
            jonaCanvas.Children.Add(body);
            Canvas.SetLeft(body, x);
            Canvas.SetTop(body, y);
        }
        #endregion

        public virtual void MoveRight()
        {
            x += 4;
            Canvas.SetLeft(body, x);
        }

        public virtual void MoveLeft()
        {
            x -= 4;
            Canvas.SetLeft(body, x);
        }
    }
}
