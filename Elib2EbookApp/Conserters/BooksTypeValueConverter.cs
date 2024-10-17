using Elib2EbookApp.Enums;
using System.Globalization;
using System.Windows.Data;

namespace Elib2EbookApp.Conserters
{
    public class BooksTypeValueConverter : IValueConverter
    {
        public static readonly BooksTypeValueConverter Instance = new();
        private BookFormat _bookFormat;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mask = (BookFormat)parameter;
            _bookFormat = (BookFormat)value;
            return (mask & _bookFormat) != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _bookFormat ^= (BookFormat)parameter;
            return _bookFormat;
        }
    }
}