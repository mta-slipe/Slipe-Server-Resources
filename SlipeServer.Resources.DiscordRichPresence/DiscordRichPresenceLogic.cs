using SlipeServer.Server.Elements;
using SlipeServer.Server;
using SlipeServer.Server.Services;
using SlipeServer.Server.Events;
using SlipeServer.Resources.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Resources.Providers;
using System.Numerics;

namespace SlipeServer.Resources.DiscordRichPresence;

internal class DiscordRichPresenceLogic
{
    private readonly MtaServer server;
    private readonly DiscordRichPresenceService discordRichPresenceService;
    private readonly ILuaEventHub<IDiscordRichPresenceEventHub> luaEventHub;
    private readonly ILogger<DiscordRichPresenceLogic> logger;
    private readonly IOptions<DiscordRichPresenceOptions> discordRichPresenceOptions;
    private readonly IOptions<DefaultResourcesOptions> defaultResourcesOptions;
    private readonly DiscordRichPresenceResource discordRichPresenceResource;
    private readonly ResourceStartedManager resourceStartedManager;

    public DiscordRichPresenceLogic(MtaServer server, DiscordRichPresenceService discordRichPresenceService, LuaEventService luaEventService, ILuaEventHub<IDiscordRichPresenceEventHub> luaEventHub, ILogger<DiscordRichPresenceLogic> logger, IOptions<DiscordRichPresenceOptions> discordRichPresenceOptions, IOptions<DefaultResourcesOptions> defaultResourcesOptions, ResourceStartedManager? resourceStartedManager = null)
    {
        this.resourceStartedManager = resourceStartedManager ?? throw new InvalidOperationException("ResourceStartedManager is not initialized. Please ensure that you have called services.AddResources() in your service configuration.");
        this.server = server;
        this.discordRichPresenceService = discordRichPresenceService;
        this.luaEventHub = luaEventHub;
        this.logger = logger;
        this.discordRichPresenceOptions = discordRichPresenceOptions;
        this.defaultResourcesOptions = defaultResourcesOptions;
        this.discordRichPresenceResource = this.server.GetAdditionalResource<DiscordRichPresenceResource>();

        this.server.PlayerJoined += HandlePlayerJoin;
        luaEventService.AddEventHandler("discordSetApplicationIdResult", HandleSetApplicationIdResult);
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            if (!discordRichPresenceOptions.Value.Autostart ?? this.defaultResourcesOptions.Value.Autostart)
            {
                void handleResourceStarted(Player sender, PlayerResourceStartedEventArgs args)
                {
                    if (args.NetId != discordRichPresenceResource.NetId)
                        return;

                    try
                    {
                        HandleResourceStarted(sender);
                    }
                    catch(Exception ex)
                    {
                        this.logger.LogError(ex, "Resource started failed for player {playerName}", sender.Name);
                    }
                    finally
                    {
                        player.ResourceStarted -= handleResourceStarted;
                    }
                }
                player.ResourceStarted += handleResourceStarted;
            }
            else
            {
                await discordRichPresenceResource.StartForAsync(player);
                HandleResourceStarted(player);
            }
        }
        catch (Exception ex)
        {
            logger.ResourceFailedToStart<DiscordRichPresenceResource>(ex, player);
        }
    }

    private void HandleResourceStarted(Player player)
    {
        if (this.resourceStartedManager.Started<DiscordRichPresenceResource>(player))
        {
            var applicationId = discordRichPresenceOptions.Value.ApplicationId.ToString();
            luaEventHub.Invoke(player, x => x.SetApplicationId(applicationId));
        }
    }

    private void HandleSetApplicationIdResult(LuaEvent luaEvent)
    {
        if (this.resourceStartedManager.IsStarted<DiscordRichPresenceResource>(luaEvent.Player))
        {
            var success = luaEvent.Parameters[0].BoolValue;
            var userId = luaEvent.Parameters[1].StringValue;
            discordRichPresenceService.AddPlayer(luaEvent.Player, success ?? false, userId);
        }
    }
}

public interface IDiscordRichPresenceEventHub
{
    void SetApplicationId(string applicationId);
    void SetState(string state);
    void SetDetails(string details);
    void SetAsset(string asset, string assetName);
    void SetSmallAsset(string asset, string assetName);
    void SetButton(int index, string text, string url);
    void SetPartySize(int size, int max);
    void StartTime(int seconds);
}
