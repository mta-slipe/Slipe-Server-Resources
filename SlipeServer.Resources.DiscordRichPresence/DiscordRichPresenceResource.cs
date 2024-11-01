using SlipeServer.Server;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.DiscordRichPresence;

public sealed class DiscordRichPresenceResource : Resource
{
    internal DiscordRichPresenceResource(MtaServer server) : base(server, server.RootElement, "DiscordRichPresence") { }
}