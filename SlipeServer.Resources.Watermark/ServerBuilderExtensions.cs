using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Watermark;

public static class ServerBuilderExtensions
{
    public static void AddWatermarkResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new WatermarkResource(server);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<WatermarkService>();
        });

        builder.AddLogic<WatermarkLogic>();
    }
}
