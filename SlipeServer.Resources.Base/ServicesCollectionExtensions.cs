using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Base;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddResources(this IServiceCollection services, DefaultResourcesOptions options)
    {
        services.AddSingleton<ResourceStartedManager>();
        services.AddSingleton(Options.Create(options));
        return services;
    }

    public static IServiceCollection AddLuaEventHub<THub, TResource>(this IServiceCollection services) where TResource : Resource
    {
        services.AddSingleton<ILuaEventHub<THub>, LuaEventHub<THub, TResource>>();
        return services;
    }
}
