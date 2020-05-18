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
            body.StrokeThickness = 0.5;
            body.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#da5f49"));
            body.Stroke = new SolidColorBrush(Colors.Black);
            jonaCanvas.Children.Add(body);
            Canvas.SetLeft(body, x);
            Canvas.SetTop(body, y);
        }
        #endregion

        public void MoveRight()
        {
            x += 4;
            Canvas.SetLeft(body, x);
        }

        public void MoveLeft()
        {
            x -= 4;
            Canvas.SetLeft(body, x);
        }

        public void MakeBobBounce(Field[] fieldArray, double startY, int margins, double gravity)
        {
            if (this.Y + this.VertSpeed >= startY) // Græs
            {
                this.VertSpeed = -this.VertSpeed * 0.98;
            }
            else if (this.Y + this.VertSpeed <= margins)// Top
            {
                this.Y += 2;
                this.VertSpeed = -this.VertSpeed;
            }
            else
            {
                this.VertSpeed += gravity;
            }

            this.Y += this.VertSpeed;
            Canvas.SetTop(this.Body, this.Y);
        }
        public void CheckCollision(Field currField, double gravity)
        {
            if (this.Y >= currField.Y + currField.Box.Height - 5 &&
                        this.X + this.Body.Width >= currField.X &&
                        this.X <= currField.X + currField.Box.Width) // Under field
            {
                this.Y += 2;
                this.VertSpeed = -this.VertSpeed;

            }
            else if (this.Y + this.Body.Height <= currField.Y + 10 &&
                this.X + this.Body.Width >= currField.X + 10 &&
                this.X <= currField.X + currField.Box.Width - 10) // Over field 
            {
                this.VertSpeed = -gravity - this.VertSpeed * 0.99;

            }
            else if (this.X + this.Body.Width >= currField.X &&
                this.X + this.Body.Width <= currField.X + 5 &&
                this.Y <= currField.Y + currField.Box.Height &&
                this.Y + this.Body.Height >= currField.Y) //venstre side
            {
                this.MoveLeft();
            }
            else if (this.X <= currField.X + currField.Box.Width &&
                this.X >= currField.X + currField.Box.Width - 5 &&
                this.Y <= currField.Y + currField.Box.Height &&
                this.Y + this.Body.Height >= currField.Y) //Højre side
            {
                this.MoveRight();
            }
            else if (this.Y > currField.Y + currField.Box.Height &&
              this.X > currField.X + currField.Box.Width) //SØ hjørne
            {
                this.MoveRight();
                this.Y += 2;
                this.VertSpeed = -this.VertSpeed;
            }
            else if (this.Y > currField.Y + currField.Box.Height &&
              this.X + this.Body.Width < currField.X) // SV Hjørne 
            {
                this.MoveLeft();
                this.Y += 2;
            }
            else if (this.Y + this.Body.Height < currField.Y &&
              this.X + this.Body.Width < currField.X) // NV hjørne 
            {
                this.MoveLeft();
            }
            else if (this.Y + this.Body.Height < currField.Y &&
                  this.X > currField.X + currField.Box.Width) //NE
            {
                this.MoveRight();
            }
        }
    }
}
