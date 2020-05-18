using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;


namespace CWPF
{
    public class Highscore
    {

        public string PlayerName
        {
            get;
            set;
        }

        public int Score
        {
            get;
            set;
        }


    }
}
