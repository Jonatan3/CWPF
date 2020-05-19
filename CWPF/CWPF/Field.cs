using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;


namespace CWPF
{
    public class Field
    {
        protected Rectangle box;
        protected Canvas jonaCanvas;
        protected double x, y;
        protected int fieldSize = 30, size;

        #region Constructors
        public Field(Rectangle box, Canvas jonaCanvas, double y, double x, int size)
        {
            this.box = box;
            this.jonaCanvas = jonaCanvas;
            this.y = y;
            this.x = x;
            this.size = size;
            IniShape();
        }
        #endregion
        #region Properties
        public virtual Rectangle Box
        {
            get { return box; }
            set { this.box = value; }
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
        #region Protected methods
        protected void IniShape()
        {
            box.Height = fieldSize;
            box.Width = fieldSize * size;
            box.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6ea147"));
            box.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000 "));
            box.StrokeThickness = 0.5;
            jonaCanvas.Children.Add(box);
            Canvas.SetLeft(box, x);
            Canvas.SetTop(box, y);
        }
        #endregion
    }
}

