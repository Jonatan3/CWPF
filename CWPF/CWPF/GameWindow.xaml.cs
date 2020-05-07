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
        private JumpingJona jumpingJona;
        private double startY;
        private double gravity = 0.1;
        private double friction = 0.99;
        private int time = 0, realScore = 500;
        private TextBlock scoreText;

        public GameWindow()
        {
            InitializeComponent();
            
            double nativeWidth = ((Panel)Application.Current.MainWindow.Content).ActualWidth;
            double nativeHeight = ((Panel)Application.Current.MainWindow.Content).ActualHeight;
            Console.WriteLine("hej " + nativeWidth);
            jonaCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            jonaCanvas.Arrange(new Rect(0, 0, nativeWidth, nativeHeight));
            startY = jonaCanvas.ActualHeight * 0.66; 
            Console.WriteLine("hej " + jonaCanvas.ActualHeight);

            jumpingJona = new JumpingJona(new Ellipse(), jonaCanvas);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
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

            IniScoreCounter();
            DispatcherTimer score = new DispatcherTimer();
            scoreText.Text = realScore.ToString();
            clock.Interval = TimeSpan.FromSeconds(1);
            clock.Tick += UpdateScore;
            clock.Start();
        }

        private void IniScoreCounter()
        {
            scoreText = new TextBlock();
            scoreText.FontSize = 20;
            scoreText.HorizontalAlignment = HorizontalAlignment.Left;
            scoreText.VerticalAlignment = VerticalAlignment.Top;
            jonaCanvas.Children.Add(scoreText);
        }

        private void MoveJumpingJona(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A))
            {
                jumpingJona.MoveLeft();
            }
            if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
            {
                jumpingJona.MoveRight();
            }
            if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Space))
            {
                jumpingJona.Jumb();
            }
        }


        private void UpdateScreen(object sender, EventArgs e)
        {

            if (jumpingJona.Y + jumpingJona.Body.Height/2 + jumpingJona.Y >= startY)
            {
                jumpingJona.VertSpeed = -gravity;
                jumpingJona.VertSpeed *= friction;
               
            } else
            {
                jumpingJona.VertSpeed += gravity;
            }
            jumpingJona.Y += jumpingJona.VertSpeed;
            Canvas.SetTop(jumpingJona.Body, jumpingJona.Y);

        }

        private void StartClock(object sender, EventArgs e) 
        {
            time = ++time;
            lblTime.Content = TimeSpan.FromSeconds(time);
        }

        private void UpdateScore(object sender, EventArgs e)
        {
            realScore -= 5;
            scoreText.Text = realScore.ToString();
        }
    }
}
