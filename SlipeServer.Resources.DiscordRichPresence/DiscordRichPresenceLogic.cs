using SlipeServer.Server.Elements;
using SlipeServer.Server;
using SlipeServer.Server.Services;
using SlipeServer.Server.Events;

namespace SlipeServer.Resources.DiscordRichPresence;

internal sealed class DiscordRichPresenceLogic : ResourceLogicBase<DiscordRichPresenceResource, DiscordRichPresenceOptions>
{
    private readonly DiscordRichPresenceService discordRichPresenceService;
    private readonly ILuaEventHub<IDiscordRichPresenceEventHub> luaEventHub;

    public DiscordRichPresenceLogic(MtaServer server, DiscordRichPresenceService discordRichPresenceService, LuaEventService luaEventService, ILuaEventHub<IDiscordRichPresenceEventHub> luaEventHub) : base(server)
    {
        this.discordRichPresenceService = discordRichPresenceService;
        this.luaEventHub = luaEventHub;

        luaEventService.AddEventHandler("discordSetApplicationIdResult", HandleSetApplicationIdResult);
    }

    private void HandleSetApplicationIdResult(LuaEvent luaEvent)
    {
        if (IsStarted(luaEvent.Player))
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
