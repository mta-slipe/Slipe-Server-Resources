using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Watermark;

public static class ServerBuilderExtensions
{
    public static void AddWatermarkResource(this ServerBuilder builder, WatermarkOptions options)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new WatermarkResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddWatermarkServices(options);
        });

        builder.AddLogic<WatermarkLogic>();
    }

    public static IServiceCollection AddWatermarkServices(this IServiceCollection services, WatermarkOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddSingleton<WatermarkService>();
        return services;
    }
}
