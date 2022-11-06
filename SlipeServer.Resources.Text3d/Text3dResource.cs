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

    private readonly Text3dService _text3DService;

    internal Text3dResource(MtaServer server)
        : base(server, server.GetRequiredService<RootElement>(), "Text3d")
    {
        foreach (var (path, content) in AdditionalFiles)
            Files.Add(ResourceFileFactory.FromBytes(content, path));

        _text3DService = server.GetRequiredService<Text3dService>();
        server.PlayerJoined += Server_PlayerJoined;

    }
    private void Server_PlayerJoined(Player player)
    {
        player.ResourceStarted += Player_ResourceStarted;
    }

    private void Player_ResourceStarted(Player player, Server.Elements.Events.PlayerResourceStartedEventArgs e)
    {
        if(e.NetId == NetId)
            _text3DService.AddPlayer(player);
    }
}