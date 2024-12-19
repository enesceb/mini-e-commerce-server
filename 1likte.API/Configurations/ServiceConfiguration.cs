using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1likte.Core.Services;

namespace _1likte.API.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection builder)
        {
            var type = typeof(IUserService);
            var types = type.Assembly.ExportedTypes.ToArray();
            var ns = type.Namespace!;

            var services = types.Where(x => x.Namespace == ns);

            foreach (var service in services)
            {
                var concrete = types.Where(x => x.IsAssignableTo(service) && x.IsClass).FirstOrDefault();

                if (concrete != null)
                    builder.AddScoped(service, concrete);
            }

            return builder;
        }

    }
}