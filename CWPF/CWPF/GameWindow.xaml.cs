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
        
        private double gravity = 0.1;
        private double friction = 0.99;
        private int time = 0, realScore = 500;
        private TextBlock scoreText;
        private double startY;

        #region Constructures
        public GameWindow()
        {
            InitializeComponent();

            double nativeWidth = ((Panel)Application.Current.MainWindow.Content).ActualWidth;
            double nativeHeight = ((Panel)Application.Current.MainWindow.Content).ActualHeight;
            jonaCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            jonaCanvas.Arrange(new Rect(0, 0, nativeWidth, nativeHeight));
            startY = jonaCanvas.ActualHeight * (2.0 / 3.0);
            jumpingJona = new JumpingJonaFastState(new Ellipse(), jonaCanvas, startY);

            Rectangle grass = new Rectangle();
            grass.Height = jonaCanvas.ActualHeight * (1.0 / 3.0) - jumpingJona.Body.Height / 2;
            grass.Width = jonaCanvas.ActualWidth;
            grass.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6ea147"));
            startY = jonaCanvas.ActualHeight * 0.66;
            Console.WriteLine("hej " + jonaCanvas.ActualHeight);

            StartTimers();
        }
        #endregion

        #region Private Methods
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
                jumpingJona.MoveLeft();
            if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
                jumpingJona.MoveRight();
            if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Space))
                jumpingJona.Jump();
        }


        private void UpdateScreen(object sender, EventArgs e)
        {

            if (jumpingJona.Y + jumpingJona.Body.Height/2 + jumpingJona.VertSpeed >= startY)
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
        #endregion

        #region Clock Timers
        private void StartTimers()
        {
            DispatcherTimer miliSecTimer = new DispatcherTimer();
            miliSecTimer.Interval = TimeSpan.FromMilliseconds(1);
            miliSecTimer.Tick += new EventHandler(MoveJumpingJona);
            miliSecTimer.Tick += UpdateScreen;
            miliSecTimer.Start();

            DispatcherTimer secTimer = new DispatcherTimer();
            secTimer.Interval = TimeSpan.FromSeconds(1);
            lblTime.Content = TimeSpan.FromSeconds(0);
            secTimer.Tick += StartClock;
            IniScoreCounter();
            scoreText.Text = realScore.ToString();
            secTimer.Tick += UpdateScore;
            secTimer.Start();
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
        #endregion
    }
}
