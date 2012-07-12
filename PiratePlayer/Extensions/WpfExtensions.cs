using System.Windows;

namespace PiratePlayer.Extensions
{
    public static class WpfExtensions
    {
        public static T SourceDataContext<T>(this RoutedEventArgs e) where T : class
        {
            return ((FrameworkElement) e.OriginalSource).DataContext as T;
        }
    }
}
