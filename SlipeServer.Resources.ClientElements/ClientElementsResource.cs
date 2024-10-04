using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.ClientElements;

internal class ClientElementsResource : Resource
{
    internal ClientElementsResource(MtaServer server)
        : base(server, server.RootElement, "ClientElements")
    {
    }

}
