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
                    ["color"] = new LuaValue(new LuaValue[] { (int)Color.R, (int)Color.G, (int)Color.B, (int)Color.A }),
                    ["interior"] = (int)Interior,
                    ["dimension"] = (int)Dimension,
                    ["shadow"] = Shadow == null ? LuaValue.Nil : new LuaValue([Shadow.Value.X, Shadow.Value.Y])
                };

                if (Position != null)
                {
                    var pos = Position.Value;
                    table["position"] = new LuaValue(new LuaValue[] { pos.X, pos.Y, pos.Z });
                }

                if (Element != null)
                    table["element"] = Element;

                return table;
            }
        }
    }

    private ReaderWriterLockSlim _playersLock = new();
    private int _id = 0;
    private readonly Dictionary<int, Text3d> _texts3d = new Dictionary<int, Text3d>();
    private HashSet<Player> _players = new();

    public Text3dService(IElementCollection elementCollection)
    {
        _players = elementCollection.GetByType<Player>().ToHashSet();
    }

    private void AddText3d(Text3d text)
    {
        var parameter = text.LuaValue;
        _texts3d[_id] = text;
        _playersLock.EnterReadLock();
        try
        {
            foreach (var player in _players)
                player.TriggerLuaEvent("internalAddText3d", player, parameter);
        }
        finally
        {
            _playersLock.ExitReadLock();
        }
    }

    internal void AddPlayer(Player player)
    {
        _playersLock.EnterWriteLock();
        _players.Add(player);
        _playersLock.ExitWriteLock();
        player.TriggerLuaEvent("internalAddText3d", player, new LuaValue(_texts3d.Values.Select(x => x.LuaValue)));
        player.Disconnected += (p, e) =>
        {
            _playersLock.EnterWriteLock();
            _players.Remove(p);
            _playersLock.ExitWriteLock();
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
        if (!_texts3d.ContainsKey(id))
            return false;

        _texts3d[id].Text = text;
        _playersLock.EnterReadLock();
        try
        {
            foreach (var player in _players)
                player.TriggerLuaEvent("internalSetText3dText", player, id, text);
        }
        finally
        {
            _playersLock.ExitReadLock();
        }

        return true;
    }

    public bool SetText3dPosition(int id, Vector3 position)
    {
        if (!_texts3d.ContainsKey(id))
            return false;

        _texts3d[id].Position = position;
        _playersLock.EnterReadLock();
        try
        {
            var newPosition = new LuaValue(new LuaValue[] { position.X, position.Y, position.Z });
            foreach (var player in _players)
                player.TriggerLuaEvent("internalSetText3dPosition", player, id, newPosition);
        }
        finally
        {
            _playersLock.ExitReadLock();
        }

        return true;
    }
    
    public bool SetText3dFontSize(int id, float fontSize)
    {
        if (!_texts3d.ContainsKey(id))
            return false;

        _texts3d[id].FontSize = fontSize;
        _playersLock.EnterReadLock();
        try
        {
            foreach (var player in _players)
                player.TriggerLuaEvent("internalSetText3dFontSize", player, id, fontSize);
        }
        finally
        {
            _playersLock.ExitReadLock();
        }

        return true;
    }
    
    public bool SetText3dDistance(int id, float distance)
    {
        if (!_texts3d.ContainsKey(id))
            return false;

        _texts3d[id].Distance = distance;
        _playersLock.EnterReadLock();
        try
        {
            foreach (var player in _players)
                player.TriggerLuaEvent("internalSetText3dDistance", player, id, distance);
        }
        finally
        {
            _playersLock.ExitReadLock();
        }

        return true;
    }

    public bool RemoveText3d(int id)
    {
        if (!_texts3d.ContainsKey(id))
            return false;

        _texts3d.Remove(id);
        _playersLock.EnterReadLock();
        try
        {
            foreach (var player in _players)
                player.TriggerLuaEvent("internalRemoveText3d", player, id);
        }
        finally
        {
            _playersLock.ExitReadLock();
        }

        return true;
    }

    public void SetRenderingEnabled(Player player, bool enabled)
    {
        player.TriggerLuaEvent("internalSetText3dRenderingEnabled", player, enabled);
    }
}
