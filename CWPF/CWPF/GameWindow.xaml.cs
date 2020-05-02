using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CWPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Ellipse jumpingJona;
        private double x = 0.0, y = 0.0;
        public GameWindow()
        {
            InitializeComponent();
            IniJumpingJona();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(MoveJumpingJona);
            timer.Start();
            //Thread thread = new Thread(new ThreadStart(MoveJumpingJona));
            //thread.Start();
        }


        private void IniJumpingJona()
        {
            jumpingJona = new Ellipse();
            jumpingJona.Name = "jumpingJona";
            jumpingJona.Height = 50;
            jumpingJona.Width = 50;
            jumpingJona.StrokeThickness = 2;
            jumpingJona.Fill = new SolidColorBrush(Colors.Blue);
            jumpingJona.Stroke = new SolidColorBrush(Colors.Black);
            jonaCanvas.Children.Add(jumpingJona);
            Canvas.SetLeft(jumpingJona, x);
            Canvas.SetTop(jumpingJona, y);

        }

        private void MoveJumpingJona(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Left))
            {
                x -= 0.1;
                Canvas.SetLeft(jumpingJona, x);
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                x += 0.1;
                Canvas.SetLeft(jumpingJona, x);

            }
        }
    }
}
