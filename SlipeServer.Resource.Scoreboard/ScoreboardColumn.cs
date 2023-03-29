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
    public string Font { get; set; } = "sans";
    public string TextAlign { get; set; } = "left";

    internal LuaValue LuaValue => new LuaValue(new LuaValue[]
    {
        new LuaValue(Name),
        new LuaValue(Key),
        new LuaValue(Width),
        new LuaValue(WidthRelative),
        new LuaValue((int)Source),
        new LuaValue(Font),
        new LuaValue(TextAlign),
    });
}
