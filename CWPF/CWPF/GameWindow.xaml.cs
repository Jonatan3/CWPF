using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace CWPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private JumpingJona jumpingJona;
        private bool power1 = false, power3 = false, power4 = false, PowerExist = false;
        private int realScore = 0, numField = 15, numCoin = 25, numBob = 5, margins = 22;
        private TextBlock scoreText, powerText = new TextBlock();
        private double ranY, ranX, startY, out_, coinRadius = 12.5, grassTop, gravity = 0.1;
        private Coin[] coinArray;
        private Field[] fieldArray;
        private BouncingBob[] bobArray;
        private PowerUp PU;
        private Random rand = new Random();
        private int MaxHighscoreListEntryCount = 5;

        #region Constructures
        public GameWindow(bool? hardMode)
        {
            coinArray = new Coin[numCoin];
            fieldArray = new Field[numField];
            bobArray = new BouncingBob[numBob];
            InitializeComponent();
            LoadHighscoreList();
                       
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
            
            bdrHighscoreList.Visibility = Visibility.Collapsed;
            bdrEndOfGame.Visibility = Visibility.Collapsed;
        }
        #endregion
        #region Initiaters
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
                            j = i;
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
                                    j = i;
                                    k = numField;
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
        #endregion
        #region Private Methods
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

        public ObservableCollection<Highscore> HighscoreList
        {
            get;
            set;
        } = new ObservableCollection<Highscore>();
        #endregion
        #region Highscore
        private void ButtonAddHighscore_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = 0;
    
            if((this.HighscoreList.Count > 0) && (realScore < this.HighscoreList.Max(x => x.Score)))
            {
                Highscore justAbove = this.HighscoreList.OrderByDescending(x => x.Score).First(x => x.Score >= realScore);
                if(justAbove != null)
                    newIndex = this.HighscoreList.IndexOf(justAbove) + 1;
            }
            // Create and insert the neew highscore
            this.HighscoreList.Insert(newIndex, new Highscore()
            {
                PlayerName = txtPlayerName.Text,
                Score = realScore
            });
            // Make sure that the amount of higscores does not exceed the maximum (5)
            while(this.HighscoreList.Count > MaxHighscoreListEntryCount)
                this.HighscoreList.RemoveAt(MaxHighscoreListEntryCount);

            SaveHighscoreList();
    
            bdrNewHighscore.Visibility = Visibility.Collapsed;
            bdrHighscoreList.Visibility = Visibility.Visible;
        }

        private void LoadHighscoreList()
        {
            if(File.Exists("highscorelist.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
                using(Stream reader = new FileStream("highscorelist.xml", FileMode.Open))
                {            
                List<Highscore> tempList = (List<Highscore>)serializer.Deserialize(reader);
                this.HighscoreList.Clear();
                foreach(var item in tempList.OrderByDescending(x => x.Score))
                this.HighscoreList.Add(item);
                }
             }
        }

        private void SaveHighscoreList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Highscore>));
            using(Stream writer = new FileStream("highscorelist.xml", FileMode.Create))
            {
                serializer.Serialize(writer, this.HighscoreList);
            }
        }

        private void EndGame()
        {
            bool isNewHighscore = false;
            if(realScore > 0)
            {
                int lowestHighscore = (this.HighscoreList.Count > 0 ? this.HighscoreList.Min(x => x.Score) : 0);
                if((realScore > lowestHighscore) || (this.HighscoreList.Count < MaxHighscoreListEntryCount))
                {
                    bdrNewHighscore.Visibility = Visibility.Visible;
                    txtPlayerName.Focus();
                    isNewHighscore = true;
                }
            }
            if(!isNewHighscore)
            {
                tbFinalScore.Text = realScore.ToString();
                bdrEndOfGame.Visibility = Visibility.Visible;
            }
        }
        #endregion
    }
}
