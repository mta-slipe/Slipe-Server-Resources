using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.ElementCollections;
using SlipeServer.Server.Elements;

namespace SlipeServer.Resources.Watermark;

public class WatermarkService
{
    private List<Player> _players = new();
    private string? _watermarkContent = null;

    public WatermarkService(IElementCollection elementCollection)
    {
        _players = elementCollection.GetByType<Player>().ToList();
    }

    internal void AddPlayer(Player player)
    {
        _players.Add(player);
        player.TriggerLuaEvent("internalSetWatermarkContent", player, new LuaValue(_watermarkContent));
        player.Disconnected += (p, e) => _players.Remove(p);
    }

    public void SetContent(string? newContent)
    {
        _watermarkContent = newContent;
        foreach (var player in _players)
            player.TriggerLuaEvent("internalSetWatermarkContent", player, newContent ?? LuaValue.Nil);
    }

    public void SetRenderingEnabled(Player player, bool enabled)
    {
        player.TriggerLuaEvent("internalSetWatermarkRenderingEnabled", player, enabled);
    }
}
