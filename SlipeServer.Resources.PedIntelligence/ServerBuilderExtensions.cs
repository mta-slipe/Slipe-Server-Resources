using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.PedIntelligance;

public static class ServerBuilderExtensions
{
    public static void AddPedIntelliganceResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new PedIntelliganceResource(server);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<PedIntelliganceService>();
        });

        builder.AddLogic<PedIntelliganceLogic>();
    }
}
