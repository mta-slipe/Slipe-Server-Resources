using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.Scoreboard;

public sealed class ScoreboardResource : Resource
{
    private readonly ScoreboardOptions _options;

    internal ScoreboardOptions Options => _options;

    internal ScoreboardResource(IMtaServer server, ScoreboardOptions options)
        : base(server, server.RootElement, "Scoreboard")
    {
        _options = options;
    }

}