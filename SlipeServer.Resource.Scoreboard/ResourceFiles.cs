using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.Scoreboard;

internal class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] ScoreboardLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.Scoreboard.Lua.Scoreboard.lua", Assembly);
}
