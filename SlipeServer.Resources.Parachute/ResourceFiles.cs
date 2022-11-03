using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.Parachute;
public static class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] UtilityLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.Parachute.Lua.utility.lua", Assembly);
    public static byte[] ParachuteClientLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.Parachute.Lua.parachute_cl.lua", Assembly);
    public static byte[] OpenChuteLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.Parachute.Lua.openChute.lua", Assembly);
    public static byte[] SkydivingClientLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.Parachute.Lua.skydiving_cl.lua", Assembly);
    public static byte[] ClientAnimationLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.Parachute.Lua.client_anim.lua", Assembly);
    public static byte[] ParachuteOpenMp3 { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.Parachute.Lua.parachuteopen.mp3", Assembly);
}
