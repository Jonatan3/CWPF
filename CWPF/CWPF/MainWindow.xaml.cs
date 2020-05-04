﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IniMainMenu();
        }

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
            startButton.Margin = new Thickness(0, -60, 0, 0);
            buttonGrid.Children.Add(startButton);

            // Collaborators button
            Button collaboratorsButton = new Button();
            collaboratorsButton.Height = 50;
            collaboratorsButton.Width = 100;
            collaboratorsButton.Content = "Project\nCollaborators";
            collaboratorsButton.Background = new SolidColorBrush(Colors.Black);
            collaboratorsButton.Foreground = new SolidColorBrush(Colors.White);
            collaboratorsButton.Click += new RoutedEventHandler(CollaberatorsButton_Click);
            collaboratorsButton.Margin = new Thickness(0, 60, 0, 0);
            buttonGrid.Children.Add(collaboratorsButton);
        }
        
        private void IniCollab()
        {
            // Textblock
            TextBlock collaboratorsText = new TextBlock();
            collaboratorsText.Text = "Collaborators for this project are:\n" + 
                "Josephine Zaina Brandstrup Weirsøe\n" +
                "Max Peter Gammelgaard\n"+
                "Jonatan Amtoft Dahl";
            collaboratorsText.FontSize = 20;
            collaboratorsText.HorizontalAlignment = HorizontalAlignment.Center;
            collaboratorsText.VerticalAlignment = VerticalAlignment.Center;
            buttonGrid.Children.Add(collaboratorsText);

            // Back button
            Button backButton = new Button();
            backButton.Height = 50;
            backButton.Width = 100;
            backButton.Content = "Main Menu";
            backButton.Background = new SolidColorBrush(Colors.Black);
            backButton.Foreground = new SolidColorBrush(Colors.White);
            backButton.Margin = new Thickness(0, 180, 0, 0);
            backButton.Click += new RoutedEventHandler(BackButton_Click);
            buttonGrid.Children.Add(backButton);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow();
            gameWindow.WindowState = WindowState.Maximized;
            gameWindow.Show();
        }
        private void CollaberatorsButton_Click(object sender, RoutedEventArgs e)
        {
            buttonGrid.Children.Clear();
            IniCollab();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            buttonGrid.Children.Clear();
            IniMainMenu();
        }
    }
}
