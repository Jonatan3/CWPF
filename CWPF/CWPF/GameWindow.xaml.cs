﻿using System;
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
        private int margins = 22;

        private int time = 60 * 60, realScore = 0, ranPoint, fieldSize, numField = 30, numCoin = 40, numBob = 3;
        private TextBlock scoreText, clockText;
        private double ranY, ranX, startY, out_, coinRadius = 12.5;
        private Coin[] coinArray;
        private Field[] fieldArray;
        private BouncingBob[] bobArray;
        Random rand = new Random();


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
            //Background/grass/bottom
            Rectangle grass = new Rectangle();
            grass.Height = jonaCanvas.ActualHeight * (1.0 / 3.0) - jumpingJona.Body.Height / 2 - margins;
            grass.Width = jonaCanvas.ActualWidth - margins;
            grass.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6ea147"));
            grass.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#588139 "));
            jonaCanvas.Children.Add(grass);
            Canvas.SetTop(grass, jonaCanvas.ActualHeight - grass.Height - margins);
            StartTimers();
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
                            i--;
                        } else
                        {
                            for (int k = 0; k < numCoin; k++)
                            {
                                if (CheckCollisionEllipses(coinArray[k].Shape, bobArray[i].Body))
                                {
                                    jonaCanvas.Children.Remove(bobArray[i].Body);
                                    i--;
                                }
                                else
                                {
                                    for (int l = 0; l < numField; l++)
                                    {
                                        if (CheckCollisionDifferent(fieldArray[l].Box, bobArray[i].Body))
                                        {
                                            jonaCanvas.Children.Remove(bobArray[i].Body);
                                            i--;
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
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
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            MakeBobBounce(); 
            if (jumpingJona.Y + jumpingJona.Body.Height / 2 + jumpingJona.VertSpeed >= startY) // Græs
            {
                jumpingJona.VertSpeed = 0;
                jumpingJona.CanJump = true;

            }
            else if (jumpingJona.Y + jumpingJona.Body.Height / 2 + jumpingJona.VertSpeed <= margins)
            { // Top
                jumpingJona.Y += 2;
                jumpingJona.VertSpeed = -jumpingJona.VertSpeed * gravity;

            }
            else
            {
                jumpingJona.VertSpeed += gravity;
            }

            jumpingJona.Y += jumpingJona.VertSpeed;
            Canvas.SetTop(jumpingJona.Body, jumpingJona.Y);

            if (jumpingJona.X + jumpingJona.Body.Width / 2.0 + jumpingJona.VertSpeed <= margins) // Venstre
            {
                jumpingJona.MoveRight();
            }
            else if (jumpingJona.X + jumpingJona.Body.Width / 2 + jumpingJona.VertSpeed >= jonaCanvas.ActualWidth - margins)  //Højre
            {
                jumpingJona.MoveLeft();
            }

            for (int i = 0; i < numField; i++)
            {
                if (CheckCollisionDifferent(fieldArray[i].Box, jumpingJona.Body))
                {
                    if (jumpingJona.Y >= fieldArray[i].Y + fieldArray[i].Box.Height - 5 &&
                        jumpingJona.X + jumpingJona.Body.Width >= fieldArray[i].X &&
                        jumpingJona.X <= fieldArray[i].X + fieldArray[i].Box.Width) // Under field
                    {
                        jumpingJona.Y += 2;
                        jumpingJona.VertSpeed = -jumpingJona.VertSpeed * gravity;
                    }
                    else if (jumpingJona.Y + jumpingJona.Body.Height <= fieldArray[i].Y + 10 &&
                        jumpingJona.X + jumpingJona.Body.Width >= fieldArray[i].X + 10 &&
                        jumpingJona.X <= fieldArray[i].X + fieldArray[i].Box.Width - 10) // Over field 
                    {
                        jumpingJona.VertSpeed = -gravity;
                        jumpingJona.CanJump = true;

                    }
                    else if (jumpingJona.X + jumpingJona.Body.Width >= fieldArray[i].X &&
                        jumpingJona.X + jumpingJona.Body.Width <= fieldArray[i].X + 5 &&
                        jumpingJona.Y <= fieldArray[i].Y + fieldArray[i].Box.Height &&
                        jumpingJona.Y + jumpingJona.Body.Height >= fieldArray[i].Y) //venstre side
                    {
                        jumpingJona.MoveLeft();
                    }
                    else if (jumpingJona.X <= fieldArray[i].X + fieldArray[i].Box.Width &&
                        jumpingJona.X >= fieldArray[i].X + fieldArray[i].Box.Width - 5 &&
                        jumpingJona.Y <= fieldArray[i].Y + fieldArray[i].Box.Height &&
                        jumpingJona.Y + jumpingJona.Body.Height >= fieldArray[i].Y) //Højre side
                    {
                        jumpingJona.MoveRight();
                    }
                    else if (jumpingJona.Y > fieldArray[i].Y + fieldArray[i].Box.Height &&
                      jumpingJona.X > fieldArray[i].X + fieldArray[i].Box.Width) //SØ hjørne
                    {
                        jumpingJona.MoveRight();
                        jumpingJona.Y += 2;
                        jumpingJona.VertSpeed = -jumpingJona.VertSpeed * gravity;
                    }
                    else if (jumpingJona.Y > fieldArray[i].Y + fieldArray[i].Box.Height &&
                      jumpingJona.X + jumpingJona.Body.Width < fieldArray[i].X) // SV Hjørne 
                    {
                        jumpingJona.MoveLeft();
                        jumpingJona.Y += 2;
                    }
                    else if (jumpingJona.Y + jumpingJona.Body.Height < fieldArray[i].Y &&
                      jumpingJona.X + jumpingJona.Body.Width < fieldArray[i].X) // NV hjørne 
                    {
                        jumpingJona.MoveLeft();
                    }
                    else if (jumpingJona.Y + jumpingJona.Body.Height < fieldArray[i].Y &&
                          jumpingJona.X > fieldArray[i].X + fieldArray[i].Box.Width)
                    {
                        jumpingJona.MoveRight();
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

                    for (int j = 0; j < i; j++)
                    {
                        if (CheckCollisionEllipses(coinArray[j].Shape, coinArray[i].Shape))
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
                                }
                                else if (CheckCollisionEllipses(coinArray[i].Shape, jumpingJona.Body))
                                {
                                    jonaCanvas.Children.Remove(coinArray[i].Shape);
                                    jonaCanvas.Children.Remove(coinArray[i].CoinText);
                                    coinArray[i] = MakeCoin();
                                    j--;
                                }
                            }
                        }
                    }
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
            fieldSize = rand.Next(1, 4);
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
        private void MakeBobBounce()
        {
            for (int i = 0; i < numBob; i++)
            {
                if (bobArray[i].Y  >= startY) // Græs
                {
                    bobArray[i].VertSpeed = - bobArray[i].VertSpeed ;
                }
                else if (bobArray[i].Y + bobArray[i].VertSpeed <= margins)
                { // Top
                    bobArray[i].Y += 2;
                    bobArray[i].VertSpeed = -bobArray[i].VertSpeed;
                }
                else
                {
                    bobArray[i].VertSpeed += gravity;
                }

                bobArray[i].Y += bobArray[i].VertSpeed;
                Canvas.SetTop(bobArray[i].Body, bobArray[i].Y);

                if (bobArray[i].X + bobArray[i].VertSpeed <= margins) // Venstre
                {
                    bobArray[i].MoveRight();
                }
                else if (bobArray[i].X + bobArray[i].Body.Width + bobArray[i].VertSpeed >= jonaCanvas.ActualWidth - margins)  //Højre
                {
                    bobArray[i].MoveLeft();
                }

                for (int j = 0; j < numField; j++)
                {
                    if (CheckCollisionDifferent(fieldArray[i].Box, bobArray[i].Body))
                    {
                        if (bobArray[i].Y >= fieldArray[i].Y + fieldArray[i].Box.Height - 5 &&
                            bobArray[i].X + bobArray[i].Body.Width >= fieldArray[i].X &&
                            bobArray[i].X <= fieldArray[i].X + fieldArray[i].Box.Width) // Under field
                        {
                            bobArray[i].Y += 2;
                            bobArray[i].VertSpeed = -bobArray[i].VertSpeed;
                        }
                        else if (bobArray[i].Y + bobArray[i].Body.Height <= fieldArray[i].Y + 10 &&
                            bobArray[i].X + bobArray[i].Body.Width >= fieldArray[i].X + 10 &&
                            bobArray[i].X <= fieldArray[i].X + fieldArray[i].Box.Width - 10) // Over field 
                        {
                            bobArray[i].VertSpeed = -gravity;
                        }
                        else if (bobArray[i].X + bobArray[i].Body.Width >= fieldArray[i].X &&
                            bobArray[i].X + bobArray[i].Body.Width <= fieldArray[i].X + 5 &&
                            bobArray[i].Y <= fieldArray[i].Y + fieldArray[i].Box.Height &&
                            bobArray[i].Y + bobArray[i].Body.Height >= fieldArray[i].Y) //venstre side
                        {
                            bobArray[i].MoveLeft();
                        }
                        else if (bobArray[i].X <= fieldArray[i].X + fieldArray[i].Box.Width &&
                            bobArray[i].X >= fieldArray[i].X + fieldArray[i].Box.Width - 5 &&
                            bobArray[i].Y <= fieldArray[i].Y + fieldArray[i].Box.Height &&
                            bobArray[i].Y + jumpingJona.Body.Height >= fieldArray[i].Y) //Højre side
                        {
                            bobArray[i].MoveRight();
                        }
                        else if (bobArray[i].Y > fieldArray[i].Y + fieldArray[i].Box.Height &&
                          bobArray[i].X > fieldArray[i].X + fieldArray[i].Box.Width) //SØ hjørne
                        {
                            bobArray[i].MoveRight();
                            bobArray[i].Y += 2;
                            bobArray[i].VertSpeed = -bobArray[i].VertSpeed;
                        }
                        else if (bobArray[i].Y > fieldArray[i].Y + fieldArray[i].Box.Height &&
                          bobArray[i].X + bobArray[i].Body.Width < fieldArray[i].X) // SV Hjørne 
                        {
                            bobArray[i].MoveLeft();
                            bobArray[i].Y += 2;
                        }
                        else if (bobArray[i].Y + bobArray[i].Body.Height < fieldArray[i].Y &&
                          bobArray[i].X + bobArray[i].Body.Width < fieldArray[i].X) // NV hjørne 
                        {
                            bobArray[i].MoveLeft();
                        }
                        else if (bobArray[i].Y + bobArray[i].Body.Height < fieldArray[i].Y &&
                              bobArray[i].X > fieldArray[i].X + fieldArray[i].Box.Width)
                        {
                            bobArray[i].MoveRight();
                        }

                    }
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
        public void testFun()
        {
            //NetComm.Host server = new NetComm.Host(2020);
            //server.StartConnection();
        }


        


    }
}
