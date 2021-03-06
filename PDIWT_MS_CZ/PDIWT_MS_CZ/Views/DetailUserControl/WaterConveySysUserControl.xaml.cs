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
using PDIWT_MS_CZ.Views.DetailUserControl;

namespace PDIWT_MS_CZ.Views.DetailUserControl
{
    /// <summary>
    /// WaterConveySysUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class WaterConveySysUserControl : UserControl
    {
        public WaterConveySysUserControl()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _combobox = (ComboBox)sender;
            switch (_combobox.SelectedIndex)
            {
                case 0:
                    CulvertContent.Content = new ShortCulvertUserControl();
                    break;
                case 1:
                    CulvertContent.Content = new LocalConcertationCulvertUserControl();
                    break;
                default:
                    break;
            }
        }
    }
}
