using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Codeplex.Reactive;
using PiratePlayer.Utils;

namespace PiratePlayer
{
	public class MainViewModel
	{
		private readonly DirectoryInfo _directoryToMonitor;

		public IEnumerable<Episode> AllShows { get; private set; }
		public ReactiveProperty<IEnumerable<Episode>> Shows { get; private set; }
		public ReactiveProperty<string> SearchFilter { get; private set; }

		public MainViewModel()
		{
			_directoryToMonitor = new DirectoryInfo(Properties.Settings.Default.DirectoryToObserve);
			AllShows = LoadShows();

			Observable.Interval(TimeSpan.FromSeconds(5))
				.Subscribe(_ => AllShows = LoadShows());

			SearchFilter = new ReactiveProperty<string>();
			Shows = SearchFilter
				.Select(Filter)
				.Throttle(TimeSpan.FromMilliseconds(100))
				.ToReactiveProperty();
		}

		private IEnumerable<Episode> Filter(string filter)
		{
			if (string.IsNullOrWhiteSpace(filter))
				return AllShows;

			var filterParts = filter.Split(' ');

			return from s in AllShows
			       where filterParts.All(x => s.File.Name.SearchFinds(x))
			       select s;
		}

		private IEnumerable<Episode> LoadShows()
		{
			return GetFiles(_directoryToMonitor.ToString(), new[] {"*.avi", "*.mp4"}, SearchOption.AllDirectories)
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

			Process.Start(episode.File.ToString());
		}
	}
}