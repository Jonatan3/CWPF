using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CheckBox hardModeBox;
        #region Constructures
        public MainWindow()
        {
            InitializeComponent();
            IniMainMenu();
           
        }
        #endregion

        #region Initialiser
        private void IniMainMenu()
        {
            // Start game button
            Button startButton = new Button();
            startButton.Height = 50;
            startButton.Width = 100;
            startButton.Content = "Start Game";
            startButton.Background = new SolidColorBrush(Colors.Black);
            startButton.Foreground = new SolidColorBrush(Colors.White);
            startButton.Click += new RoutedEventHandler(StartButton_Click);
            startButton.Margin = new Thickness(0, -80, 0, 0);
            buttonGrid.Children.Add(startButton);

            // Collaborators button
            Button collaboratorsButton = new Button();
            collaboratorsButton.Height = 50;
            collaboratorsButton.Width = 100;
            collaboratorsButton.Content = "Project\nCollaborators";
            collaboratorsButton.Background = new SolidColorBrush(Colors.Black);
            collaboratorsButton.Foreground = new SolidColorBrush(Colors.White);
            collaboratorsButton.Click += new RoutedEventHandler(CollaberatorsButton_Click);
            collaboratorsButton.Margin = new Thickness(0, 80, 0, 0);
            buttonGrid.Children.Add(collaboratorsButton);

            // Highscore Button
            Button highscoreButton = new Button();
            highscoreButton.Height = 50;
            highscoreButton.Width = 100;
            highscoreButton.Content = "Highscore";
            highscoreButton.Background = new SolidColorBrush(Colors.Black);
            highscoreButton.Foreground = new SolidColorBrush(Colors.White);
            highscoreButton.Click += new RoutedEventHandler(HighscoreButton_Click);
            highscoreButton.Margin = new Thickness(0, 220, 0, 0);
            buttonGrid.Children.Add(highscoreButton);


            // Hardmode checkbox
            hardModeBox = new CheckBox();
            hardModeBox.IsChecked = false;
            hardModeBox.Content = "Enable hardmode";
            hardModeBox.Width = 130;
            hardModeBox.Height = 15;
            hardModeBox.Margin = new Thickness(30, 0, 0, 0);
            buttonGrid.Children.Add(hardModeBox);
        }

        private void HighscoreButton_Click(object sender, RoutedEventArgs e)
        {
            buttonGrid.Children.Clear();
            IniHigscore();

        }
        #endregion

        #region Clickers
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow(hardModeBox.IsChecked);
            gameWindow.Show();
        }
        private void CollaberatorsButton_Click(object sender, RoutedEventArgs e)
        {
            buttonGrid.Children.Clear();
            IniCollab();
        }
        #endregion
    }
}
