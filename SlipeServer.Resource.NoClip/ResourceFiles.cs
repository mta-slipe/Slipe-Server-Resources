using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SlipeServer.Resource.NoClip;

internal class ResourceFiles
{
    public static byte[] NoClipLua { get; } = GetLuaFile("SlipeServer.Resource.NoClip.Lua.NoClip.lua");

    private static byte[] GetLuaFile(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(name);

        if (stream == null)
            throw new FileNotFoundException($"File \"{name}\" not found in embedded resources.");

        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        return buffer;
    }
}
