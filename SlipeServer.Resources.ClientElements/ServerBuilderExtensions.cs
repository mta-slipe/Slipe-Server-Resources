using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.ClientElements;

public static class ServerBuilderExtensions
{
    public static void AddClientElementsResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ClientElementsResource(server);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<ClientElementsService>();
        });

        builder.AddLogic<ClientElementsLogic>();
    }
}
