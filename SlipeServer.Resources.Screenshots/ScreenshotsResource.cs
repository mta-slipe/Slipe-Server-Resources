using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Screenshots;

public sealed class ScreenshotsResource : Resource
{
    internal ScreenshotsResource(IMtaServer server)
        : base(server, server.RootElement, "Screenshots")
    {
    }
}