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
    public class JumpingJona : IJumpingJona
    {
        private Ellipse body;
        private double vertSpeed, x = 25, y = 0.0, startYY=0.0;
        private Canvas jonaCanvas;
       public double startY
        {
            get { return startYY; }
            set { startYY = value; }
        }
        public JumpingJona(Ellipse body, Canvas jonaCanvas, double startY) {
            this.body = body;
            this.jonaCanvas = jonaCanvas;
            this.startY = startY;
            IniBody();
        }
        public JumpingJona(Ellipse body, Canvas jonaCanvas, double x, double y, double vertSpeed)
        {
            this.body = body;
            this.jonaCanvas = jonaCanvas;
            this.x = x;
            this.y = y;
            this.vertSpeed = vertSpeed;
        }

        private void IniBody()
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

        public void Jump()
        {
            vertSpeed = -3;
            y += vertSpeed;
            Canvas.SetTop(body, y);
        }

        public void MoveRight()
        {
            x += 1;
            Canvas.SetLeft(body, x);
        }

        public void MoveLeft()
        {
            x -= 1;
            Canvas.SetLeft(body, x);
        }

        public Ellipse Body
        {
            get { return body; }
            set { body = value; }
        }
        public double X
        {
            get { return x; }
            set { y = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

         public double VertSpeed
        {
            get { return vertSpeed; }
            set { vertSpeed = value; }
        }
    }
}
