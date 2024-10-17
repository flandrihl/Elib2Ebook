using NLog;
using NLog.Config;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;

namespace Elib2EbookApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The logger
        /// </summary>
        public static Logger Logger;

        public App()
        {
            InitializeLogger();
        }

        private void InitializeLogger()
        {
            // Step 1. Create configuration object
            var config = new LoggingConfiguration();
            // Step 2. Create targets and add them to the configuration 
            var fileTarget = new NLog.Targets.FileTarget();
            config.AddTarget("logFile", fileTarget);
            // Step 3. Set target properties 
            fileTarget.FileName = "Logs\\${cached:Events_${date:format=yyyy-MM-dd HH-mm-ss}}.log";
            fileTarget.Layout = "${longdate}|${level}|\t${message}";
            fileTarget.Encoding = Encoding.UTF8;
            // Step 4. Define rules
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, fileTarget));
            // Step 5. Activate the configuration
            LogManager.Configuration = config;
            // Step 6. Create logger
            Logger = LogManager.GetLogger(ResourceAssembly.GetName().Name + "_Logger");
            Logger.Debug($"Started Version {ResourceAssembly.FullName}");
            Logger.Info("Start Application");
        }
    }
}
