﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

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
        // Initiates main menu
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
        #endregion

        #region Clickers
        // Hide button click
        private void ButtonHide_Click(object snder, RoutedEventArgs e) 
        {
            bdrHighscoreList.Visibility = Visibility.Collapsed;
        }
        // Start game button click
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow(hardModeBox.IsChecked);
            gameWindow.Show();
        }
        // Collaborators button click
        private void CollaberatorsButton_Click(object sender, RoutedEventArgs e)
        {
            buttonGrid.Children.Clear();
            IniCollab();
        }
        // Highscore button click
        private void HighscoreButton_Click(object sender, RoutedEventArgs e)
        {
            LoadHighscoreList();
            bdrHighscoreList.Visibility = Visibility.Visible;
        }
        #endregion

        #region Highscore
        // Get and set for highscorelist
        public ObservableCollection<Highscore> HighscoreList
        {
            get;
            set;
        } = new ObservableCollection<Highscore>();
        // Loads highscorelist from highscorelist.xml
        private void LoadHighscoreList()
        {
            if (File.Exists("highscorelist.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Highscore>));
                using (Stream reader = new FileStream("highscorelist.xml", FileMode.Open))
                {
                    List<Highscore> tempList = (List<Highscore>)serializer.Deserialize(reader);
                    this.HighscoreList.Clear();
                    foreach (var item in tempList.OrderByDescending(x => x.Score))
                        this.HighscoreList.Add(item);
                }
            }
        }
        #endregion
    }
}
