using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CWPF
{
    public abstract class JumpingJona : IJumpingJona
    {
        protected Ellipse body;
        protected double vertSpeed, x = 25, y = 0.0, startY = 0.0;
        protected Canvas jonaCanvas;

        public double startY
        {
            get { return startYY; }
            set { startYY = value; }
        }
        #region Constructures
        public JumpingJona(Ellipse body, Canvas jonaCanvas) {
            this.body = body;
            this.jonaCanvas = jonaCanvas;
            this.startY = startY;
            IniBody();
        }
        #endregion

        #region Properties
        public virtual Ellipse Body { get; set; }
        public virtual double X { get; set; }
        public virtual double Y { get; set; }
        public virtual double VertSpeed { get; set; }
        #endregion

        #region Private methods
        protected void IniBody()
        {
            body.Name = "jumpingJona";
            body.Height = 50;
            body.Width = 50;
            body.StrokeThickness = 1;
            body.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2185C5"));
            body.Stroke = new SolidColorBrush(Colors.Black);
            jonaCanvas.Children.Add(body);
            y = startY - body.Height/2;
            Canvas.SetLeft(body, x);
            Canvas.SetTop(body, y);
        }
        #endregion

        #region Public Methods
        public virtual void Jump()
        {
        }

        public virtual void MoveRight()
        {
        }

        public virtual void MoveLeft()
        {
        }
        #endregion
    }
}
