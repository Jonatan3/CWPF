using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace CWPF
{
    public class JumpingJonaSlowState : JumpingJona
    {
        public JumpingJonaSlowState(Ellipse body, Canvas jonaCanvas) : base(body, jonaCanvas)
        {
        }

        public override void Jump()
        {
            vertSpeed = -2;
            y += vertSpeed;
            Canvas.SetTop(body, y);
        }

        public override void MoveRight()
        {
            x += 2;
            Canvas.SetLeft(body, x);
        }

        public override void MoveLeft()
        {
            x -= 2;
            Canvas.SetLeft(body, x);
        }

        #region Properties
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
