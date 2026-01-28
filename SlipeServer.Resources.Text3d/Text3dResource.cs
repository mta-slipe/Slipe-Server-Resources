using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Text3d;

public sealed class Text3dResource : Resource
{
    internal Text3dResource(IMtaServer server)
        : base(server, server.RootElement, "Text3d") { }
}