using System.Reflection;

namespace SlipeServer.Resources.Base
{
    public static class EmbeddedResourceHelper
    {
        public static byte[] GetLuaFile(string name, Assembly assembly)
        {
#if DEBUG
            var debugListOfLuaFiles = assembly.GetManifestResourceNames();
#endif
            using var stream = assembly.GetManifestResourceStream(name);

            if (stream == null)
                throw new FileNotFoundException($"File \"{name}\" not found in embedded resources.");

            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}
