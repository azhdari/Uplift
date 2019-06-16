using Mohmd.AspNetCore.Uplift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UpliftServiceCollectionExtensions
    {
        public static IServiceCollection AddUplift(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return AddUplift(services, null);
        }

        public static IServiceCollection AddUplift(this IServiceCollection services, Action<UpliftOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return services;
        }
    }
}
