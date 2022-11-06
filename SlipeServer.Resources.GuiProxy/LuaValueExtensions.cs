using SlipeServer.Packets.Definitions.Lua;
using System.Numerics;

namespace SlipeServer.Resources.GuiProxy;
public static class LuaValueExtensions
{
    public static LuaValue ToLuaValue(this Vector2 vector) => new Dictionary<LuaValue, LuaValue>()
    {
        ["X"] = vector.X,
        ["Y"] = vector.Y
    };
    public static LuaValue ToLuaValue(this Vector3 vector) => new Dictionary<LuaValue, LuaValue>()
    {
        ["X"] = vector.X,
        ["Y"] = vector.Y,
        ["Z"] = vector.Z
    };
}
