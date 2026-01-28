namespace SlipeServer.Resources.Assets;

internal class AssetsResource : Resource
{
    internal AssetsResource(IMtaServer server)
        : base(server, server.RootElement, "Assets")
    {
        Exports.Add("assetsGetRawData");
    }
}