using Mohmd.AspNetCore.Uplift.Filters;

namespace Microsoft.AspNetCore.Mvc
{
    public static class MvcOptionsExtensions
    {
        public static MvcOptions InsertUpliftModelBinderProviders(this MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new UpliftModelBinderProvider());
            return options;
        }
    }
}
