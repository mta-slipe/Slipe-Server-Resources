using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.DiscordRichPresence;

internal class DiscordRichPresenceResource : Resource
{
    private readonly DiscordRichPresenceOptions _options;

    internal DiscordRichPresenceOptions Options => _options;

    internal DiscordRichPresenceResource(MtaServer server, DiscordRichPresenceOptions options)
        : base(server, server.RootElement, "DiscordRichPresence")
    {
        _options = options;
    }

}