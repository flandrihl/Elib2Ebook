using CommunityToolkit.Mvvm.ComponentModel;
using Elib2EbookApp.Enums;

namespace Elib2EbookApp
{
    public class MainModel : ObservableObject
    {
        private string _url;
        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }
    }

    public class Settings : ObservableObject
    {
        private string _login;
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _dontLoadImages;
        public bool DontLoadImages
        {
            get => _dontLoadImages;
            set => SetProperty(ref _dontLoadImages, value);
        }

        private bool _saveCover;
        public bool SaveCover
        {
            get => _saveCover;
            set => SetProperty(ref _saveCover, value);
        }

        private string _savePath;
        public string SavePath
        {
            get => _savePath ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            set => SetProperty(ref _savePath, value);
        }

        private string _tempPath;
        public string TempPath
        {
            get => _tempPath ?? Environment.GetFolderPath(Environment.SpecialFolder.CommonTemplates);
            set => SetProperty(ref _tempPath, value);
        }

        private bool _dontClearTemp;
        public bool DontClearTemp
        {
            get => _dontClearTemp;
            set => SetProperty(ref _dontClearTemp, value);
        }

        private bool _saveTempFiles;
        public bool SaveTempFiles
        {
            get => _saveTempFiles;
            set => SetProperty(ref _saveTempFiles, value);
        }

        private BookFormat _bookFormat = BookFormat.fb2;
        public BookFormat BookFormat
        {
            get => _bookFormat;
            set => SetProperty(ref _bookFormat, value);
        }

        private int _delayInterval = 0;
        public int DelayInterval
        {
            get => _delayInterval;
            set => SetProperty(ref _delayInterval, value);
        }

        private int _timeout = 120;
        public int Timeout
        {
            get => _timeout;
            set => SetProperty(ref _timeout, value);
        }

        private bool _useProxy;
        public bool UseProxy
        {
            get => _useProxy;
            set => SetProperty(ref _useProxy, value);
        }

        private ProxyProtocol _protocol;
        public ProxyProtocol Protocol
        {
            get => _protocol;
            set => SetProperty(ref _protocol, value);
        }

        private string _proxyHost = string.Empty;
        public string ProxyHost
        {
            get => _proxyHost;
            set => SetProperty(ref _proxyHost, value);
        }

        private ushort _proxyPort = 0;
        public ushort ProxyPort
        {
            get => _proxyPort;
            set => SetProperty(ref _proxyPort, value);
        }

        private AddFileType _fileTypes;
        public AddFileType FileTypes
        {
            get => _fileTypes;
            set => SetProperty(ref _fileTypes, value);
        }

        private bool _loadChapters;
        public bool LoadChapters
        {
            get  => _loadChapters;
            set => SetProperty(ref _loadChapters, value);
        }

        public ushort _startChapters;
        public ushort StartChapters
        {
            get => _startChapters;
            set => SetProperty(ref _startChapters, value);
        }

        public ushort _endChapters;
        public ushort EndChapters
        {
            get => _endChapters;
            set => SetProperty(ref _endChapters, value);
        }

        private string _firstChapters;
        public string FirstChapters
        {
            get => _firstChapters;
            set => SetProperty(ref _firstChapters, value);
        }

        private string _lastChapters;
        public string LastChapters
        {
            get => _lastChapters;
            set => SetProperty(ref _lastChapters, value);
        }

        public Settings Clone() => new()
        {
            BookFormat = BookFormat,
            DelayInterval = DelayInterval,
            DontClearTemp = DontClearTemp,
            DontLoadImages = DontLoadImages,
            EndChapters = EndChapters,
            FirstChapters = FirstChapters,
            LastChapters = LastChapters,
            FileTypes = FileTypes,
            LoadChapters = LoadChapters,
            Login = Login,
            Password = Password,
            Protocol = Protocol,
            ProxyHost = ProxyHost,
            ProxyPort = ProxyPort,
            SaveCover = SaveCover,
            SavePath = SavePath,
            SaveTempFiles = SaveTempFiles,
            StartChapters = StartChapters,
            TempPath = TempPath,
            Timeout = Timeout,
            UseProxy = UseProxy
        };
    }
}