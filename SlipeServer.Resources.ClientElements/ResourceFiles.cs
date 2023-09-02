using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.ClientElements;

internal class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] ClientElementsLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.ClientElements.Lua.ClientElements.lua", Assembly);
}
