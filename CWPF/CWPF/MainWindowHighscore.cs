using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CWPF
{
   public partial class MainWindow : Window
    
    {
        private void IniHigscore()
        {
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
        
    }

    
}

