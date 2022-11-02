using SlipeServer.Resources.Base;

namespace SlipeServer.Resources.NoClip;

internal class ResourceFiles
{
    public static byte[] NoClipLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resource.NoClip.Lua.NoClip.lua");
}
