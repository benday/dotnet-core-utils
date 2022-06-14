using System;
using Microsoft.Extensions.DependencyInjection;

namespace Benday.Common
{
    public interface ITypeRegistrationItem
    {
        Type ImplementationType { get; }
        ServiceLifetime Lifetime { get; set; }
        Type ServiceType { get; }
    }
}
