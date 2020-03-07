using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mohmd.AspNetCore.Uplift.Helpers;
using Mohmd.AspNetCore.Uplift.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Uplift.Filters
{
    public class SingleFileModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            UpliftOptions upliftOptions = bindingContext.HttpContext.RequestServices.GetService<IOptions<UpliftOptions>>().Value;

            IFormFileCollection files = bindingContext.HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files.FirstOrDefault(x => x.Name == bindingContext.ModelName);
                if (file != null)
                {
                    var result = new FormFile
                    {
                        Length = file.Length,
                        ContentDisposition = file.ContentDisposition,
                        ContentType = file.ContentType,
                        FileName = file.FileName,
                        FilePath = await FileHelper.SaveTemporaryFile(file, upliftOptions),
                        Headers = file.Headers,
                        Name = file.Name,
                    };
                    bindingContext.Result = ModelBindingResult.Success(result);
                }
            }
        }
    }
}
