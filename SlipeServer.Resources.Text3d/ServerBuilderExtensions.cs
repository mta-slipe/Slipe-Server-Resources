using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Text3d;

public static class ServerBuilderExtensions
{
    public static void AddText3dResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new Text3dResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddText3dServices();
        });

        builder.AddLogic<Text3dLogic>();
    }

    public static IServiceCollection AddText3dServices(this IServiceCollection services)
    {
        services.AddSingleton<Text3dService>();
        return services;
    }
}
