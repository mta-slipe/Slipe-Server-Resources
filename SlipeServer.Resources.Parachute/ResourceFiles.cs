using SlipeServer.Resources.Base;

namespace SlipeServer.Resources.Parachute;
public static class ResourceFiles
{
    public static byte[] UtilityLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Console.AdditionalResources.Parachute.Lua.utility.lua");
    public static byte[] ParachuteClientLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Console.AdditionalResources.Parachute.Lua.parachute_cl.lua");
    public static byte[] OpenChuteLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Console.AdditionalResources.Parachute.Lua.openChute.lua");
    public static byte[] SkydivingClientLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Console.AdditionalResources.Parachute.Lua.skydiving_cl.lua");
    public static byte[] ClientAnimationLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Console.AdditionalResources.Parachute.Lua.client_anim.lua");
    public static byte[] ParachuteOpenMp3 { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Console.AdditionalResources.Parachute.Lua.parachuteopen.mp3");
}
