using SlipeServer.Server.Resources;
using SlipeServer.Server;

namespace SlipeServer.Resources.Watermark;

public sealed class WatermarkResource : Resource
{
    internal WatermarkResource(IMtaServer server)
        : base(server, server.RootElement, "Watermark") { }
}