using AngleSharp.Dom;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Elib2EbookApp.Helpers;
using Microsoft.Win32;
using Newtonsoft.Json;
using NLog;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace Elib2EbookApp
{
    public class MainViewModel : ObservableObject
    {
        private List<string> _args = [];

        private readonly ILogger _logger;
        private readonly EventLogTarget _logTarget;
        public ObservableCollection<string> LogEntries { get; } = [];
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MainViewModel(ILogger logger)
        {
            _logger = logger;
            _logTarget = new EventLogTarget();
            _logTarget.EventReceived += LogTarget_EventReceived;
        }

        private void LogTarget_EventReceived(LogEventInfo obj)
        {
            App.Current.Dispatcher.BeginInvoke(() =>
            {
                var level = obj.Level;
                var message = string.Empty;//$"{obj.TimeStamp:HH:mm:ss} {obj.Level} : ";
                if (level >= LogLevel.Error && obj.Exception != null)
                    message += obj.Exception?.ToString();
                else
                    message += obj.Message;

                LogEntries.Insert(0, message);
            });
        }

        private Settings _settings;
        public Settings Settings
        {
            get => _settings ??= new();
            set
            {
                var prewValue = _settings;
                if (!SetProperty(ref _settings, value))
                    return;

                prewValue.PropertyChanged -= Settings_PropertyChanged;
                _settings.PropertyChanged += Settings_PropertyChanged;
            }
        }

        private static readonly string SerringsListFileName = $"{nameof(SettingsList)}.json";
        private Dictionary<string, Settings> _settingsList;
        public Dictionary<string, Settings> SettingsList
            => _settingsList ??= GetSettingsList();
        private Dictionary<string, Settings> GetSettingsList()
        {
            var result = new Dictionary<string, Settings>();
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, SerringsListFileName)))
            {
                var jsonContext = File.ReadAllText(SerringsListFileName);
                result = JsonConvert.DeserializeObject<Dictionary<string, Settings>>(jsonContext);
            }

            return result;
        }

        private MainModel _mainModel;
        public MainModel Model => _mainModel ??= GetMainModel();
        private MainModel GetMainModel()
        {
            var model = new MainModel();
            model.PropertyChanged += Model_PropertyChanged;

            return model;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(MainModel.Url) || string.IsNullOrWhiteSpace(Model.Url))
                return;

            try
            {
                var url = Url.Create(Model.Url);
                if (!SettingsList.TryGetValue(url.HostName, out var settings))
                {
                    settings = Settings.Clone();
                    SettingsList.TryAdd(url.HostName, settings);
                }

                Settings = settings;
            }
            catch(Exception ex)
            {
                _ = ex.Message;
            }
        }

        private IEnumerable<string> FillArgs()
        {
            var args = new List<string>{ $"-u {Model.Url}" };

            if (!string.IsNullOrEmpty(Settings.SavePath))
            {
                if (!Directory.Exists(Settings.SavePath))
                    Directory.CreateDirectory(Settings.SavePath);

                args.Add($"-s {Settings.SavePath}");
            }

            if (Settings.Timeout != 120)
                args.Add($"-t {Settings.Timeout}");

            if (Settings.DelayInterval > 0)
                args.Add($"-d {Settings.DelayInterval}");

            if (!string.IsNullOrWhiteSpace(Settings.Login))
                args.Add($"-l {Settings.Login}");
            if (!string.IsNullOrWhiteSpace(Settings.Password))
                args.Add($"-p {Settings.Password}");

            if (Settings.BookFormat.TraGetArgsFormat(out var bookFormats))
                args.Add(bookFormats);

            if (Settings.SaveTempFiles)
            {
                args.Add("--additional");
                if (Settings.FileTypes.TraGetArgsFormat(out var fileFormats))
                    args.Add(fileFormats);
                if (!string.IsNullOrWhiteSpace(Settings.TempPath))
                {
                    if (!Directory.Exists(Settings.TempPath))
                        Directory.CreateDirectory(Settings.TempPath);

                    args.Add("--temp");
                }

                if (Settings.DontClearTemp)
                    args.Add("--save-temp");
            }

            if (!Settings.LoadChapters)
                args.Add("--no-chapters");
            else
            {
                if (Settings.StartChapters > 0)
                    args.Add($"--start {Settings.StartChapters}");
                if (Settings.EndChapters > 0)
                    args.Add($"--end {Settings.EndChapters}");

                if (!string.IsNullOrEmpty(Settings.FirstChapters))
                    args.Add($"--start-name {Settings.FirstChapters}");
                if (!string.IsNullOrEmpty(Settings.LastChapters))
                    args.Add($"--end-name {Settings.LastChapters}");
            }

            if (Settings.DontLoadImages)
                args.Add("--no-image");

            if (Settings.SaveCover)
                args.Add("--cover");

            if (Settings.UseProxy)
                args.Add($"--proxy {Settings.Protocol}//{Settings.ProxyHost}:{Settings.ProxyPort}/");

            return args;
        }

        private void SaveSettings()
        {
            try
            {
                var jsonContext = JsonConvert.SerializeObject(SettingsList, Formatting.Indented);
                File.WriteAllText(SerringsListFileName, jsonContext);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            var url = Url.Create(Model.Url);
            if (!url.IsInvalid && (!SettingsList.TryGetValue(url.HostName, out var settings) || !settings.Equals(sender)))
                SettingsList.TryAdd(url.HostName, (Settings)sender);
        }

        private ICommand _selectFolderCommand;
        public ICommand SelectFolderCommand => _selectFolderCommand ??= new RelayCommand(() =>
        {
            OpenFolderDialog dlg = new()
            {
                Title = "Выберите путь для сохранения книг",
                Multiselect = false,
                ShowHiddenItems = true
            };
            if (dlg.ShowDialog() != true)
                return;

            Settings.SavePath = dlg.FolderName;
        });

        private ICommand _selectTempFolderCommand;
        public ICommand SelecteTempFolderCommand => _selectTempFolderCommand ??= new RelayCommand(() =>
        {
            OpenFolderDialog dlg = new()
            {
                Title = "Выберите путь для сохранения доп. файлов",
                Multiselect = false,
                ShowHiddenItems = true
            };
            if (dlg.ShowDialog() != true)
                return;

            Settings.TempPath = dlg.FolderName;
        });

        private void RaiseDownloadCommand()
        {
            if (DownloadCommand is RelayCommand command)
                command.NotifyCanExecuteChanged();
        }

        private bool _isDownloading;

        private ICommand _downloadCommand;
        public ICommand DownloadCommand => _downloadCommand ??= new RelayCommand(() => RunDownloading(FillArgs()), () => !_isDownloading);

        private void RunDownloading(IEnumerable<string> args)
        {
            SaveSettings();
            RaiseDownloadCommand();
            var downloaderProcces = new Process();
            downloaderProcces.StartInfo.FileName = "Elib2EbookCli.exe";
            downloaderProcces.StartInfo.Arguments = string.Join(' ', args);
            downloaderProcces.StartInfo.UseShellExecute = false;
            downloaderProcces.StartInfo.RedirectStandardOutput = true;
            downloaderProcces.StartInfo.RedirectStandardError = true;
            downloaderProcces.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            downloaderProcces.StartInfo.CreateNoWindow = true;
            downloaderProcces.OutputDataReceived += DownloaderProcces_OutputDataReceived;
            downloaderProcces.ErrorDataReceived += DownloaderProcces_ErrorDataReceived;
            downloaderProcces.Disposed += DownloaderProcces_Disposed;
            downloaderProcces.Exited += DownloaderProcces_Exited;
            downloaderProcces.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            downloaderProcces.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            _isDownloading = true;
            RaiseDownloadCommand();
            downloaderProcces.Start();
            downloaderProcces.BeginOutputReadLine();
            downloaderProcces.BeginErrorReadLine();
        }

        private void DownloaderProcces_Disposed(object sender, EventArgs e)
        {
            _isDownloading = false;
            App.Current.Dispatcher.Invoke(RaiseDownloadCommand);
        }

        private void DownloaderProcces_Exited(object sender, EventArgs e)
        {
            _isDownloading = false;
            App.Current.Dispatcher.Invoke(RaiseDownloadCommand);
        }

        private void DownloaderProcces_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            var errorMessage = e.Data?.Trim('\n')?.Trim('\r');
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                errorMessage = $"{DateTime.Now:HH:mm:ss.fff}|Info| Done.";
            }
            _isDownloading = false;
            App.Current.Dispatcher.Invoke(RaiseDownloadCommand);
            App.Logger.Error(errorMessage);
        }

        private void DownloaderProcces_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var outputMessage = e.Data?.Trim('\n')?.Trim('\r');
            if (string.IsNullOrWhiteSpace(outputMessage))
                return;

            App.Logger.Info(outputMessage);
        }
    }
}