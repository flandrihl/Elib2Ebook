using Elib2EbookApp.Enums;

namespace Elib2EbookApp.Helpers
{
    public static class FlagsEnumHelper
    {
        /// <summary>
        /// Tras the get arguments format.
        /// </summary>
        /// <param name="bookFormat">The book format.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static bool TraGetArgsFormat(this BookFormat bookFormat, out string format)
        {
            format = string.Empty;

            if (bookFormat.HasFlag(BookFormat.cbz))
                format += $"{BookFormat.cbz},";
            if (bookFormat.HasFlag(BookFormat.epub))
                format += $"{BookFormat.epub},";
            if (bookFormat.HasFlag(BookFormat.fb2))
                format += $"{BookFormat.fb2},";
            if (bookFormat.HasFlag(BookFormat.json))
                format += $"{BookFormat.json}_lite,";
            if (bookFormat.HasFlag(BookFormat.txt))
                format += $"{BookFormat.txt},";

            if (string.IsNullOrWhiteSpace(format))
                return false;

            format = "-f " + format.TrimEnd(',');
            return true;
        }

        /// <summary>
        /// Tras the get arguments format.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static bool TraGetArgsFormat(this AddFileType fileType, out string format)
        {
            format = string.Empty;

            if (fileType.HasFlag(AddFileType.audio))
                format += $"{AddFileType.audio},";
            if (fileType.HasFlag(AddFileType.books))
                format += $"{AddFileType.books},";
            if (fileType.HasFlag(AddFileType.images))
                format += $"{AddFileType.images},";

            if (string.IsNullOrWhiteSpace(format))
                return false;

            format = "--additional-types " + format.TrimEnd(',');
            return true;
        }
    }
}