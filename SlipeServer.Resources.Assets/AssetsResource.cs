namespace SlipeServer.Resources.Assets;

internal class AssetsResource : Resource
{
    internal AssetsResource(MtaServer server)
        : base(server, server.RootElement, "Assets")
    {
        Exports.Add("assetsGetRawData");
    }
}