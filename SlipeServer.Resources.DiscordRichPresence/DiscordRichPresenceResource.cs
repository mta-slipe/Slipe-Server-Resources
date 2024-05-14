using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;

namespace SlipeServer.Resources.DiscordRichPresence;

internal class DiscordRichPresenceResource : Resource
{
    private readonly DiscordRichPresenceOptions _options;

    internal Dictionary<string, byte[]> AdditionalFiles { get; } = new Dictionary<string, byte[]>()
    {
        ["discordRichPresence.lua"] = ResourceFiles.DiscordRichPresenceLua,
    };

    internal DiscordRichPresenceOptions Options => _options;

    internal DiscordRichPresenceResource(MtaServer server, DiscordRichPresenceOptions options)
        : base(server, server.RootElement, "DiscordRichPresence")
    {
        foreach (var (path, content) in AdditionalFiles)
            Files.Add(ResourceFileFactory.FromBytes(content, path));
        _options = options;
    }

}