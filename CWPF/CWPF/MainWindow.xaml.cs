using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            IniButtons();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow();
            gameWindow.Show();
        }

        private void IniButtons()
        {
            Button startButton = new Button();
            startButton.Height = 50;
            startButton.Width = 80;
            startButton.Content = "Start Game";
            startButton.Background = new SolidColorBrush(Colors.Black);
            startButton.Foreground = new SolidColorBrush(Colors.White);
            startButton.Click += new RoutedEventHandler(startButton_Click);
            startButton.Margin = new Thickness(0, 0, 0, 0);
            buttonGrid.Children.Add(startButton);
        }
    }
}
