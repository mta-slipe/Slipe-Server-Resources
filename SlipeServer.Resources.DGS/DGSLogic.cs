using SlipeServer.Server;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.DGS;

internal class DGSLogic
{
    private readonly MtaServer _server;
    private readonly DGSResource _resource;

    public DGSLogic(MtaServer server)
    {
        _server = server;
        server.PlayerJoined += HandlePlayerJoin;

        _resource = _server.GetAdditionalResource<DGSResource>();
    }

    private void HandlePlayerJoin(Player player)
    {
        _resource.StartFor(player);
    }
}
