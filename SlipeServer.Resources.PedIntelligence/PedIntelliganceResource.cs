using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.PedIntelligence;

public sealed class PedIntelligenceResource : Resource
{
    internal PedIntelligenceResource(IMtaServer server)
        : base(server, server.RootElement, "PedIntelligence")
    {
    }
}