using SlipeServer.Server.Elements;
using SlipeServer.Server;
using Microsoft.Extensions.Options;
using System.Numerics;
using SlipeServer.Server.Services;
using SlipeServer.Server.Events;

namespace SlipeServer.Resources.DiscordRichPresence;

internal class DiscordRichPresenceLogic
{
    private readonly MtaServer _server;
    private readonly DiscordRichPresenceService _discordRichPresenceService;
    private readonly DiscordRichPresenceResource _discordRichPresenceResource;

    public DiscordRichPresenceLogic(MtaServer server, DiscordRichPresenceService discordRichPresenceService, LuaEventService luaEventService)
    {
        _server = server;
        _discordRichPresenceService = discordRichPresenceService;
        server.PlayerJoined += HandlePlayerJoin;

        _discordRichPresenceResource = _server.GetAdditionalResource<DiscordRichPresenceResource>();
        luaEventService.AddEventHandler("discordSetApplicationIdResult", HandleSetApplicationIdResult);
    }

    private void HandlePlayerJoin(Player player)
    {
        player.ResourceStarted += HandleResourceStarted;
        _discordRichPresenceResource.StartFor(player);
    }

    private void HandleResourceStarted(Player player, Server.Elements.Events.PlayerResourceStartedEventArgs e)
    {
        var applicationId = _discordRichPresenceResource.Options.ApplicationId;
        player.TriggerLuaEvent("discordSetApplicationId", player, applicationId.ToString());
        player.ResourceStarted -= HandleResourceStarted;
    }

    private void HandleSetApplicationIdResult(LuaEvent luaEvent)
    {
        var success = luaEvent.Parameters[0].BoolValue;
        _discordRichPresenceService.AddPlayer(luaEvent.Player, success ?? false);
    }
}
