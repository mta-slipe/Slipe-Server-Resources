using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server;
using SlipeServer.Server.ElementCollections;
using SlipeServer.Server.Elements;
using System.Numerics;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace SlipeServer.Resources.Text3d;

public class Text3dService
{
    private class Text3d
    {
        public Vector3? Position { get; set; }
        public Element? Element { get; set; }
        public string Text { get; set; }
        public int Id { get; set; }
    }

    private int _id = 0;
    private readonly Dictionary<int, Text3d> _texts3d = new Dictionary<int, Text3d>();
    private HashSet<Player> _players = new();

    public Text3dService(MtaServer mtaServer, IElementCollection elementCollection)
    {
        _players = elementCollection.GetByType<Player>().ToHashSet();
    }

    private LuaValue Text3dToLuaValue(Text3d text)
    {
        var table = new Dictionary<LuaValue, LuaValue>
        {
            ["text"] = text.Text,
            ["id"] = text.Id,
        };
        if(text.Position != null)
        {
            var pos = text.Position.Value;
            table["position"] = new LuaValue(new LuaValue[] { pos.X, pos.Y, pos.Z });
        }
        
        if(text.Element != null)
        {
            table["element"] = text.Element;
        }
        return table;
    }

    private void AddText3d(Text3d text)
    {
        var parameter = Text3dToLuaValue(text);
        _texts3d[_id] = text;
        foreach (var player in _players)
            player.TriggerLuaEvent("internalAddText3d", player, parameter);
    }

    internal void AddPlayer(Player player)
    {
        _players.Add(player);
        player.TriggerLuaEvent("internalAddText3d", player, new LuaValue(_texts3d.Values.Select(Text3dToLuaValue)));
        player.Disconnected += (p, e) => _players.Remove(p);
    }

    public int CreateText3d(Vector3 position, string text)
    {
        AddText3d(new Text3d
        {
            Position = position,
            Text = text,
            Id = _id,
        });
        return _id++;
    }

    public bool UpdateText3d(int id, string text)
    {
        if (!_texts3d.ContainsKey(id))
            return false;

        _texts3d[id].Text = text;
        foreach (var player in _players)
            player.TriggerLuaEvent("internalUpdateText3d", player, id, text);

        return true;
    }

    public bool RemoveText3d(int id)
    {
        if (!_texts3d.ContainsKey(id))
            return false;

        _texts3d.Remove(id);
        foreach (var player in _players)
            player.TriggerLuaEvent("internalRemoveText3d", player, id);

        return true;
    }

    public void SetRenderingEnabled(Player player, bool enabled)
    {
        player.TriggerLuaEvent("internalSetText3dRenderingEnabled", player, enabled);
    }
}
