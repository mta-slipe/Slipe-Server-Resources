using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Screenshots;

public static class ServerBuilderExtensions
{
    public static void AddScreenshotsResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ScreenshotsResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);

        });

        builder.ConfigureServices(services =>
        {
            services.AddScreenshotsServices();
        });

        builder.AddLogic<ScreenshotsLogic>();
    }

    public static IServiceCollection AddScreenshotsServices(this IServiceCollection services)
    {
        services.AddSingleton<ScreenshotsService>();
        return services;
    }
}
