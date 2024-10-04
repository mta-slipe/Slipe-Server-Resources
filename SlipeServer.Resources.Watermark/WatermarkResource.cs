using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;
using SlipeServer.Server;

namespace SlipeServer.Resources.Watermark;

internal class WatermarkResource : Resource
{
    private readonly WatermarkService _watermarkService;

    internal WatermarkResource(MtaServer server)
        : base(server, server.RootElement, "Watermark")
    {
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