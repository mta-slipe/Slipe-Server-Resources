using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Scoreboard;

internal class ScoreboardResource : Resource
{
    private readonly ScoreboardOptions _options;

    internal Dictionary<string, byte[]> AdditionalFiles { get; } = new Dictionary<string, byte[]>()
    {
        ["scoreboard.lua"] = ResourceFiles.ScoreboardLua,
    };

    internal ScoreboardOptions Options => _options;

    internal ScoreboardResource(MtaServer server, ScoreboardOptions options)
        : base(server, server.GetRequiredService<RootElement>(), "Scoreboard")
    {
        foreach (var (path, content) in AdditionalFiles)
            Files.Add(ResourceFileFactory.FromBytes(content, path));
        _options = options;
    }

}