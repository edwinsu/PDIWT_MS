﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BMWPF = Bentley.MstnPlatformNET.WPF;

namespace PDIWT_MS_Tool.Views
{
    /// <summary>
    /// Interaction logic for FindnstancesView.xaml
    /// </summary>
    public partial class FindnstancesView : Window
    {
        private FindnstancesView()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.FindnstancesViewModel();
            m_wpfHelper = new BMWPF.WPFInteropHelper(this);
            m_wpfHelper.Attach(Program.Addin, true, "FindnstancesView");
        }

        static FindnstancesView m_windowhost;
        BMWPF.WPFInteropHelper m_wpfHelper;

        public static void ShowWindow()
        {
            if (m_windowhost != null)
            {
                m_windowhost.Focus();
                return;
            }
            m_windowhost = new FindnstancesView();
            m_windowhost.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            m_wpfHelper.Detach();
            m_wpfHelper.Dispose();
            m_windowhost = null;
        }

    }
}
