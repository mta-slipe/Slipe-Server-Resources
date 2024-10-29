using Microsoft.Extensions.Logging;
using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.DGS;

internal class DGSLogic
{
    private readonly MtaServer _server;
    private readonly ILogger<DGSLogic> logger;
    private readonly DGSResource _resource;

    public DGSLogic(MtaServer server, ILogger<DGSLogic> logger)
    {
        _server = server;
        this.logger = logger;
        server.PlayerJoined += HandlePlayerJoin;

        _resource = _server.GetAdditionalResource<DGSResource>();
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            await _resource.StartForAsync(player);
        }
        catch (Exception ex)
        {
            logger.ResourceFailedToStart<DGSResource>(ex, player);
        }
    }
}
