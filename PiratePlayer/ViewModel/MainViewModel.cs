using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Data;
using Codeplex.Reactive;
using PiratePlayer.Extensions;

namespace PiratePlayer.ViewModel
{
	public class MainViewModel
	{
		private readonly DirectoryInfo _directoryToMonitor;
		private readonly string[] _fileEndingFilters;

		private ICollection<Episode> _allEpisodes;
		private readonly ObservableCollection<Episode> _episodes;

		public ListCollectionView Episodes
		{
			get{ return new ListCollectionView(_episodes); }
		}

		public ReactiveProperty<string> SearchTerm { get; private set; }

		public MainViewModel()
		{
			_directoryToMonitor = new DirectoryInfo(Properties.Settings.Default.DirectoryToObserve);
			_fileEndingFilters = Properties.Settings.Default.FileFilters.Split(' ');
			SearchTerm = new ReactiveProperty<string>();
			_episodes = new ObservableCollection<Episode>();
			_allEpisodes = new List<Episode>();

			var loadedEpisodes = from _ in Observable.Interval(TimeSpan.FromSeconds(5)).Merge(Observable.Return(0L))
			                  let files = GetFiles(_directoryToMonitor.ToString(), _fileEndingFilters, SearchOption.AllDirectories)
							  select files.Select(file => new Episode(new FileInfo(file))).ToList();

			loadedEpisodes.Subscribe(files =>
				{
					_allEpisodes = files;
				});
			
			var searchClickEvents = SearchTerm.Throttle(TimeSpan.FromMilliseconds(100));

			searchClickEvents.ObserveOn(SynchronizationContext.Current).Subscribe(_ =>
				{
					var episodes = from episode in _allEpisodes
					               where IsContainedInFilter(episode.File.Name)
					               select episode;

					_episodes.Clear();
					foreach (var episode in episodes)
						_episodes.Add(episode);
				});
		}

		public static IEnumerable<string> GetFiles(string path, string[] searchPatterns, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return searchPatterns.AsParallel().SelectMany(searchPattern => Directory.EnumerateFiles(path, searchPattern, searchOption));
		}

		private bool IsContainedInFilter(string fileName)
		{
			var searchTerm = SearchTerm.Value;
			if (string.IsNullOrWhiteSpace(searchTerm))
				return true;

			var searchTermParts = searchTerm.Trim().Split(' ');
			return searchTermParts.All(fileName.ContainsIgnoreCase);
		}

		public void ExecuteFile(Episode episode)
		{
			if (episode == null || episode.File == null || !episode.File.Exists)
				return;

			episode.IsPlaying = true;

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

		public void ExecuteTopMostElement()
		{
			if(_episodes.Any())
				ExecuteFile(_episodes.First());
		}
	}
}