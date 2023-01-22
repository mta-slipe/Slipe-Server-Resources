using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.Watermark;

internal class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] WatermarkLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.Watermark.Lua.Watermark.lua", Assembly);
}
