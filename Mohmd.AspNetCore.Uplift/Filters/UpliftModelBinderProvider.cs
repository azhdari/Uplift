using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Mohmd.AspNetCore.Uplift.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohmd.AspNetCore.Uplift.Filters
{
    public class UpliftModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (!context.Metadata.IsComplexType)
            {
                return null;
            }

            var propertyName = context.Metadata.PropertyName;
            if (propertyName == null)
            {
                return null;
            }

            var propertyInfo = context.Metadata.ContainerType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                return null;
            }

            if (propertyInfo.PropertyType == typeof(FormFile))
            {
                return new SingleFileModelBinder();
            }
            else if (propertyInfo.PropertyType == typeof(List<FormFile>))
            {
                return new MultipleFileModelBinder();
            }

            return null;
        }
    }
}
