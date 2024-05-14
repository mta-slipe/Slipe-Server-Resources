using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.PedIntelligence;

internal class PedIntelligenceResource : Resource
{
    internal Dictionary<string, byte[]> AdditionalFiles { get; } = new Dictionary<string, byte[]>()
    {
        ["PedIntelligence.lua"] = ResourceFiles.PedIntelligenceLua,
    };

    internal PedIntelligenceResource(MtaServer server)
        : base(server, server.RootElement, "PedIntelligence")
    {
        foreach (var (path, content) in AdditionalFiles)
            Files.Add(ResourceFileFactory.FromBytes(content, path));
    }
}