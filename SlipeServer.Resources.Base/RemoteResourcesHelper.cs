using System.Security.Cryptography;

namespace SlipeServer.Resources.Base;

public static class RemoteResourcesHelper
{
    public static readonly string ResourcesCacheDirectory = "resources-cache";
    public static async Task<Stream> DownloadOrGetFromCache(string url, string filename, byte[] hash)
    {
        if(!Directory.Exists(ResourcesCacheDirectory))
            Directory.CreateDirectory(ResourcesCacheDirectory);

        filename = Path.Join(ResourcesCacheDirectory, filename);
        if(File.Exists(filename))
        {
            var file = File.ReadAllBytes(filename);
            using var md5 = MD5.Create();
            if(md5.ComputeHash(file).SequenceEqual(hash))
            {
                var memoryStream = new MemoryStream(file);
                memoryStream.Position = 0;
                return memoryStream;
            }
            else
            {
                File.Delete(filename);
            }
        }
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var stream = response.Content.ReadAsStream();

        using (Stream file = File.Create(filename))
        {
            stream.CopyTo(file);
        }
        return stream;
    }
}
