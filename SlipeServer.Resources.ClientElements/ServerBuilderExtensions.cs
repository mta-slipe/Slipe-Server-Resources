using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.ClientElements;

public static class ServerBuilderExtensions
{
    public static void AddClientElementsResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ClientElementsResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddClientElementsServices();
        });

        builder.AddLogic<ClientElementsLogic>();
    }

    public static IServiceCollection AddClientElementsServices(this IServiceCollection services)
    {
        services.AddSingleton<ClientElementsService>();
        return services;
    }
}
