using SlipeServer.Server.Elements;
using SlipeServer.Server;
using Microsoft.Extensions.Logging;
using SlipeServer.Resources.Base;

namespace SlipeServer.Resources.Watermark;

internal class WatermarkLogic
{
    private readonly MtaServer _server;
    private readonly WatermarkResource resource;
    private readonly WatermarkService _watermarkService;
    private readonly ILogger<WatermarkLogic> logger;

    public WatermarkLogic(MtaServer server, WatermarkService watermarkService, ILogger<WatermarkLogic> logger)
    {
        _server = server;
        _watermarkService = watermarkService;
        this.logger = logger;
        server.PlayerJoined += HandlePlayerJoin;

        this.resource = _server.GetAdditionalResource<WatermarkResource>();
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            await this.resource.StartForAsync(player);
        }
        catch (Exception ex)
        {
            this.logger.ResourceFailedToStart<WatermarkResource>(ex, player);
        }
    }
}
