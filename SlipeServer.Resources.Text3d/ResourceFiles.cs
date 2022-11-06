using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.Text3d;

internal class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] Text3dLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.Text3d.Lua.Text3d.lua", Assembly);
}
