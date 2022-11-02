using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resource.NoClip;

public static class ServerBuilderExtensions
{
    public static void AddNoClipResource(this ServerBuilder builder, NoClipOptions? options = null)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new NoClipResource(server, options ?? NoClipOptions.Default);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<NoClipService>();
        });

        builder.AddLogic<NoClipLogic>();
    }
}
