using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;
using System.Security.Cryptography;

namespace SlipeServer.Resources.Reload;

public class ReloadResource : Resource
{
    public Dictionary<string, byte[]> AdditionalFiles { get; } = new Dictionary<string, byte[]>()
    {
        ["utility.lua"] = ResourceFiles.ReloadLua,
    };

    public ReloadResource(MtaServer server)
        : base(server, server.RootElement, "Reload")
    {
        using var md5 = MD5.Create();

        foreach (var (path, content) in this.AdditionalFiles)
            this.Files.Add(ResourceFileFactory.FromBytes(content, path));
    }
}
