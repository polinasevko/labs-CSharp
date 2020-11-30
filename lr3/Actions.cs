using System.IO;
using System.IO.Compression;
using System.Text;

namespace FileWatcherService
{
    class Actions
    {
        public static string Encryption(string FilePath, bool mode, string key)
        {
            if (key == "12345678")
            {
                StringBuilder temp = new StringBuilder(FilePath);
                if (!mode)
                {
                    temp.Replace(".crypt", ".TXT");
                }
                byte[] curFile = File.ReadAllBytes(FilePath);
                byte[] newFile = Crypt(curFile);
                if (mode)
                {
                    temp.Replace(".TXT", ".crypt");
                }
                File.WriteAllBytes(temp.ToString(), newFile);
                return temp.ToString();
            }
            else
            {
                return FilePath;
            }

        }

        private static byte[] Crypt(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= 1;
            return bytes;
        }

        public static void Compress(string sourceFile, string compressedFile)
        {
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open))
            {
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        public static void Decompress(string compressedFile, string targetFile)
        {
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.Open))
            {
                using (FileStream targetStream = File.Create(targetFile))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                    }
                }
            }
        }

        public static void AddToArchive(string filePath, string myArchive)
        {
            using (ZipArchive archive = ZipFile.Open(myArchive, ZipArchiveMode.Update))
            {
                archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            }
        }
    }
}
