using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private int margins = 22;
        private int time = 60*60, realScore = 0, ranPoint, fieldSize, numField = 30, numCoin = 25;
        private TextBlock scoreText, clockText;
        private double ranY, ranX, startY, out_, coinRadius = 12.5;
        private Coin[] coinArray;
        private Field[] fieldArray;
        Random rand = new Random();
        

        #region Constructures
        public GameWindow(bool? hardMode)
        {
            coinArray = new Coin[numCoin];
            fieldArray = new Field[numField];

            InitializeComponent();

            double nativeWidth = ((Panel)Application.Current.MainWindow.Content).ActualWidth;
            double nativeHeight = ((Panel)Application.Current.MainWindow.Content).ActualHeight;
            jonaCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            jonaCanvas.Arrange(new Rect(0, 0, nativeWidth, nativeHeight));

            startY = jonaCanvas.ActualHeight * (2.0 / 3.0);

            if (hardMode == false || hardMode == null)
                jumpingJona = new JumpingJonaFastState(new Ellipse(), jonaCanvas, startY);
            else
                jumpingJona = new JumpingJonaSlowState(new Ellipse(), jonaCanvas, startY);

            IniCoins();
            IniFields();

            Rectangle grass = new Rectangle();
            grass.Height = jonaCanvas.ActualHeight * (1.0/3.0)-jumpingJona.Body.Height/2 -margins;
            grass.Width = jonaCanvas.ActualWidth-margins;
            grass.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6ea147"));
            grass.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#588139 "));
            jonaCanvas.Children.Add(grass);
            Canvas.SetTop(grass, jonaCanvas.ActualHeight - grass.Height -margins);
            StartTimers();

        }
        #endregion

        #region Private Methods
        private void IniScoreCounter()
        {
            scoreText = new TextBlock();
            scoreText.FontSize = 32;
            scoreText.HorizontalAlignment = HorizontalAlignment.Left;
            scoreText.VerticalAlignment = VerticalAlignment.Top;
            scoreText.Margin = new Thickness(10, 50, 0, 0);
            jonaCanvas.Children.Add(scoreText);
        }
        private void IniClock()
        {
            clockText = new TextBlock();
            clockText.FontSize = 48;
            clockText.HorizontalAlignment = HorizontalAlignment.Left;
            clockText.VerticalAlignment = VerticalAlignment.Top;
            clockText.Margin = new Thickness(10, 0, 0, 0);
            jonaCanvas.Children.Add(clockText);
        }

        private void IniCoins()
        {
            for (int i = 0; i < numCoin; i++)
            {
                coinArray[i] = MakeCoin();
            }
        }
        private void IniFields()
        {
            for (int i = 0; i < numField; i++)
            {
                fieldArray[i] = MakeField();

                for(int j = 0; j<i; j++)
                {
                    if (CheckCollisionRecktangle(fieldArray[j].Box, fieldArray[i].Box))
                    {
                        jonaCanvas.Children.Remove(fieldArray[i].Box);
                        fieldArray[i] = MakeField();
                        i--;
                    }
                }
               

            }
            Console.WriteLine();

        }


        private void MoveJumpingJona(object sender, EventArgs e) 
        {
            if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A))
                jumpingJona.MoveLeft();
            if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
                jumpingJona.MoveRight();
            if ((Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Space)) && jumpingJona.CanJump)
            {
                jumpingJona.Jump();
                jumpingJona.CanJump = false;
            }
        }


        private void UpdateScreen(object sender, EventArgs e)
        {

            if (jumpingJona.Y + jumpingJona.Body.Height / 2 + jumpingJona.VertSpeed >= startY) // Når Jona rammer græsset 
            {
                jumpingJona.VertSpeed = -gravity;
                jumpingJona.VertSpeed *= friction;
                jumpingJona.CanJump = true;
            } else if(jumpingJona.Y + jumpingJona.Body.Height/2 + jumpingJona.VertSpeed <= margins){
                jumpingJona.Y += 4;
                jumpingJona.VertSpeed += gravity; 

            } else
            {
                jumpingJona.VertSpeed += gravity;
            }

            jumpingJona.Y += jumpingJona.VertSpeed;
            Canvas.SetTop(jumpingJona.Body, jumpingJona.Y);

            if (jumpingJona.X + jumpingJona.Body.Width/2.0 + jumpingJona.VertSpeed <= margins)
            {
                jumpingJona.MoveRight();
            } else if (jumpingJona.X +jumpingJona.Body.Width/2 + jumpingJona.VertSpeed >= jonaCanvas.ActualWidth -margins)
            {
                jumpingJona.MoveLeft();
            }

            for (int i = 0; i < numField; i++)
            {
                if (CheckCollisionDifferent(fieldArray[i].Box, jumpingJona.Body))
                {
                    if (jumpingJona.Y + jumpingJona.Body.Height <= fieldArray[i].Y)
                    {
                        Console.WriteLine("________________________");
                        jumpingJona.VertSpeed = -gravity;
                        jumpingJona.VertSpeed *= friction;
                    }

                    
                    else if (jumpingJona.Y <= fieldArray[i].Y + fieldArray[i].Box.Height)
                    {
                        jumpingJona.Y += 4;
                        jumpingJona.VertSpeed += gravity;

                    }
                }
            }
        }
        #endregion

        #region Clock Timers
        private void StartTimers()
        {
            IniClock();
            IniScoreCounter();
            DispatcherTimer miliSecTimer = new DispatcherTimer();
            miliSecTimer.Interval = TimeSpan.FromMilliseconds(1);
            miliSecTimer.Tick += new EventHandler(MoveJumpingJona);
            miliSecTimer.Tick += UpdateScreen;
            miliSecTimer.Tick += StartClock;
            miliSecTimer.Tick += UpdateScore;
            miliSecTimer.Start();

            clockText.Text = TimeSpan.FromSeconds(0).ToString();
            scoreText.Text = realScore.ToString();

        }

        private void StartClock(object sender, EventArgs e) 
        {
            time = --time;
            clockText.Text = TimeSpan.FromSeconds(time).ToString();
        }

        private void UpdateScore(object sender, EventArgs e)
        {
            for (int i = 0; i < 25; i++)
            {
                if (CheckCollisionEllipses(jumpingJona.Body, coinArray[i].Shape))
                {
                    realScore += coinArray[i].Point;
                    jonaCanvas.Children.Remove(coinArray[i].Shape);
                    jonaCanvas.Children.Remove(coinArray[i].CoinText);

                    coinArray[i] = MakeCoin();
                }
            }
            scoreText.Text = realScore.ToString();
        }



        private Coin MakeCoin()
        {
            ranX = RandomDoubleFromRange(margins, jonaCanvas.ActualWidth - coinRadius * 2 - margins);
            ranY = RandomDoubleFromRange(startY, coinRadius * 2 + margins);

            if (ranY <= startY && ranY > startY * 2.0 / 3.0) { ranPoint = 1; }
            else if (startY * 2.0 / 3.0 >= ranY && ranY > startY * 1.0 / 3.0) { ranPoint = 2; }
            else if (startY * 1.0 / 3.0 >= ranY) { ranPoint = 3; }
            else { ranPoint = 0; }

            return new Coin(new Ellipse(), jonaCanvas, ranY, ranX, coinRadius, ranPoint);
        }
        private Field MakeField()
        {
            ranX = RandomDoubleFromRange(margins, jonaCanvas.ActualWidth - coinRadius * 2 - margins);
            ranY = RandomDoubleFromRange(startY, coinRadius * 2 + margins);
            fieldSize = rand.Next(1, 3);

            return new Field(new Rectangle(), jonaCanvas, ranY, ranX, fieldSize);
        }

        private double RandomDoubleFromRange(double min, double max)
        {
            out_ = rand.NextDouble() * (max - min) + min;
            return out_;
        }

        public bool CheckCollisionEllipses(Ellipse s1, Ellipse s2)
        {
            Rect r1 = new Rect(Canvas.GetLeft(s1), Canvas.GetTop(s1), s1.Width, s1.Height);
            Rect r2 = new Rect(Canvas.GetLeft(s2), Canvas.GetTop(s2), s2.Width, s2.Height);

            return r1.IntersectsWith(r2);
        }

        public bool CheckCollisionRecktangle(Rectangle s1, Rectangle s2)
        {
            Rect r1 = new Rect(Canvas.GetLeft(s1), Canvas.GetTop(s1), s1.Width, s1.Height);
            Rect r2 = new Rect(Canvas.GetLeft(s2), Canvas.GetTop(s2), s2.Width, s2.Height);

            return r1.IntersectsWith(r2);
        }
        public bool CheckCollisionDifferent(Rectangle s1, Ellipse s2)
        {
            Rect r1 = new Rect(Canvas.GetLeft(s1), Canvas.GetTop(s1), s1.Width, s1.Height);
            Rect r2 = new Rect(Canvas.GetLeft(s2), Canvas.GetTop(s2), s2.Width, s2.Height);

            return r1.IntersectsWith(r2);
        }



        public void testFun() 
        {
            NetComm.Host server = new NetComm.Host(2020);
            server.StartConnection();
        }

            
        #endregion

        
    }
}
