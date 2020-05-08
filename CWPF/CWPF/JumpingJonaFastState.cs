using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace CWPF
{
    public class JumpingJonaFastState : JumpingJona
    {
        public JumpingJonaFastState(Ellipse body, Canvas jonaCanvas, double startY) : base(body, jonaCanvas, startY)
        {
        }
        #region Override methods
        public override void Jump()
        {
            vertSpeed = -3;
            y += vertSpeed;
            Canvas.SetTop(body, y);
        }

        public override void MoveRight()
        {
            x += 4;
            Canvas.SetLeft(body, x);
        }

        public override void MoveLeft()
        {
            x -= 4;
            Canvas.SetLeft(body, x);
        }
        #endregion
        #region Properties
        public override double StartY
        {
            get { return startY; }
            set { this.startY = value; }
        }
        public override Ellipse Body
        {
            get { return body; }
            set { this.body = value; }
        }
        public override double X
        {
            get { return x; }
            set { this.x = value; }
        }
        public override double Y
        {
            get { return y; }
            set { this.y = value; }
        }
        public override double VertSpeed
        {
            get { return vertSpeed; }
            set { this.vertSpeed = value; }
        }
        #endregion
    }
}
