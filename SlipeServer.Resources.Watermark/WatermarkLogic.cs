using SlipeServer.Server.Elements;
using SlipeServer.Server;

namespace SlipeServer.Resources.Watermark;

internal class WatermarkLogic
{
    private readonly MtaServer _server;
    private readonly WatermarkResource _resource;
    private readonly WatermarkService _watermarkService;

    public WatermarkLogic(MtaServer server, WatermarkService watermarkService)
    {
        _server = server;
        _watermarkService = watermarkService;
        server.PlayerJoined += HandlePlayerJoin;

        _resource = _server.GetAdditionalResource<WatermarkResource>();
    }

    private void HandlePlayerJoin(Player player)
    {
        _resource.StartFor(player);
    }
}
