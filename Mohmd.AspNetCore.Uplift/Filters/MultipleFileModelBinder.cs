using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mohmd.AspNetCore.Uplift.Helpers;
using Mohmd.AspNetCore.Uplift.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Uplift.Filters
{
    public class MultipleFileModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var logger = bindingContext.HttpContext.RequestServices.GetService<ILogger<MultipleFileModelBinder>>();
            UpliftOptions upliftOptions = bindingContext.HttpContext.RequestServices.GetService<IOptions<UpliftOptions>>().Value;

            IFormFileCollection files = bindingContext.HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                logger.LogInformation($"{files.Count} files uploaded.");
                foreach (var item in files)
                {
                    logger.LogInformation($"Uploaded file `{item.Name} = {item.FileName}`");
                }

                var modelFiles = files.Where(x => Regex.IsMatch(x.Name, $@"^({bindingContext.ModelName})(\[\d+\])?$")).ToList();
                List<FormFile> result = new List<FormFile>();

                foreach (var file in modelFiles)
                {
                    result.Add(new FormFile
                    {
                        Length = file.Length,
                        ContentDisposition = file.ContentDisposition,
                        ContentType = file.ContentType,
                        FileName = file.FileName,
                        FilePath = await FileHelper.SaveTemporaryFile(file, upliftOptions),
                        Headers = file.Headers,
                        Name = file.Name,
                    });
                }

                bindingContext.Result = ModelBindingResult.Success(result);
            }
        }
    }
}
