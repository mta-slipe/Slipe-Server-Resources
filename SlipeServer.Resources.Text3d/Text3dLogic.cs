using SlipeServer.Server;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Text3d;

internal class Text3dLogic
{
    private readonly MtaServer _server;
    private readonly Text3dResource _resource;
    private readonly Text3dService _text3dService;

    private readonly HashSet<Player> _noClipPlayers = new();

    public Text3dLogic(MtaServer server, Text3dService text3dService)
    {
        _server = server;
        _text3dService = text3dService;
        server.PlayerJoined += HandlePlayerJoin;

        _resource = _server.GetAdditionalResource<Text3dResource>();
    }

    private void HandlePlayerJoin(Player player)
    {
        _resource.StartFor(player);
    }
}
