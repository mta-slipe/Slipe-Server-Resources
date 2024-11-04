using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Screenshots;

public static class ServerBuilderExtensions
{
    public static void AddScreenshotsResource(this ServerBuilder builder, ScreenshotsOptions options)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ScreenshotsResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);

        });

        builder.ConfigureServices(services =>
        {
            services.AddScreenshotsServices(options);
        });

        builder.AddLogic<ScreenshotsLogic>();
    }

    public static IServiceCollection AddScreenshotsServices(this IServiceCollection services, ScreenshotsOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddSingleton<ScreenshotsService>();
        return services;
    }
}
