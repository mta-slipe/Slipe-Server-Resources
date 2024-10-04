using SlipeServer.Server;
using SlipeServer.Server.Resources;
using System.Security.Cryptography;

namespace SlipeServer.Resources.Reload;

public class ReloadResource : Resource
{
    public ReloadResource(MtaServer server)
        : base(server, server.RootElement, "Reload")
    {
    }
}
