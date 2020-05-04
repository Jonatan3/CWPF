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
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Ellipse jumpingJona;
        private double x = 350.0, y = 0.0, dy = 0.0;
        private double gravity = 0.1;
        private double friction = 0.99;
        private int time = 0;
        


        public GameWindow()
        {
            InitializeComponent();
            IniJumpingJona();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(MoveJumpingJona);
            timer.Start();

            DispatcherTimer update = new DispatcherTimer();
            update.Interval = TimeSpan.FromMilliseconds(1);
            update.Tick += UpdateScreen;
            update.Start();

            DispatcherTimer clock = new DispatcherTimer();
            lblTime.Content = TimeSpan.FromSeconds(0);
            clock.Interval = TimeSpan.FromSeconds(1);
            clock.Tick += StartClock;
            clock.Start();
        }
    
        private void IniJumpingJona()
        {
            jumpingJona = new Ellipse();
            jumpingJona.Name = "jumpingJona";
            jumpingJona.Height = 50;
            jumpingJona.Width = 50;
            jumpingJona.StrokeThickness = 1;
            jumpingJona.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2185C5"));
            jumpingJona.Stroke = new SolidColorBrush(Colors.Black);
            jonaCanvas.Children.Add(jumpingJona);
            Canvas.SetLeft(jumpingJona, x);
            Canvas.SetTop(jumpingJona, y);

        }

        private void MoveJumpingJona(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A))
            {
                x -= 0.05;
                Canvas.SetLeft(jumpingJona, x);
            }
            if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
            {
                x += 0.05;
                Canvas.SetLeft(jumpingJona, x);
            }
            if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W))
            {
                y -= 0.1;
                Canvas.SetTop(jumpingJona, y);
            }
            if (Keyboard.IsKeyDown(Key.Down))
            {
                y += 0.1;
                Canvas.SetTop(jumpingJona, y);
            }
        }


        private void UpdateScreen(object sender, EventArgs e)
        {

            if (y + jumpingJona.Height/2 +dy >= 300)
            {
                dy = -dy;
                dy *= friction;
            } else
            {
                dy += gravity;
            }
            y += dy;
            Canvas.SetTop(jumpingJona, y);

        }

        private void StartClock(object sender, EventArgs e) 
        {
            time = ++time;
            lblTime.Content = TimeSpan.FromSeconds(time);
        }

    }
}
