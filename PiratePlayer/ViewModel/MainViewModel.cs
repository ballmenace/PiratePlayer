using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Codeplex.Reactive;
using PiratePlayer.Extensions;
using PiratePlayer.Model;

namespace PiratePlayer.ViewModel
{
	public class MainViewModel
	{
		private readonly DirectoryInfo _directoryToMonitor;
		private readonly string[] _fileFilters;

		public IEnumerable<Episode> AllShows { get; private set; }
		public ReactiveProperty<IEnumerable<Episode>> Shows { get; private set; }
		public ReactiveProperty<string> SearchFilter { get; private set; }

		public MainViewModel()
		{
			_directoryToMonitor = new DirectoryInfo(Properties.Settings.Default.DirectoryToObserve);
			_fileFilters = Properties.Settings.Default.FileFilters.Split(' ');

			LoadShows();

			Observable.Interval(TimeSpan.FromSeconds(5))
				.Subscribe(_ => LoadShows());

			SearchFilter = new ReactiveProperty<string>();
			Shows = SearchFilter
				.Select(Filter)
				.Throttle(TimeSpan.FromMilliseconds(100))
				.ToReactiveProperty();
		}

		private IEnumerable<Episode> Filter(string filter)
		{
			return from s in AllShows
			       where 
					string.IsNullOrWhiteSpace(filter) ||
					filter.Trim().Split(' ').All(x => s.File.Name.SearchFinds(x))
				   orderby s.File.LastWriteTime descending 
			       select s;
		}

		private void LoadShows()
		{
			AllShows = GetFiles(_directoryToMonitor.ToString(), _fileFilters, SearchOption.AllDirectories)
				.Select(x => new Episode(new FileInfo(x))).ToList();
		}

		public static IEnumerable<string> GetFiles(string path, string[] searchPatterns, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return searchPatterns.AsParallel().SelectMany(searchPattern => Directory.EnumerateFiles(path, searchPattern, searchOption));
		}

		public void ExecuteFile(Episode episode)
		{
			if (episode == null || episode.File == null || !episode.File.Exists)
				return;

			try
			{
				Process.Start(episode.File.ToString());
			}
			catch (Win32Exception e)
			{
				if (!e.IsCancelled())
					throw;
			}
		}
	}
}