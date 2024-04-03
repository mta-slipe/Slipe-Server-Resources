using SlipeServer.Server.Resources;
using System.Security.Cryptography;
using System.Text;

namespace SlipeServer.Resources.Base;

public static class ResourceExtensions
{
    public static Dictionary<string, byte[]> GetAndAddLuaFiles(this Resource resource, string basePath = "Lua", bool obfuscateName = false, bool cache = true, bool obfuscateSources = false, HttpClient? httpClient = null)
    {
        Dictionary<string, byte[]> additionalFiles = [];

        var assembly = resource.GetType().Assembly;
        var nameBase = $"{resource.GetType().Namespace}.{basePath}";
        var resourceNames = assembly.GetManifestResourceNames();
        if (resourceNames.Length == 0)
            throw new InvalidOperationException("No embedded resources found.");

        foreach (var item in resourceNames)
        {
            if (item.StartsWith(nameBase))
            {
                var extension = Path.GetExtension(item).ToLower();
                if (extension is ".lua" or ".luac")
                {
                    var path = item[(nameBase.Length + 1)..];
                    var content = EmbeddedResourceHelper.GetLuaFile(item, assembly);

                    if (obfuscateName)
                    {
                        var bytes = MD5.HashData(content);
                        var builder = new StringBuilder();
                        for (int i = 0; i < bytes.Length; i++)
                            builder.Append(bytes[i].ToString("X2"));

                        path = builder.ToString() + extension;
                    }

                    if (extension is ".lua" && obfuscateSources)
                    {
                        path = Path.ChangeExtension(path, ".luac");
                        content = ResourcesHelper.Obfuscate(httpClient ?? new(), content, path).Result;
                    }

                    if (cache)
                    {
                        resource.Files.Add(ResourceFileFactory.FromBytes(content, path));
                    }
                    else
                    {
                        resource.NoClientScripts[path] = content;
                    }

                    additionalFiles[path] = content;
                }
            }
        }

        if (additionalFiles.Count == 0)
            throw new InvalidOperationException($"No lua files found in path: {nameBase} but found {resourceNames.Length} embedded resources.");

        return additionalFiles;
    }
}
