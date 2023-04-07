using Microsoft.Extensions.Logging;
using SlipeServer.Server;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Text3d;

internal class Text3dLogic
{
    private readonly Text3dResource resource;
    private readonly Text3dService text3dService;
    private readonly ILogger<Text3dLogic> logger;

    public Text3dLogic(MtaServer server, Text3dService text3dService, ILogger<Text3dLogic> logger)
    {
        this.text3dService = text3dService;
        this.logger = logger;
        server.PlayerJoined += HandlePlayerJoin;

        resource = server.GetAdditionalResource<Text3dResource>();
    }

    private void HandlePlayerJoin(Player player)
    {
        try
        {
            resource.StartForAsync(player);
            text3dService.AddPlayer(player);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to start Text3d resource");
        }
    }
}
