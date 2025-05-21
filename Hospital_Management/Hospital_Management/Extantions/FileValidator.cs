namespace Hospital_Management.Extantions
{
    public static class FileValidator
    {
        public static bool ValidateTypeSize(this IFormFile file, int? maxMb = null, params string[] type)
        {
            bool isValidType = type.Length == 0 || type.Contains(file.ContentType);
            bool isValidSize = true;

            if (maxMb != null)
            {
                isValidSize = file.Length <= maxMb * 1024 * 1024;
            }

            return isValidType && isValidSize;
        }

        public static bool ValidateSize(this IFormFile file, int maxMb)
        {
            return file.Length <= maxMb * 1024 * 1024;
        }

        public static async Task<string> CreateFileAsync(this IFormFile file, string root, params string[] folder)
        {
            string originalFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            string finalFileName = _extractGuidFileName(originalFileName) + _getFileFormat(originalFileName);

            string path = root;
            for (int i = 0; i < folder.Length; i++)
            {
                path = Path.Combine(path, folder[i]);
            }
            path = Path.Combine(path, finalFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return finalFileName;

        }

        public static void DeleteFile(this string fileName, string root, params string[] folders)
        {
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, fileName);
            if (File.Exists(path)) File.Delete(path);
        }
        public static string Capitalize(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            return char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }
        private static string _extractGuidFileName(string fullFileName)
        {
            int underscoreIndex = fullFileName.IndexOf('_');

            if (underscoreIndex != -1) return fullFileName.Substring(0, underscoreIndex);

            return fullFileName;

        }
        private static string _getFileFormat(string fullFileName)
        {
            int lastDotIndex = fullFileName.LastIndexOf('.');

            if (lastDotIndex != -1) return fullFileName.Substring(lastDotIndex);
            return string.Empty;
        }
    }
}
