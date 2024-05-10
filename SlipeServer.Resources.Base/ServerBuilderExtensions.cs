using SlipeServer.Server.Resources;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Base;

public static class ServerBuilderExtensions
{
    public static ServerBuilder AddLuaEventHub<THub, TResource>(this ServerBuilder builder) where TResource : Resource
    {
        builder.ConfigureServices(services =>
        {
            services.AddLuaEventHub<THub, TResource>();
        });
        return builder;
    }
}
