using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.ClientElements;

public sealed class ClientElementsResource : Resource
{
    internal ClientElementsResource(IMtaServer server)
        : base(server, server.RootElement, "ClientElements")
    {
    }

}
