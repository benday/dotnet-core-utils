using System;
using Microsoft.Extensions.DependencyInjection;

namespace Benday.Common
{
    public class TypeRegistrationItem<TService, TImplementation> : ITypeRegistrationItem
        where TService : class
        where TImplementation : class, TService
    {
        public TypeRegistrationItem(ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            Lifetime = lifetime;
        }

        public ServiceLifetime Lifetime { get; set; }

        public Type ServiceType => typeof(TService);

        public Type ImplementationType => typeof(TImplementation);
    }
}
