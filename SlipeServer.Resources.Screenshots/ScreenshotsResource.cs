using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Screenshots;

internal class ScreenshotsResource : Resource
{
    internal ScreenshotsResource(MtaServer server)
        : base(server, server.RootElement, "Screenshots")
    {
    }
}