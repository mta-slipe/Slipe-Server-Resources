using SlipeServer.Server.Elements;
using SlipeServer.Server;
using Microsoft.Extensions.Logging;
using SlipeServer.Resources.Base;

namespace SlipeServer.Resources.ClientElements;

internal class ClientElementsLogic
{
    private readonly MtaServer _server;
    private readonly ILogger<ClientElementsLogic> logger;
    private readonly ClientElementsResource _resource;

    public ClientElementsLogic(MtaServer server, ILogger<ClientElementsLogic> logger)
    {
        _server = server;
        this.logger = logger;
        server.PlayerJoined += HandlePlayerJoin;

        _resource = _server.GetAdditionalResource<ClientElementsResource>();
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            await _resource.StartForAsync(player);
        }
        catch(Exception ex)
        {
            logger.ResourceFailedToStart<ClientElementsResource>(ex, player);
        }
    }
}
