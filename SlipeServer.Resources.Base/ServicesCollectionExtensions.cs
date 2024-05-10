using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Base;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddLuaEventHub<THub, TResource>(this IServiceCollection services) where TResource : Resource
    {
        services.AddSingleton<ILuaEventHub<THub>, LuaEventHub<THub, TResource>>();
        return services;
    }
}
