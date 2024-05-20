using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.NoClip;

public static class ServerBuilderExtensions
{
    public static void AddNoClipResource(this ServerBuilder builder, NoClipOptions? options = null)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new NoClipResource(server, options ?? NoClipOptions.Default);
            var additionalFiles = resource.GetAndAddLuaFiles(httpClient: server.GetRequiredService<HttpClient>());
            server.AddAdditionalResource(resource, additionalFiles);
            resource.AddLuaEventHub<INoClipEventHub>();
        });

        builder.ConfigureServices(services =>
        {
            services.AddNoClipServices();
        });

        builder.AddLogic<NoClipLogic>();
    }

    public static IServiceCollection AddNoClipServices(this IServiceCollection services)
    {
        services.AddLuaEventHub<INoClipEventHub, NoClipResource>();
        services.AddSingleton<NoClipService>();
        return services;
    }
}
