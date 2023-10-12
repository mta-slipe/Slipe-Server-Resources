using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.DiscordRichPresence;

public static class ServerBuilderExtensions
{
    public static void AddDiscordRichPresenceResource(this ServerBuilder builder, DiscordRichPresenceOptions options)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new DiscordRichPresenceResource(server, options);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<DiscordRichPresenceService>();
        });

        builder.AddLogic<DiscordRichPresenceLogic>();
    }
}