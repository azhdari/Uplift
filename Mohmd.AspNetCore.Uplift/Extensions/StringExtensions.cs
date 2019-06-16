using System;
using System.IO;
using System.Linq;

namespace System
{
    internal static class StringExtensions
    {
        public static string GetFilenameSafe(this string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return filename;
            }

            char[] invalidChars = Path.GetInvalidFileNameChars();
            var validFilename = new string(filename.Where(ch => !invalidChars.Contains(ch)).ToArray());
            return validFilename;
        }
    }
}
