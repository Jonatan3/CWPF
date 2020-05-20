using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CWPF
{
    public partial class GameWindow 
    {
        private DispatcherTimer miliSecTimer = new DispatcherTimer();
        private DispatcherTimer tenSecTimer = new DispatcherTimer();
        private int time = 60 * 60;
        private TextBlock clockText;

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

            tenSecTimer = new DispatcherTimer();
            tenSecTimer.Interval = TimeSpan.FromSeconds(10);
            tenSecTimer.Tick += InputPowerUp;
            tenSecTimer.Start();
            clockText.Text = TimeSpan.FromSeconds(0).ToString();
            scoreText.Text = realScore.ToString();
        }
        private void StartClock(object sender, EventArgs e)
        {
            if (time != 0)
            {
                time = --time;
                clockText.Text = TimeSpan.FromSeconds(time).ToString();
            }
            else
            {
                EndGame();
                miliSecTimer.Stop();
                tenSecTimer.Stop();
            }
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
                        BottomText(powerText, jonaCanvas.ActualWidth / 2 -50, jonaCanvas.ActualHeight - 200, "Pres ENTER to use Double Jump");
                    }
                    else if (PU.Power == 2)
                    {
                        time += 5 * 60;
                        power1 = false;
                        power3 = false;
                        power4 = false;
                        BottomText(powerText, jonaCanvas.ActualWidth / 2 -40, jonaCanvas.ActualHeight - 200, "You just got +5 secs!");
                    }
                    else if (PU.Power == 3)
                    {
                        power1 = false;
                        power3 = true;
                        power4 = false;
                        BottomText(powerText, jonaCanvas.ActualWidth / 2 - 50 / 2, jonaCanvas.ActualHeight - 200, "Pres ENTER to use Size Down");
                    }
                    else if (PU.Power == 4)
                    {
                        power1 = false;
                        power3 = false;
                        power4 = true;
                        BottomText(powerText, jonaCanvas.ActualWidth / 2 -50, jonaCanvas.ActualHeight - 200, "Pres ENTER to use Size Up");
                    }
                    jonaCanvas.Children.Remove(PU.Body);
                    PowerExist = false;
                }
            }
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
        #endregion
    }
}
