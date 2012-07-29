using System;
using System.IO;
using PiratePlayer.Extensions;

namespace PiratePlayer.ViewModel
{
	public class Episode
	{
		public Episode(FileInfo fileInfo)
		{
			File = fileInfo;
		}

		public FileInfo File { get; private set; }

		public bool IsPlaying { get; set; }

		public string PrettyAge
		{
			get { return (DateTime.Now - File.LastWriteTime).PrettyFormat(); }
		}

		public TimeSpan Age
		{
			get { return (DateTime.Now - File.LastWriteTime); }
		}
	}
}
