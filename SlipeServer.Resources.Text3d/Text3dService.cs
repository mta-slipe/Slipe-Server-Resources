using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.ElementCollections;
using SlipeServer.Server.Elements;
using System.Drawing;
using System.Numerics;

namespace SlipeServer.Resources.Text3d;

public class Text3dService
{
    private class Text3d
    {
        public Vector3? Position { get; set; }
        public Element? Element { get; set; }
        public string Text { get; set; }
        public int Id { get; set; }
        public float FontSize { get; set; }
        public float Distance { get; set; } = 64.0f;
        public Color Color { get; set; } = Color.White;
        public byte Interior { get; set; }
        public ushort Dimension { get; set; }
        public Vector2? Shadow { get; set; }

        public LuaValue LuaValue
        {
            get
            {
                var table = new Dictionary<LuaValue, LuaValue>
                {
                    ["text"] = Text,
                    ["id"] = Id,
                    ["fontSize"] = FontSize,
                    ["distance"] = Distance,
                    ["color"] = new LuaValue([(int)Color.R, (int)Color.G, (int)Color.B, (int)Color.A]),
                    ["interior"] = (int)Interior,
                    ["dimension"] = (int)Dimension,
                    ["shadow"] = Shadow == null ? LuaValue.Nil : new LuaValue([Shadow.Value.X, Shadow.Value.Y])
                };

                if (Position != null)
                {
                    var pos = Position.Value;
                    table["position"] = new LuaValue([pos.X, pos.Y, pos.Z]);
                }

                if (Element != null)
                    table["element"] = Element;

                return table;
            }
        }
    }

    private readonly Lock @lock = new();
    private readonly Dictionary<int, Text3d> _texts3d = [];
    private readonly HashSet<Player> _players = [];
    private int _id = 0;

    public Text3dService(IElementCollection elementCollection)
    {
        _players = elementCollection.GetByType<Player>().ToHashSet();
    }

    private void AddText3d(Text3d text)
    {
        lock (@lock)
        {
            var parameter = text.LuaValue;
            _texts3d[_id] = text;
            foreach (var player in _players)
                player.TriggerLuaEvent("internalAddText3d", player, parameter);
        }
    }

    internal void AddPlayer(Player player)
    {
        lock (@lock)
            _players.Add(player);

        player.TriggerLuaEvent("internalAddText3d", player, new LuaValue(_texts3d.Values.Select(x => x.LuaValue)));
        player.Disconnected += (p, e) =>
        {
            lock(@lock)
                _players.Remove(p);
        };
    }

    public int CreateText3d(Vector3 position, string text, float fontSize = 1.0f, float distance = 64.0f, Color? color = null, byte interior = 0, ushort dimension = 0, Vector2? shadow = null)
    {
        AddText3d(new Text3d
        {
            Id = _id,
            Position = position,
            Text = text,
            FontSize = fontSize,
            Distance = distance,
            Color = color ?? Color.White,
            Interior = interior,
            Dimension = dimension,
            Shadow = shadow
        });
        return _id++;
    }

    public bool SetText3dText(int id, string text)
    {
        lock (@lock)
        {
            if (!_texts3d.TryGetValue(id, out var value))
                return false;

            value.Text = text;

            foreach (var player in _players)
                player.TriggerLuaEvent("internalSetText3dText", player, id, text);
        }

        return true;
    }

    public bool SetText3dPosition(int id, Vector3 position)
    {
        lock (@lock)
        {
            if (!_texts3d.TryGetValue(id, out var value))
                return false;

            value.Position = position;
            var newPosition = new LuaValue([position.X, position.Y, position.Z]);
            foreach (var player in _players)
                player.TriggerLuaEvent("internalSetText3dPosition", player, id, newPosition);
        }

        return true;
    }

    public bool SetText3dFontSize(int id, float fontSize)
    {
        lock (@lock)
        {
            if (!_texts3d.TryGetValue(id, out var value))
                return false;

            value.FontSize = fontSize;

            foreach (var player in _players)
                player.TriggerLuaEvent("internalSetText3dFontSize", player, id, fontSize);
        }

        return true;
    }
    
    public bool SetText3dDistance(int id, float distance)
    {
        lock (@lock)
        {
            if (!_texts3d.TryGetValue(id, out var value))
                return false;

            value.Distance = distance;

            foreach (var player in _players)
                player.TriggerLuaEvent("internalSetText3dDistance", player, id, distance);
        }

        return true;
    }

    public bool RemoveText3d(int id)
    {
        lock (@lock)
        {
            if (!_texts3d.ContainsKey(id))
                return false;

            _texts3d.Remove(id);

            foreach (var player in _players)
                player.TriggerLuaEvent("internalRemoveText3d", player, id);
        }

        return true;
    }

    public void SetRenderingEnabled(Player player, bool enabled)
    {
        player.TriggerLuaEvent("internalSetText3dRenderingEnabled", player, enabled);
    }
}
