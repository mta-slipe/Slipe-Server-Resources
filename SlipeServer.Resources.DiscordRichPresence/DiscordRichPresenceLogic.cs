using SlipeServer.Server.Elements;
using SlipeServer.Server;
using SlipeServer.Server.Services;
using SlipeServer.Server.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SlipeServer.Resources.DiscordRichPresence;

internal class DiscordRichPresenceLogic : ResourceLogicBase<DiscordRichPresenceResource, DiscordRichPresenceOptions>
{
    private readonly DiscordRichPresenceService discordRichPresenceService;
    private readonly LuaEventService luaEventService;
    private readonly ILuaEventHub<IDiscordRichPresenceEventHub> luaEventHub;

    public DiscordRichPresenceLogic(MtaServer server, ILogger<DiscordRichPresenceLogic> logger, IOptions<DiscordRichPresenceOptions> resourceOptions, IOptions<DefaultResourcesOptions> defaultResourcesOptions, DiscordRichPresenceService discordRichPresenceService, LuaEventService luaEventService, ILuaEventHub<IDiscordRichPresenceEventHub> luaEventHub, ResourceStartedManager? resourceStartedManager = null) : base(server, logger, resourceOptions, defaultResourcesOptions, resourceStartedManager)
    {
        this.discordRichPresenceService = discordRichPresenceService;
        this.luaEventService = luaEventService;
        this.luaEventHub = luaEventHub;
        luaEventService.AddEventHandler("discordSetApplicationIdResult", HandleSetApplicationIdResult);
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

    protected override void HandleResourceStarted(Player player)
    {
        var applicationId = this.resourceOptions.Value.ApplicationId.ToString();
        luaEventHub.Invoke(player, x => x.SetApplicationId(applicationId));
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
