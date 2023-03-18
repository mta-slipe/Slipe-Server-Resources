using SlipeServer.Packets.Definitions.Lua;

namespace SlipeServer.Resources.Scoreboard;

public class ScoreboardColumn
{
    public enum DataSource
    {
        Property,
        ElementData,
    }

    public string Name { get; set; }
    public string Key { get; set; } // ElementData, PlayerProperty key
    public float Width { get; set; }
    public bool WidthRelative { get; set; }
    public DataSource Source { get; set; }

    internal LuaValue LuaValue => new LuaValue(new LuaValue[]
    {
        new LuaValue(Name),
        new LuaValue(Key),
        new LuaValue(Width),
        new LuaValue(WidthRelative),
        new LuaValue((int)Source),
    });
}
