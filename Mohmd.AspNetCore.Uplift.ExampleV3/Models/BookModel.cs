using Mohmd.AspNetCore.Uplift.Models;
using System.Collections.Generic;

namespace Mohmd.AspNetCore.Uplift.ExampleV3.Models
{
    public class BookModel
    {
        public string Name { get; set; }

        public FormFile FrontImage { get; set; }

        public List<FormFile> ContentImages { get; set; }
    }
}
