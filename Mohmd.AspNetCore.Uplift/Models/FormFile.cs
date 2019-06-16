using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.IO;

namespace Mohmd.AspNetCore.Uplift.Models
{
    [DebuggerDisplay("FileName={FileName}, FilePath={FilePath}")]
    public class FormFile
    {
        public long Length { get; internal set; }

        public string ContentType { get; internal set; }

        public string ContentDisposition { get; internal set; }

        public IHeaderDictionary Headers { get; internal set; }

        public string Name { get; internal set; }

        public string FileName { get; internal set; }

        public string FilePath { get; internal set; }

        public bool Deleted { get; private set; }

        public void Delete()
        {
            if (Deleted)
            {
                return;
            }

            if (string.IsNullOrEmpty(FilePath))
            {
                throw new Exception("FilePath property is empty.");
            }

            File.Delete(FilePath);
        }
    }
}
