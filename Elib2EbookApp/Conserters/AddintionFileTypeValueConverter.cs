using Elib2EbookApp.Enums;
using System.Globalization;
using System.Windows.Data;

namespace Elib2EbookApp.Conserters
{
    public class AddintionFileTypeValueConverter : IValueConverter
    {
        public static readonly AddintionFileTypeValueConverter Instance = new();
        private AddFileType _fileType;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mask = (AddFileType)parameter;
            _fileType = (AddFileType)value;
            return (mask & _fileType) != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _fileType ^= (AddFileType)parameter;
            return _fileType;
        }
    }
}
