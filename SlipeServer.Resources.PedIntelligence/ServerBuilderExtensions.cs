using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.PedIntelligence;

public static class ServerBuilderExtensions
{
    public static void AddPedIntelligenceResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new PedIntelligenceResource(server);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<PedIntelligenceService>();
        });

        builder.AddLogic<PedIntelligenceLogic>();
    }
}
