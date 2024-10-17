namespace Elib2EbookApp.Enums
{
    [Flags]
    public enum AddFileType
    {
        books = 1,
        audio = 2,
        images = 4
    }

    public class AddFileTypeWrapper
    {
        public AddFileTypeWrapper(AddFileType addType, bool isSelected = false)
        {
            AddType = addType;
            IsSelected = isSelected;
        }

        public bool IsSelected { get; set; }
        public AddFileType AddType { get; set; }
    }
}