using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Text3d;

internal class Text3dResource : Resource
{
    internal Text3dResource(MtaServer server)
        : base(server, server.RootElement, "Text3d") { }
}