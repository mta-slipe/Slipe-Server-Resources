﻿using Microsoft.Extensions.Logging;
using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Scoreboard;

internal class ScoreboardLogic
{
    private readonly MtaServer _server;
    private readonly ILogger<ScoreboardLogic> logger;
    private readonly ScoreboardResource resource;

    public ScoreboardLogic(MtaServer server, ScoreboardService scoreboardService, ILogger<ScoreboardLogic> logger)
    {
        _server = server;
        this.logger = logger;
        server.PlayerJoined += HandlePlayerJoin;

        resource = _server.GetAdditionalResource<ScoreboardResource>();

        scoreboardService.StatcheChanged = HandleStateChanged;
        scoreboardService.ColumnsChanged = HandleColumnsChanged;
        scoreboardService.HeaderChanged = HandleHeaderChanged;
    }

    private void HandleStateChanged(Player player, bool enabled)
    {
        player.TriggerLuaEvent("internalSetScoreboardEnabled", player, enabled);
    }

    private void HandleHeaderChanged(Player player, ScoreboardHeader scoreboardHeader)
    {
        player.TriggerLuaEvent("internalSetScoreboardHeader", player, scoreboardHeader.LuaValue);
    }
    
    private void HandleColumnsChanged(Player player, List<ScoreboardColumn> scoreboardColumns)
    {
        player.TriggerLuaEvent("internalSetScoreboardColumns", player, scoreboardColumns.Select(x => x.LuaValue).ToArray());
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            await resource.StartForAsync(player);
            var options = resource.Options;
            player.TriggerLuaEvent("internalUpdateScoreboardConfiguration", player, options.Bind, options.Columns.Select(x => x.LuaValue).ToArray());
        }
        catch(Exception ex)
        {
            this.logger.ResourceFailedToStart<ScoreboardResource>(ex, player);
        }
    }
}
