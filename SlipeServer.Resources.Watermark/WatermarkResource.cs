using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;
using SlipeServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlipeServer.Resources.Watermark;

internal class WatermarkResource : Resource
{
    internal Dictionary<string, byte[]> AdditionalFiles { get; } = new Dictionary<string, byte[]>()
    {
        ["watermark.lua"] = ResourceFiles.WatermarkLua,
    };

    private readonly WatermarkService _watermarkService;

    internal WatermarkResource(MtaServer server)
        : base(server, server.RootElement, "Watermark")
    {
        foreach (var (path, content) in AdditionalFiles)
            Files.Add(ResourceFileFactory.FromBytes(content, path));

        _watermarkService = server.GetRequiredService<WatermarkService>();
        server.PlayerJoined += Server_PlayerJoined;

    }
    private void Server_PlayerJoined(Player player)
    {
        player.ResourceStarted += Player_ResourceStarted;
    }

    private void Player_ResourceStarted(Player player, Server.Elements.Events.PlayerResourceStartedEventArgs e)
    {
        if (e.NetId == NetId)
            _watermarkService.AddPlayer(player);
    }
}