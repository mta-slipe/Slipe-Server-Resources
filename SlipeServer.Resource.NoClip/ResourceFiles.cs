using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.NoClip;

internal class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] NoClipLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.NoClip.Lua.NoClip.lua", Assembly);
}
