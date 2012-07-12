using System;
using System.IO;
using PiratePlayer.Extensions;

namespace PiratePlayer.Model
{
    public class Episode
    {
        public FileInfo File { get; private set; }
		public string Age {
			get
			{
				return (DateTime.Now - File.LastWriteTime).PrettyFormat();
			}
		}

        public Episode(FileInfo file)
        {
            File = file;
        }
    }
}