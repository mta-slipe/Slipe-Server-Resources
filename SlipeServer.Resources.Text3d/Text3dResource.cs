using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Enums;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Text3d;

internal class Text3dResource : Resource
{
    internal Dictionary<string, byte[]> AdditionalFiles { get; } = new Dictionary<string, byte[]>()
    {
        ["text3d.lua"] = ResourceFiles.Text3dLua,
    };

    internal Text3dResource(MtaServer server)
        : base(server, server.RootElement, "Text3d")
    {
        foreach (var (path, content) in AdditionalFiles)
            Files.Add(ResourceFileFactory.FromBytes(content, path));
    }
}