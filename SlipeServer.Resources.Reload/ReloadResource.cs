using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Reload;

public sealed class ReloadResource : Resource
{
    public ReloadResource(IMtaServer server)
        : base(server, server.RootElement, "Reload")
    {
    }
}
