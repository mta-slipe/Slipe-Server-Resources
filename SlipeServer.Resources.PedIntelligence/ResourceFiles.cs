using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.PedIntelligance;

internal class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] PedIntelliganceLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.PedIntelligence.Lua.PedIntelligance.lua", Assembly);
}
