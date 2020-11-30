using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;

namespace FileWatcherService
{
    class JsonParser<T> : IConfigParser<T> where T : class
    {
        private readonly string jsonPath;

        public JsonParser(string jsonPath)
        {
            this.jsonPath = jsonPath;
        }

        public T Parse()
        {
            using (FileStream fileStream = new FileStream(jsonPath, FileMode.OpenOrCreate))
            {
                using (JsonDocument document = JsonDocument.Parse(fileStream))
                {
                    JsonElement element = document.RootElement;
                    if (typeof(T).GetProperties().First().Name != element.EnumerateObject().First().Name)
                    {
                        element = element.GetProperty(typeof(T).Name);
                    }
                    try
                    {
                        return JsonSerializer.Deserialize<T>(element.GetRawText());
                    }
                    catch (Exception ex)
                    {
                        using (var streamWriter = new StreamWriter(@"C:\Users\polin\source\repos\3_sem\lr2\FileWatcherService\error.txt", true, Encoding.Default))
                        {
                            streamWriter.WriteLine("Error of json file deserialization: " + ex.Message);
                        }
                        return null;
                    }
                }
            }
        }
    }
}
