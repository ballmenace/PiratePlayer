using System.IO;

namespace PiratePlayer
{
    public class Episode
    {
        public FileInfo File { get; set; }

        public Episode(FileInfo file)
        {
            File = file;
        }
    }
}