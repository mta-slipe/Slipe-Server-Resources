using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.PedIntelligence;

internal class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] PedIntelligenceLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.PedIntelligence.Lua.PedIntelligence.lua", Assembly);
}
