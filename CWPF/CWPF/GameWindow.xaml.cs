using Microsoft.CSharp;
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
        DispatcherTimer miliSecTimer = new DispatcherTimer();
        DispatcherTimer tenSecTimer = new DispatcherTimer();
        private JumpingJona jumpingJona;
        private bool power1 = false, power3 = false, power4 = false, PowerExist = false;
        private int time = 60 * 60, realScore = 0, numField = 15, numCoin = 25, numBob = 5, margins = 22;
        private TextBlock scoreText, clockText, powerText = new TextBlock();
        private double ranY, ranX, startY, out_, coinRadius = 12.5, grassTop, gravity = 0.1;
        private Coin[] coinArray;
        private Field[] fieldArray;
        private BouncingBob[] bobArray;
        private PowerUp PU;
        private Random rand = new Random();

        #region Constructures
        public GameWindow(bool? hardMode)
        {
            coinArray = new Coin[numCoin];
            fieldArray = new Field[numField];
            bobArray = new BouncingBob[numBob];
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
            StartTimers();
            IniBackground();
            IniFields();
            IniCoins();
            IniBobs();
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
        private void IniBackground()
        {
            BouncingBob bob;
            Coin coin;
            PowerUp p1, p2, p3, p4;
            TextBlock p1_t = new TextBlock(), p2_t = new TextBlock(), p3_t = new TextBlock(),
                p4_t = new TextBlock(), coin_t = new TextBlock(), bob_t = new TextBlock();
            Rectangle grass = new Rectangle();

            //Background/grass/bottom
            grass.Height = jonaCanvas.ActualHeight * (1.0 / 3.0) - jumpingJona.Body.Height / 2 - margins;
            grass.Width = jonaCanvas.ActualWidth - margins;
            grass.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6ea147"));
            grass.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000 "));
            grass.StrokeThickness = 0.5;
            jonaCanvas.Children.Add(grass);
            grassTop = jonaCanvas.ActualHeight - grass.Height - margins;
            Canvas.SetTop(grass, grassTop);

            bob = new BouncingBob(new Ellipse(), jonaCanvas, jonaCanvas.ActualHeight - 100 , 400);
            BottomText(bob_t, 440, jonaCanvas.ActualHeight - 97, "-10 points");
            coin = new Coin(new Ellipse(), jonaCanvas, jonaCanvas.ActualHeight - 100, 600, coinRadius, 3);
            BottomText(coin_t, 640, jonaCanvas.ActualHeight - 97, "+ points");
            p1 = new PowerUp(new Rectangle(), jonaCanvas, jonaCanvas.ActualHeight - 97.5, 800, 1);
            BottomText(p1_t, 835, jonaCanvas.ActualHeight - 97, "Double Jump");
            p2 = new PowerUp(new Rectangle(), jonaCanvas, jonaCanvas.ActualHeight - 97.5, 1000, 2);
            BottomText(p2_t, 1035, jonaCanvas.ActualHeight - 97, "+5 secs");
            p3 = new PowerUp(new Rectangle(), jonaCanvas, jonaCanvas.ActualHeight - 97.5, 1200, 3);
            BottomText(p3_t, 1235, jonaCanvas.ActualHeight - 97,"Size down");
            p4 = new PowerUp(new Rectangle(), jonaCanvas, jonaCanvas.ActualHeight - 97.5, 1400, 4);
            BottomText(p4_t, 1435, jonaCanvas.ActualHeight - 97, "Size up");
        }
        private void IniCoins()
        {
            for (int i = 0; i < numCoin; i++)
            {
                coinArray[i] = MakeCoin();

                if (CheckCollisionEllipses(coinArray[i].Shape, jumpingJona.Body))
                {
                    jonaCanvas.Children.Remove(coinArray[i].Shape);
                    jonaCanvas.Children.Remove(coinArray[i].CoinText);
                    i--;
                } else
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (CheckCollisionEllipses(coinArray[j].Shape, coinArray[i].Shape))
                        {
                            jonaCanvas.Children.Remove(coinArray[i].Shape);
                            jonaCanvas.Children.Remove(coinArray[i].CoinText);
                            i--;
                        }
                        else
                        {
                            for (int k = 0; k < numField; k++)
                            {
                                if(CheckCollisionDifferent(fieldArray[k].Box,coinArray[i].Shape))
                                {
                                    jonaCanvas.Children.Remove(coinArray[i].Shape);
                                    jonaCanvas.Children.Remove(coinArray[i].CoinText);
                                    i--;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void IniFields()
        {
            for (int i = 0; i < numField; i++)
            {
                fieldArray[i] = MakeField();

                if (CheckCollisionDifferent(fieldArray[i].Box, jumpingJona.Body))
                {
                    jonaCanvas.Children.Remove(fieldArray[i].Box);
                    i--;
                }
                else
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (CheckCollisionRecktangle(fieldArray[j].Box, fieldArray[i].Box))
                        {
                            jonaCanvas.Children.Remove(fieldArray[i].Box);
                            j = i;
                            i--;
                        } 
                    } 
                }
            }
        }
        private void IniBobs()
        {
            for (int i = 0; i < numBob; i++)
            {
                bobArray[i] = MakeBob();

                if (CheckCollisionEllipses(bobArray[i].Body, jumpingJona.Body))
                {
                    jonaCanvas.Children.Remove(bobArray[i].Body);
                    i--;
                }
                else
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (CheckCollisionEllipses(bobArray[i].Body, bobArray[j].Body))
                        {
                            jonaCanvas.Children.Remove(bobArray[i].Body);
                            j = i;
                            i--;
                        } else
                        {
                            for (int k = 0; k < numCoin; k++)
                            {
                                if (CheckCollisionEllipses(coinArray[k].Shape, bobArray[i].Body))
                                {
                                    jonaCanvas.Children.Remove(bobArray[i].Body);
                                    k = numCoin;
                                    j = i;
                                    i--;
                                }
                                else
                                {
                                    for (int l = 0; l < numField; l++)
                                    {
                                        if (CheckCollisionDifferent(fieldArray[l].Box, bobArray[i].Body))
                                        {
                                            jonaCanvas.Children.Remove(bobArray[i].Body);
                                            l = numField;
                                            k = numCoin;
                                            j = i;
                                            i--;
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
                bobArray[i].MakeBobBounce(fieldArray, startY, margins, gravity);
            }
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
            if (Keyboard.IsKeyDown(Key.Enter) && ((jumpingJona.CanJump && power1) || power3 ||power4))
            {
                jonaCanvas.Children.Remove(powerText);
                if (power1)
                {
                    jumpingJona.VertSpeed = -10;
                    jumpingJona.Y += jumpingJona.VertSpeed;
                    jumpingJona.CanJump = false;
                    power1 = false;
                }
                else if (power3)
                {
                    jumpingJona.Body.Height -= 10;
                    jumpingJona.Body.Width -= 10;
                    power3 = false;
                }
                else if (power4)
                {
                    jumpingJona.Body.Height += 10;
                    jumpingJona.Body.Width += 10;
                    jumpingJona.Y -= 10;
                    power4 = false;
                }
            }
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            jumpingJona.MoveJumpingJona(grassTop, gravity, margins);
            for (int i = 0; i < numField; i++)
            {
                if (CheckCollisionDifferent(fieldArray[i].Box, jumpingJona.Body))
                    jumpingJona.CheckCollision(fieldArray[i], gravity);
            }
            if (PowerExist)
            {
                if (CheckCollisionDifferent(PU.Body, jumpingJona.Body))
                {
                    jonaCanvas.Children.Remove(powerText);

                    if (PU.Power == 1)
                    {
                        power1 = true;
                        power3 = false;
                        power4 = false;
                        BottomText(powerText, 880, jonaCanvas.ActualHeight - 200, "Pres ENTER to use Double Jump");
                    }
                    else if (PU.Power == 2)
                    {
                        time += 5 * 60;
                        power1 = false;
                        power3 = false;
                        power4 = false;
                        BottomText(powerText, 900, jonaCanvas.ActualHeight - 200, "You just got +5 secs!");
                    }
                    else if (PU.Power == 3)
                    {
                        power1 = false;
                        power3 = true;
                        power4 = false;
                        BottomText(powerText, 880, jonaCanvas.ActualHeight - 200, "Pres ENTER to use Size Down");
                    }
                    else if (PU.Power == 4)
                    {
                        power1 = false;
                        power3 = false;
                        power4 = true;
                        BottomText(powerText, 880, jonaCanvas.ActualHeight - 200, "Pres ENTER to use Size Up");
                    }
                    jonaCanvas.Children.Remove(PU.Body);
                    PowerExist = false;
                }
            }
        }
        #endregion
        #region Clock Timers
        private void StartTimers()
        {
            IniClock();
            IniScoreCounter();
            miliSecTimer.Interval = TimeSpan.FromMilliseconds(1);
            miliSecTimer.Tick += new EventHandler(MoveJumpingJona);
            miliSecTimer.Tick += UpdateScreen;
            miliSecTimer.Tick += StartClock;
            miliSecTimer.Tick += UpdateScore;
            miliSecTimer.Tick += MakeBobBounce;
            miliSecTimer.Start();

            tenSecTimer.Interval = TimeSpan.FromSeconds(10);
            tenSecTimer.Tick += InputPowerUp;
            tenSecTimer.Start();
            clockText.Text = TimeSpan.FromSeconds(0).ToString();
            scoreText.Text = realScore.ToString();
        }
        #endregion
        private void StartClock(object sender, EventArgs e)
        {
            time = --time;
            clockText.Text = TimeSpan.FromSeconds(time).ToString();
        }
        private void UpdateScore(object sender, EventArgs e)
        {
            for (int i = 0; i < numCoin; i++)
            {
                if (CheckCollisionEllipses(jumpingJona.Body, coinArray[i].Shape))
                {
                    realScore += coinArray[i].Point;
                    jonaCanvas.Children.Remove(coinArray[i].Shape);
                    jonaCanvas.Children.Remove(coinArray[i].CoinText);

                    coinArray[i] = MakeCoin();

                    for (int j = 0; j < numCoin; j++)
                    {
                        if (j == i)
                        {
                        }
                        else if (CheckCollisionEllipses(coinArray[j].Shape, coinArray[i].Shape))
                        {
                            jonaCanvas.Children.Remove(coinArray[i].Shape);
                            jonaCanvas.Children.Remove(coinArray[i].CoinText);
                            coinArray[i] = MakeCoin();
                            j--;
                        }
                        else
                        {
                            for (int k = 0; k < numField; k++)
                            {
                                if (CheckCollisionDifferent(fieldArray[k].Box, coinArray[i].Shape))
                                {
                                    jonaCanvas.Children.Remove(coinArray[i].Shape);
                                    jonaCanvas.Children.Remove(coinArray[i].CoinText);
                                    coinArray[i] = MakeCoin();
                                    j--;
                                    break;
                                }
                                else if (CheckCollisionEllipses(coinArray[i].Shape, jumpingJona.Body))
                                {
                                    jonaCanvas.Children.Remove(coinArray[i].Shape);
                                    jonaCanvas.Children.Remove(coinArray[i].CoinText);
                                    coinArray[i] = MakeCoin();
                                    j--;
                                    break;
                                }
                            }
                        }
                    }
                }  
            }
            for (int i = 0; i < numBob; i++)
            {
                if (CheckCollisionEllipses(jumpingJona.Body, bobArray[i].Body))
                {
                    realScore -= 10;
                    jonaCanvas.Children.Remove(bobArray[i].Body);
                    bobArray[i] = MakeBob();

                    for (int j = 0; j < numBob; j++)
                    { 
                        if (j == i)
                        {
                        }
                        else if (CheckCollisionEllipses(bobArray[j].Body, bobArray[i].Body))
                        {
                            jonaCanvas.Children.Remove(bobArray[i].Body);
                            bobArray[i] = MakeBob();
                            j--;
                        }
                        else
                        {
                            for (int k = 0; k < numCoin; k++)
                            {
                                if (CheckCollisionEllipses(coinArray[k].Shape, bobArray[i].Body))
                                {
                                    jonaCanvas.Children.Remove(bobArray[i].Body);
                                    bobArray[i] = MakeBob();
                                    j--;
                                    k = numCoin;
                                }
                                else
                                {
                                    for (int l = 0; l < numField; l++)
                                    {
                                        if (CheckCollisionDifferent(fieldArray[l].Box, bobArray[i].Body))
                                        {
                                            jonaCanvas.Children.Remove(bobArray[i].Body);
                                            bobArray[i] = MakeBob();
                                            j--;
                                            l = numField;
                                            k = numCoin; 
                                            //Hej hej 

                                        }
                                        else if (CheckCollisionEllipses(bobArray[i].Body, jumpingJona.Body))
                                        {
                                            jonaCanvas.Children.Remove(bobArray[i].Body);
                                            bobArray[i] = MakeBob();
                                            l = numField;
                                            k = numCoin;
                                            j--;

                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            scoreText.Text = realScore.ToString();
        }
        private void InputPowerUp(object sender, EventArgs e)
        {
            if (PowerExist)
                jonaCanvas.Children.Remove(PU.Body); 
            PU = MakePowerUp();
            PowerExist = true;
        }
        private Coin MakeCoin()
        {
            ranX = RandomDoubleFromRange(margins, jonaCanvas.ActualWidth - coinRadius * 2 - margins);
            ranY = RandomDoubleFromRange(startY, coinRadius * 2 + margins);

            int ranPoint;
            if (ranY <= startY && ranY > startY * 2.0 / 3.0) { ranPoint = 1; }
            else if (startY * 2.0 / 3.0 >= ranY && ranY > startY * 1.0 / 3.0) { ranPoint = 2; }
            else if (startY * 1.0 / 3.0 >= ranY) { ranPoint = 3; }
            else { ranPoint = 0; }
            return new Coin(new Ellipse(), jonaCanvas, ranY, ranX, coinRadius, ranPoint);
        }
        private Field MakeField()
        {
            int fieldSize = rand.Next(3, 6);
            ranX = RandomDoubleFromRange(margins, jonaCanvas.ActualWidth - margins - 30*fieldSize);
            ranY = RandomDoubleFromRange(startY - jumpingJona.Body.Height, margins + jumpingJona.Body.Height);
            return new Field(new Rectangle(), jonaCanvas, ranY, ranX, fieldSize);
        }
        private BouncingBob MakeBob()
        {
            ranX = RandomDoubleFromRange(margins, jonaCanvas.ActualWidth - margins - 25);
            ranY = RandomDoubleFromRange(startY - 50, margins + 25);
            return new BouncingBob(new Ellipse(), jonaCanvas, ranY, ranX);
        }
        private PowerUp MakePowerUp()
        {
            int power = rand.Next(1, 5);
            ranX = RandomDoubleFromRange(margins, jonaCanvas.ActualWidth - margins - 20);
            return new PowerUp(new Rectangle(), jonaCanvas, grassTop - 20 , ranX, power);
        }
        private void BottomText(TextBlock text, double x, double y, string str)
        {
            text.FontSize = 14;
            text.Text = str;
            Canvas.SetTop(text, y);
            Canvas.SetLeft(text, x);
            jonaCanvas.Children.Add(text);
        } 
        private void MakeBobBounce(object sender, EventArgs e)
        {
            for (int i = 0; i < numBob; i++)
            {
                bobArray[i].MakeBobBounce(fieldArray, startY, margins, gravity);
                for (int j = 0; j < numField; j++)
                {
                    if (CheckCollisionDifferent(fieldArray[j].Box, bobArray[i].Body))
                        bobArray[i].CheckCollision(fieldArray[j], gravity);
                }
            }
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
    }
}
