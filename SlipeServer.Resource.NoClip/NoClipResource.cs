using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.NoClip;

public sealed class NoClipResource : Resource
{
    private readonly NoClipOptions _options;

    internal NoClipOptions Options => _options;

    internal NoClipResource(IMtaServer server, NoClipOptions options)
        : base(server, server.RootElement, "NoClip")
    {
        _options = options;
    }
}