using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FileWatcherService
{
    public class ConfigManager
    {
        private readonly string path = null;

        public ConfigManager(string path)
        {
            if (File.Exists(path))
            {
                this.path = (Path.GetExtension(path) == ".xml"
                    || Path.GetExtension(path) == ".json") ? path : null;
            }
            else if (Directory.Exists(path))
            {
                var fileEntries = from file in Directory.GetFiles(path) where 
                                  Path.GetExtension(file) == ".xml" ||
                                  Path.GetExtension(file) == ".json"
                                  select file;

                this.path = fileEntries.Count() != 0 ? fileEntries.First() : null;
            }
        }

        public T GetOptions<T>() where T : class
        {
            if (path is null)
            {
                using (StreamWriter streamWriter = new StreamWriter(@"C:\Users\polin\source\repos\3_sem\lr2\FileWatcherService\error.txt", true, Encoding.Default))
                {
                    streamWriter.WriteLine("Configuration file not found.");
                }
                return null;
            }
            Provider<T> provider = new Provider<T>(path);
            return provider.configurationParser.Parse();
        }
    }
}
