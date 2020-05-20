using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CWPF
{
    public abstract class JumpingJona : IJumpingJona
    {
        protected Ellipse body;
        protected double vertSpeed, x = 25, y = 0.0, startY = 0.0;
        protected Canvas jonaCanvas;
        protected bool canJump;

        #region Constructures
        public JumpingJona(Ellipse body, Canvas jonaCanvas, double startY) {
            this.body = body;
            this.jonaCanvas = jonaCanvas;
            this.startY = startY;
            IniBody();
        }
        #endregion
        #region Properties
        public virtual double StartY
        {
            get { return startY; }
            set { this.startY = value; }
        }
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
        public virtual bool CanJump
        {
            get { return canJump; }
            set { this.canJump = value; }
        }
        #endregion
        #region Private methods
        // Initiates body of jumpingjona
        protected void IniBody()
        {
            body.Name = "jumpingJona";
            body.Height = 50;
            body.Width = 50;
            body.StrokeThickness = 0.5;
            body.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2185C5"));
            body.Stroke = new SolidColorBrush(Colors.Black);
            jonaCanvas.Children.Add(body);
            y = startY - body.Height/2;
            Canvas.SetLeft(body, x);
            Canvas.SetTop(body, y);
        }
        #endregion
        #region Public Methods
        // Jona jump
        public virtual void Jump()
        {
        }
        // Jona move right
        public virtual void MoveRight()
        {
        }
        // Jona move left
        public virtual void MoveLeft()
        {
        }
        // Moves jumpingjona on the canvas
        public void MoveJumpingJona(double grassTop, double gravity, int margins)
        {
            if (this.Y + this.Body.Height + this.VertSpeed >= grassTop) // Græs
            {
                this.VertSpeed = 0;
                this.CanJump = true;
            }
            else if (this.Y <= 0)
            { // Top
                this.Y += 2;
                this.VertSpeed = -this.VertSpeed * gravity;
            }
            else
            {
                this.VertSpeed += gravity;
            }

            this.Y += this.VertSpeed;
            Canvas.SetTop(this.Body, this.Y);

            if (this.X + this.Body.Width / 2.0 + this.VertSpeed <= margins) // Venstre
            {
                this.MoveRight();
            }
            else if (this.X + this.Body.Width / 2 + this.VertSpeed >= jonaCanvas.ActualWidth - margins)  //Højre
            {
                this.MoveLeft();
            }
        }
        // Checks collsion with fields, and handels the collision
        public void CheckCollision(Field currField, double gravity)
        {
            if (this.Y >= currField.Y + currField.Box.Height - 5 &&
                        this.X + this.Body.Width >= currField.X &&
                        this.X <= currField.X + currField.Box.Width) // Under field
            {
                this.Y += 2;
                this.VertSpeed = -this.VertSpeed * gravity;
            }
            else if (this.Y + this.Body.Height <= currField.Y + 10 &&
                this.X + this.Body.Width >= currField.X + 10 &&
                this.X <= currField.X + currField.Box.Width - 10) // Over field 
            {
                this.VertSpeed = -gravity;
                this.CanJump = true;

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
                this.VertSpeed = -this.VertSpeed * gravity;
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
                  this.X > currField.X + currField.Box.Width)
            {
                this.MoveRight();
            }
        }
        #endregion
    }
}
