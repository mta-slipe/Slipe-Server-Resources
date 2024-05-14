using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.ClientElements;

internal class ClientElementsResource : Resource
{
    internal Dictionary<string, byte[]> AdditionalFiles { get; } = new Dictionary<string, byte[]>()
    {
        ["clientElements.lua"] = ResourceFiles.ClientElementsLua,
    };

    internal ClientElementsResource(MtaServer server)
        : base(server, server.RootElement, "ClientElements")
    {
        foreach (var (path, content) in AdditionalFiles)
            Files.Add(ResourceFileFactory.FromBytes(content, path));
    }

}
