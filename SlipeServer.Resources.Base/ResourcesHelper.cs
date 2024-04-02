namespace SlipeServer.Resources.Base;

public static class ResourcesHelper
{
    public static async Task<byte[]> Obfuscate(HttpClient httpClient, byte[] data, string fileName)
    {
        var formData = new MultipartFormDataContent
        {
            { new StringContent("1"), "compile" },
            { new StringContent("0"), "debug" },
            { new StringContent("3"), "obfuscate" },
            { new ByteArrayContent(data), "luasource", fileName }
        };

        var response = await httpClient.PostAsync("http://luac.mtasa.com/", formData);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsByteArrayAsync();
    }
}
