using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PiratePlayer.Extensions;
using PiratePlayer.ViewModel;

namespace PiratePlayer.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private readonly MainViewModel _viewModel;

		public MainWindow()
		{
			InitializeComponent();
			_viewModel = new MainViewModel();
			DataContext = _viewModel;
		}

		private void WindowLoaded(object sender, RoutedEventArgs e)
		{
			FocusManager.SetFocusedElement(this, SearchTextBox);
		}

		private void ListViewDoubleClick(object sender, MouseButtonEventArgs e)
		{
			_viewModel.ExecuteFile(e.SourceDataContext<Episode>());
		}

		private void ListViewKeyup(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.Enter)
				return;

			_viewModel.ExecuteFile(e.SourceDataContext<Episode>());
		}

		private void SearchTextBoxKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.Enter)
				return;

			_viewModel.ExecuteTopMostElement();
		}

		private void CustomSortHandler(object sender, DataGridSortingEventArgs e)
		{
			throw new System.NotImplementedException();
		}
	}
}
