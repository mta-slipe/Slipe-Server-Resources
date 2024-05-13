using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Scoreboard;

public static class ServerBuilderExtensions
{
    public static void AddScoreboard(this ServerBuilder builder, ScoreboardOptions? options = null)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ScoreboardResource(server, options ?? ScoreboardOptions.Default);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddScoreboardServices();
        });

        builder.AddLogic<ScoreboardLogic>();
    }

    public static IServiceCollection AddScoreboardServices(this IServiceCollection services)
    {
        services.AddSingleton<ScoreboardService>();
        return services;
    }
}
