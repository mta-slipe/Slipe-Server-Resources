using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.PedIntelligance;

internal class PedIntelliganceResource : Resource
{
    internal Dictionary<string, byte[]> AdditionalFiles { get; } = new Dictionary<string, byte[]>()
    {
        ["PedIntelligance.lua"] = ResourceFiles.PedIntelliganceLua,
    };

    internal PedIntelliganceResource(MtaServer server)
        : base(server, server.GetRequiredService<RootElement>(), "PedIntelligance")
    {
        foreach (var (path, content) in AdditionalFiles)
            Files.Add(ResourceFileFactory.FromBytes(content, path));
    }

}