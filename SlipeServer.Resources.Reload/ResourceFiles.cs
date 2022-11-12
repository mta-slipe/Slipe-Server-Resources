using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.Reload;
public static class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] ReloadLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.Reload.Lua.reload.lua", Assembly);
}
