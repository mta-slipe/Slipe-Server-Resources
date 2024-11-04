using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Parachute;

public sealed class ParachuteResource : Resource
{
    public ParachuteResource(MtaServer server)
        : base(server, server.RootElement, "Parachute")
    {
    }
}
