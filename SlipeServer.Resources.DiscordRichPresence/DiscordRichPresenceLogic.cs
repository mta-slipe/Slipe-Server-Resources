using SlipeServer.Server.Elements;
using SlipeServer.Server;
using Microsoft.Extensions.Options;
using System.Numerics;
using SlipeServer.Server.Services;
using SlipeServer.Server.Events;
using SlipeServer.Resources.Base;
using Microsoft.Extensions.Logging;
using SlipeServer.Server.Resources;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;
using System;

namespace SlipeServer.Resources.DiscordRichPresence;

internal class DiscordRichPresenceLogic
{
    private readonly MtaServer _server;
    private readonly DiscordRichPresenceService _discordRichPresenceService;
    private readonly ILuaEventHub<IDiscordRichPresenceEventHub> _luaEventHub;
    private readonly ILogger<DiscordRichPresenceLogic> _logger;
    private readonly DiscordRichPresenceResource _discordRichPresenceResource;

    public DiscordRichPresenceLogic(MtaServer server, DiscordRichPresenceService discordRichPresenceService, LuaEventService luaEventService, ILuaEventHub<IDiscordRichPresenceEventHub> luaEventHub, ILogger<DiscordRichPresenceLogic> logger)
    {
        _server = server;
        _discordRichPresenceService = discordRichPresenceService;
        _luaEventHub = luaEventHub;
        _logger = logger;
        server.PlayerJoined += HandlePlayerJoin;

        _discordRichPresenceResource = _server.GetAdditionalResource<DiscordRichPresenceResource>();
        luaEventService.AddEventHandler("discordSetApplicationIdResult", HandleSetApplicationIdResult);
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            await _discordRichPresenceResource.StartForAsync(player);
            var applicationId = _discordRichPresenceResource.Options.ApplicationId.ToString();
            _luaEventHub.Invoke(player, x => x.SetApplicationId(applicationId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start discordRichPresence resource for player {playerName}", player.Name);
        }
    }

    private void HandleResourceStarted(Player player, Server.Elements.Events.PlayerResourceStartedEventArgs e)
    {
        var applicationId = _discordRichPresenceResource.Options.ApplicationId.ToString();
        player.TriggerLuaEvent("discordSetApplicationId", player, applicationId.ToString());
        player.ResourceStarted -= HandleResourceStarted;
    }

    private void HandleSetApplicationIdResult(LuaEvent luaEvent)
    {
        var success = luaEvent.Parameters[0].BoolValue;
        var userId = luaEvent.Parameters[1].StringValue;
        _discordRichPresenceService.AddPlayer(luaEvent.Player, success ?? false, userId);
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
