using SlipeServer.Server;
using SlipeServer.Server.Resources;
using System.Security.Cryptography;

namespace SlipeServer.Resources.Parachute;

public class ParachuteResource : Resource
{
    public ParachuteResource(MtaServer server)
        : base(server, server.RootElement, "Parachute")
    {
    }
}
