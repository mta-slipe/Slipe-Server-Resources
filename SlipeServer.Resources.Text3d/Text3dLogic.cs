using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Text3d;

public sealed class Text3dOptions : ResourceOptionsBase;

internal sealed class Text3dLogic : ResourceLogicBase<Text3dResource, Text3dOptions>
{
    private readonly Text3dService text3dService;

    public Text3dLogic(MtaServer server, Text3dService text3dService) : base(server)
    {
        this.text3dService = text3dService;
    }

    protected override void HandleResourceStarted(Player player)
    {
        this.text3dService.AddPlayer(player);
    }
}
