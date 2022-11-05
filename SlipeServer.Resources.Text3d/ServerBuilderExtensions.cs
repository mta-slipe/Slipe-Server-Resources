using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Text3d;

public static class ServerBuilderExtensions
{
    public static void AddText3dResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new Text3dResource(server);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<Text3dService>();
        });

        builder.AddLogic<Text3dLogic>();
    }
}
