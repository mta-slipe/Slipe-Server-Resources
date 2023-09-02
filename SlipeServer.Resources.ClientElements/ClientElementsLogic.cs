using SlipeServer.Server.Elements;
using SlipeServer.Server;

namespace SlipeServer.Resources.ClientElements;

internal class ClientElementsLogic
{
    private readonly MtaServer _server;
    private readonly ClientElementsResource _resource;

    public ClientElementsLogic(MtaServer server)
    {
        _server = server;
        server.PlayerJoined += HandlePlayerJoin;

        _resource = _server.GetAdditionalResource<ClientElementsResource>();
    }

    private void HandlePlayerJoin(Player player)
    {
        _resource.StartFor(player);
    }
}
