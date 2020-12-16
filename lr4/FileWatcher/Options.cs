namespace FileWatcherService
{
    public class Options
    {
        public StorageOptions StorageOptions { get; set; }
        public ArchiveOptions ArchiveOptions { get; set; }
        public CryptingOptions CryptingOptions { get; set; }
    }

    public class StorageOptions
    {
        public string SourceDir{ get; set; }
        public string TargetDir { get; set; }
    }

    public class ArchiveOptions
    {
        public string ArchName { get; set; }
    }

    public class CryptingOptions
    {
        public string Key { get; set; }
    }
}
