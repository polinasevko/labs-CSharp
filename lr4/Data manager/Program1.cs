using System;
using System.IO;
using FileWatcherService;

namespace DataManager
{
    class Program1
    {
        static void Main(string[] args)
        {
            
            Data configOptions;
            ConfigManager optionsManager = new ConfigManager(AppDomain.CurrentDomain.BaseDirectory);
            configOptions = optionsManager.GetOptions<Data>();
            string loggerConnectionString = configOptions.LoggerConnectionString;
            DBApplicationInsights insights = new DBApplicationInsights(loggerConnectionString);
            try
            {
                insights.ClearAction();
                insights.AddAction("Clear ApplicationInsigths table", DateTime.Now);
                DataManager dataManager = new DataManager(insights, configOptions);
                dataManager.Transfer();
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errorFile.txt"), true))
                {
                    sw.WriteLine($"Ошибка в методе Main:{e.Message} \t {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                }
                insights.AddAction("Error: the ApplicationInsigths table is not cleared", DateTime.Now);
            }
        }
    }
}
