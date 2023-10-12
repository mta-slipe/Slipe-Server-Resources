using SlipeServer.Resources.Base;
using System.Reflection;

namespace SlipeServer.Resources.DiscordRichPresence;

internal class ResourceFiles
{
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static byte[] DiscordRichPresenceLua { get; } = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Resources.DiscordRichPresence.Lua.DiscordRichPresence.lua", Assembly);
}
