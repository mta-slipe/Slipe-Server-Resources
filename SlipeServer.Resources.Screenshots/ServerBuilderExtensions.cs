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
            var additionalFiles = resource.GetAndAddLuaFiles(httpClient: server.GetRequiredService<HttpClient>());
            server.AddAdditionalResource(resource, additionalFiles);

        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<ScreenshotsService>();
        });

        builder.AddLogic<ScreenshotsLogic>();
    }
}
