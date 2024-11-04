using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.PedIntelligence;

public static class ServerBuilderExtensions
{
    public static void AddPedIntelligenceResource(this ServerBuilder builder, PedIntelligenceOptions options)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new PedIntelligenceResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddPedIntelligenceServices(options);
        });

        builder.AddLogic<PedIntelligenceLogic>();
    }

    public static IServiceCollection AddPedIntelligenceServices(this IServiceCollection services, PedIntelligenceOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddSingleton<PedIntelligenceService>();
        return services;
    }
}
