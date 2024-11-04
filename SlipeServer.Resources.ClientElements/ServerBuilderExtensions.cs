using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.ClientElements;

public static class ServerBuilderExtensions
{
    public static void AddClientElementsResource(this ServerBuilder builder, ClientElementsOptions options)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ClientElementsResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddClientElementsServices(options);
        });

        builder.AddLogic<ClientElementsLogic>();
    }

    public static IServiceCollection AddClientElementsServices(this IServiceCollection services, ClientElementsOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddSingleton<ClientElementsService>();
        return services;
    }
}
