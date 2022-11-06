using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.GuiProxy;

public static class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] MainLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.GuiProxy.Lua.Main.lua", Assembly);
}
