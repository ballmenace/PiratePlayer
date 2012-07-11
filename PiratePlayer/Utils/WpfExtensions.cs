using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PiratePlayer.Utils
{
    public static class WpfExtensions
    {
        public static T SourceDataContext<T>(this RoutedEventArgs e) where T : class
        {
            return ((FrameworkElement) e.OriginalSource).DataContext as T;
        }
    }
}
