﻿using System.Windows.Controls;
using System.Windows.Shapes;

namespace CWPF
{
    public class JumpingJonaFastState : JumpingJona, IJumpingJona
    {
        public JumpingJonaFastState(Ellipse body, Canvas jonaCanvas, double startY) : base(body, jonaCanvas, startY)
        {
        }
        #region Override methods
        // Jona jump
        public override void Jump()
        {
            vertSpeed = -5;
            y += vertSpeed;
            Canvas.SetTop(body, y);
        }
        // Jona move right
        public override void MoveRight()
        {
            x += 4;
            Canvas.SetLeft(body, x);
        }
        // Jona move left
        public override void MoveLeft()
        {
            x -= 4;
            Canvas.SetLeft(body, x);
        }
        #endregion
        #region Properties
        public override double StartY { get => base.StartY; set => base.StartY = value; }
        public override Ellipse Body { get => base.Body; set => base.Body = value; }
        public override double X { get => base.X; set => base.X = value; }
        public override double Y { get => base.Y; set => base.Y = value; }
        public override double VertSpeed { get => base.VertSpeed; set => base.VertSpeed = value; }
        public override bool CanJump { get => base.CanJump; set => base.CanJump = value; }
        #endregion
    }
}
