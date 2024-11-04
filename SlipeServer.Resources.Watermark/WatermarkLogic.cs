using SlipeServer.Server;
using SlipeServer.Resources.Base;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Watermark;

public sealed class WatermarkOptions : ResourceOptionsBase;

internal sealed class WatermarkLogic : ResourceLogicBase<WatermarkResource, WatermarkOptions>
{
    private readonly WatermarkService watermarkService;

    public WatermarkLogic(MtaServer server, WatermarkService watermarkService) : base(server)
    {
        this.watermarkService = watermarkService;
    }

    protected override void HandleResourceStarted(Player player)
    {
        this.watermarkService.AddPlayer(player);
    }
}
