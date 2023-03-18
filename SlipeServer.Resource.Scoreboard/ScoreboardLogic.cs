using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Scoreboard;

internal class ScoreboardLogic
{
    private readonly MtaServer _server;
    private readonly ILogger<ScoreboardLogic> _logger;
    private readonly ScoreboardResource _resource;

    public ScoreboardLogic(MtaServer server, ScoreboardService scoreboardService, ILogger<ScoreboardLogic> logger)
    {
        _server = server;
        _logger = logger;
        server.PlayerJoined += HandlePlayerJoin;

        _resource = _server.GetAdditionalResource<ScoreboardResource>();

        scoreboardService.ScoreboardStateChanged = HandleScoreboardStateChanged;
    }

    private void HandleScoreboardStateChanged(Player player, bool enabled)
    {
        player.TriggerLuaEvent("internalSetScoreboardEnabled", player, enabled);
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            await _resource.StartForAsync(player);
            var options = _resource.Options;
            player.TriggerLuaEvent("internalUpdateScoreboardConfiguration", player, options.Bind, options.Columns.Select(x => x.LuaValue).ToArray());
            _logger.LogInformation("kk");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Failed to start scoreboard resource for player {playerName}", player.Name);
        }
    }
}
