using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CWPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private Ellipse jumpingJona;
        public GameWindow()
        {
            InitializeComponent();
            IniJumpingJona();
            //Thread thread = new Thread(new ThreadStart(MoveJumpingJona));
            //thread.Start();
        }


        private void IniJumpingJona()
        {
            jumpingJona = new Ellipse();
            jumpingJona.Name = "jumpingJona";
            jumpingJona.Height = 50;
            jumpingJona.Width = 50;
            jumpingJona.StrokeThickness = 2;
            jumpingJona.Fill = new SolidColorBrush(Colors.Blue);
            jumpingJona.Stroke = new SolidColorBrush(Colors.Black);
            jumpingJona.Margin = new Thickness(0, 0, 0, 0);
            jonaGrid.Children.Add(jumpingJona);
        }

        private void MoveJumpingJona()
        {
            while (true)
            {
                if (Keyboard.IsKeyDown(Key.Right))
                {
                    Thickness currPos = jumpingJona.Margin;
                    Thickness newPos = currPos;
                    newPos.Right = newPos.Right + 100;
                    jumpingJona.Margin = newPos;
                }
            }
        }
    }
}
