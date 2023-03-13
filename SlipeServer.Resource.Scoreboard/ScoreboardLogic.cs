using Microsoft.Extensions.Options;
using SlipeServer.Server;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Scoreboard;

internal class ScoreboardLogic
{
    private readonly MtaServer _server;
    private readonly ScoreboardService _scoreboardService;
    private readonly ScoreboardResource _resource;

    public ScoreboardLogic(MtaServer server, ScoreboardService scoreboardService)
    {
        _server = server;
        _scoreboardService = scoreboardService;
        server.PlayerJoined += HandlePlayerJoin;

        _resource = _server.GetAdditionalResource<ScoreboardResource>();
    }

    private async void HandlePlayerJoin(Player player)
    {
        await _resource.StartForAsync(player);
        var options = _resource.Options;
        player.TriggerLuaEvent("internalUpdateConfiguration", player, options.Bind);
    }
}
