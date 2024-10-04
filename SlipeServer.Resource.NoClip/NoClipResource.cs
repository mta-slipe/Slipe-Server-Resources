using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.NoClip;

internal class NoClipResource : Resource
{
    private readonly NoClipOptions _options;

    internal NoClipOptions Options => _options;

    internal NoClipResource(MtaServer server, NoClipOptions options)
        : base(server, server.RootElement, "NoClip")
    {
        _options = options;
    }
}