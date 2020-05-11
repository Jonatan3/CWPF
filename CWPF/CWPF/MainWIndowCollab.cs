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
        private void IniCollab()
        {
            // Textblock
            TextBlock collaboratorsText = new TextBlock();
            collaboratorsText.Text = "Collaborators for this project are:\n" +
                "Josephine Zaina Brandstrup Weirsøe\n" +
                "Max Peter Gammelgaard\n" +
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
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            buttonGrid.Children.Clear();
            IniMainMenu();
        }
    }
}
