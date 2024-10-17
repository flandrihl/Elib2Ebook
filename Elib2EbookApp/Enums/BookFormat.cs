namespace Elib2EbookApp.Enums
{
    [Flags]
    public enum BookFormat
    {
        epub = 1,
        fb2 = 2,
        cbz = 4,
        json = 8,
        txt = 16
    }

    public class BookFormatWrapper
    {
        public BookFormatWrapper(BookFormat format, bool isSelected = false)
        {
            Format = format;
            IsSelected = isSelected;
        }

        public bool IsSelected { get; set; }
        public BookFormat Format { get; set; }
    }
}