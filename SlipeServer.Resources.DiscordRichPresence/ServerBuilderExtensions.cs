using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.DiscordRichPresence;

public static class ServerBuilderExtensions
{
    public static void AddDiscordRichPresenceResource(this ServerBuilder builder, DiscordRichPresenceOptions options)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new DiscordRichPresenceResource(server, options);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
            resource.AddLuaEventHub<IDiscordRichPresenceEventHub>();
        });

        builder.ConfigureServices(services =>
        {
            services.AddDiscordRichPresenceServices();
        });

        builder.AddLogic<DiscordRichPresenceLogic>();
    }

    public static IServiceCollection AddDiscordRichPresenceServices(this IServiceCollection services)
    {
        services.AddSingleton<DiscordRichPresenceService>();
        services.AddLuaEventHub<IDiscordRichPresenceEventHub, DiscordRichPresenceResource>();
        return services;
    }
}