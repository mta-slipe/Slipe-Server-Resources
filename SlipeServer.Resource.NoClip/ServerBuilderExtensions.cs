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

        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<NoClipService>();
        });

        builder.AddLogic<NoClipLogic>();
    }
}
