using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.NoClip;

public static class ServerBuilderExtensions
{
    public static void AddNoClipResource(this ServerBuilder builder, NoClipOptions options)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new NoClipResource(server, options);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
            resource.AddLuaEventHub<INoClipEventHub>();
        });

        builder.ConfigureServices(services =>
        {
            services.AddNoClipServices(options);
        });

        builder.AddLogic<NoClipLogic>();
    }

    public static IServiceCollection AddNoClipServices(this IServiceCollection services, NoClipOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddLuaEventHub<INoClipEventHub, NoClipResource>();
        services.AddSingleton<NoClipService>();
        return services;
    }
}
