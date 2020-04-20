﻿using System;
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

namespace SDEE.Launcher
{
    /// <summary>
    /// Interaction logic for MyStartMenu.xaml
    /// </summary>
    public partial class MyStartMenu : UserControl
    {
        public MyStartMenu()
        {
            InitializeComponent();
        }

        public void Toggle()
        {
            Visibility = Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
