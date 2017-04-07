﻿using System.Linq;
using DevExpress.Mvvm.UI;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace TestDev.Converter
{
    public class ListBoxEventArgsConverter : EventArgsConverterBase<MouseEventArgs>
    {
        protected override object Convert(object sender, MouseEventArgs args)
        {
            var element = LayoutTreeHelper.GetVisualParents((DependencyObject)args.OriginalSource, (DependencyObject)sender).OfType<ListBoxItem>().FirstOrDefault();
            return element != null ? element.DataContext : null;
        }

    }
}
