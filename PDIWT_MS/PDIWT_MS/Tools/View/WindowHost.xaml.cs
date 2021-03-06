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

using Bentley.MstnPlatformNET.WPF;
using Bentley.Windowing;

namespace PDIWT_MS.Tools.View
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class WindowHost : Window
    {
        private static WindowHost m_window1;
        WPFInteropHelper m_wndHelper;

        public WindowHost()
        {
            InitializeComponent();

            m_wndHelper = new WPFInteropHelper(this);
            m_wndHelper.Attach(Program.Addin, true, "WPFWindowTest");
        }

        public static void ShowWindow()
        {
            if (m_window1 != null)
            {
                m_window1.Focus();
                return;
            }
            m_window1 = new WindowHost();
            m_window1.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            m_wndHelper.Detach();
            m_wndHelper.Dispose();
            m_window1 = null;
        }
    }
}
