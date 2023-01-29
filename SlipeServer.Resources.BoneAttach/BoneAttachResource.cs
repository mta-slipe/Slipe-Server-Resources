using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Enums;
using SlipeServer.Server.Resources;
using SlipeServer.Server.Resources.Interpreters.Meta;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace SlipeServer.Resources.BoneAttach;

internal class BoneAttachResource : Resource
{
    internal Dictionary<string, byte[]> AdditionalFiles { get; } = new();
    private readonly BoneAttachVersion version;

    internal BoneAttachResource(MtaServer server, BoneAttachVersion version)
        : base(server, server.GetRequiredService<RootElement>(), "boneAttach")
    {
        this.version = version;

        DownloadBoneAttach().Wait();
        //Root.SetData("DGSI_FileInfo", DGSRecordedFiles, DataSyncType.Broadcast);
        //server.PlayerJoined += Server_PlayerJoined;
    }

    private async Task DownloadBoneAttach()
    {
        var versionString = version switch
        {
            BoneAttachVersion.Release_1_2_0 => "v1.2.0",
            _ => throw new NotImplementedException()
        };
        var downloadPath = $"https://github.com/Patrick2562/mtasa-pAttach/releases/download/{versionString}/pAttach-{versionString}.zip";

        var client = new HttpClient();
        var response = await client.GetAsync(downloadPath);
        response.EnsureSuccessStatusCode();

        using var zip = new ZipArchive(response.Content.ReadAsStream(), ZipArchiveMode.Read);
        var metaXmlEntry = zip.GetEntry($"pAttach/meta.xml");

        StreamReader streamReader = new StreamReader(metaXmlEntry.Open());
        MetaXml? metaXml = (MetaXml?)new XmlSerializer(typeof(MetaXml)).Deserialize(streamReader);

        foreach (MetaXmlScript item in metaXml.Value.scripts.Where((MetaXmlScript x) => x.Type == "client" || x.Type == "shared"))
        {
            var entry = zip.GetEntry($"pAttach/{item.Source}");
            using StreamReader sr = new StreamReader(entry.Open());
            var data = Encoding.Default.GetBytes(sr.ReadToEnd());
            Files.Add(ResourceFileFactory.FromBytes(data, item.Source, ResourceFileType.ClientScript));
            AdditionalFiles.Add(item.Source, data);
        }

        Exports.AddRange(metaXml.Value.exports
            .Where(x => x.Type == "client")
            .Select(x => x.Function));
    }
}