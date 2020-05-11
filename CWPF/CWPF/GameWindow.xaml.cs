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
        private int time = 60*60, realScore = 0, ranPoint, fieldSize;
        private TextBlock scoreText, clockText;
        private double ranY, ranX, startY, out_, coinRadius = 12.5, x1, x2, y1, y2;
        private Coin[] coinArray = new Coin[25];
        private Field[] fieldArray = new Field[40];

        Random rand = new Random();

        #region Constructures
        public GameWindow()
        {
            InitializeComponent();

            double nativeWidth = ((Panel)Application.Current.MainWindow.Content).ActualWidth;
            double nativeHeight = ((Panel)Application.Current.MainWindow.Content).ActualHeight;
            jonaCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            jonaCanvas.Arrange(new Rect(0, 0, nativeWidth, nativeHeight));

            startY = jonaCanvas.ActualHeight * (2.0 / 3.0);

            jumpingJona = new JumpingJonaSlowState(new Ellipse(), jonaCanvas, startY);
            IniCoins();
            IniFields();

            Console.WriteLine(CheckCollisionRecktangle(fieldArray[0].Box, fieldArray[1].Box));


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
            for (int i = 0; i < 25; i++)
            {
                coinArray[i] = MakeCoin();
            }
        }
        int counter = 0;
        private void IniFields()
        {
            for (int i = 0; i < 40; i++)
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
            if (Keyboard.IsKeyDown(Key.Up) || Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Space))
                jumpingJona.Jump();
        }


        private void UpdateScreen(object sender, EventArgs e)
        {
            if (jumpingJona.Y + jumpingJona.Body.Height / 2 + jumpingJona.VertSpeed >= startY)
            {
                jumpingJona.VertSpeed = -gravity;
                jumpingJona.VertSpeed *= friction;
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

        public bool CheckCollisionEllipses(Ellipse e1, Ellipse e2)
        {
            x1 = Canvas.GetLeft(e1);
            y1 = Canvas.GetTop(e1);
            Rect r1 = new Rect(x1, y1, e1.ActualWidth, e1.ActualHeight);

            x2 = Canvas.GetLeft(e2);
            y2 = Canvas.GetTop(e2);
            Rect r2 = new Rect(x2, y2, e2.ActualWidth, e2.ActualHeight);
            
            return r1.IntersectsWith(r2);
        }

        public bool CheckCollisionRecktangle(Rectangle r_1, Rectangle r_2)
        {
         
            Rect r1 = new Rect(Canvas.GetLeft(r_1), Canvas.GetTop(r_1), r_1.Width, r_1.Height);
            Rect r2 = new Rect(Canvas.GetLeft(r_2), Canvas.GetTop(r_2), r_2.Width, r_2.Height);

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
