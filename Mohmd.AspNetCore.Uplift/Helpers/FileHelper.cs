using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Uplift.Helpers
{
    public class FileHelper
    {
        public static async Task<string> SaveTemporaryFile(IFormFile file, UpliftOptions options)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (!file.FileName.Equals(file.FileName.GetFilenameSafe()))
            {
                throw new InvalidDataException($"`{file.FileName}` is invalid as a FileName.");
            }

            string tempPath = string.Empty;

            if (options.UseDefaultTempPath)
            {
                tempPath = Path.GetTempFileName();
            }
            else if (!string.IsNullOrEmpty(options.CustomTempPath))
            {
                string ext = Path.GetExtension(file.FileName);

                tempPath = Path.Combine(Environment.CurrentDirectory, options.CustomTempPath, Guid.NewGuid().ToString() + ext);
                var dir = Path.GetDirectoryName(tempPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }

            using (FileStream fs = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            return tempPath;
        }
    }
}
