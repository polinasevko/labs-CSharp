using System;
using System.ServiceProcess;
using System.IO;
using System.Threading;

namespace FileWatcherService
{
    public partial class Service1 : ServiceBase
    {
        Logger logger;
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            logger = new Logger();
            Thread loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(1000);
        }
    }

    class Logger
    {
        private readonly Options configOptions;
        bool enabled = true;
        object obj = new object();
        FileSystemWatcher watcher;

        public Logger()
        {
            ConfigManager optionsManager = new ConfigManager(@"C:\Users\polin\source\repos\3_sem\lr2\FileWatcherService");
            configOptions = optionsManager.GetOptions<Options>();
            CreateDir(configOptions.StorageOptions.SourceDir);
            CreateDir(configOptions.StorageOptions.TargetDir);
            watcher = new FileSystemWatcher(configOptions.StorageOptions.SourceDir);
            watcher.Deleted += Watcher_Deleted;
            watcher.Created += Watcher_Created;
            watcher.Changed += Watcher_Changed;
            watcher.Renamed += Watcher_Renamed;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }
        // создание директорий 
        private void CreateDir(string dirName)
        {
            if (Directory.Exists(dirName))
            {
                return;
            }
            Directory.CreateDirectory(dirName);
        }
        // удаление файлов
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "удален";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }
        // переименование файлов
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "переименован в " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }
        // изменение файлов
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }
        // создание файлов
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            string fileName = e.Name;

            if (filePath.Contains(".gz") || filePath.Contains(".crypt"))
            {
                return;
            }

            RecordEntry(fileEvent, filePath);

            string compressedFile = Path.ChangeExtension(filePath, "gz");
            string newCompressedFile = configOptions.StorageOptions.TargetDir + "\\" + Path.ChangeExtension(fileName, "gz");
            string targetFile = Path.ChangeExtension(newCompressedFile, "crypt");

            string encryptFile = Actions.Encryption(filePath, true, configOptions.CryptingOptions.Key);

            Actions.Compress(encryptFile, compressedFile);
            File.Delete(filePath);

            File.Move(compressedFile, newCompressedFile);
            
            Actions.Decompress(newCompressedFile, targetFile);

            filePath = Actions.Encryption(targetFile, false, configOptions.CryptingOptions.Key);

            Actions.AddToArchive(filePath, configOptions.ArchiveOptions.ArchName);

            File.Delete(encryptFile);
            File.Delete(newCompressedFile);
            File.Delete(targetFile);
        }

        private void RecordEntry(string fileEvent, string filePath, string fileName = null)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter(@"C:\Users\polin\source\repos\3_sem\lr2\templog.TXT", true))
                {
                    writer.WriteLine(String.Format("{0} файл {1} был {2}",
                        DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                    writer.Flush();
                }
            }
        }
    }
}