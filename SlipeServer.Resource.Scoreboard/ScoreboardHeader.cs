using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Resources;
using static SlipeServer.Packets.Constants.KeyConstants;
using System.Xml.Linq;

namespace SlipeServer.Resources.Scoreboard;

public class ScoreboardHeader
{
    public string Text { get; set; }
    public float Size { get; set; }
    public string Font { get; set; } = "sans";

    internal LuaValue LuaValue => new LuaValue(new LuaValue[]
    {
        new LuaValue(Text),
        new LuaValue(Size),
        new LuaValue(Font),
    });
}
