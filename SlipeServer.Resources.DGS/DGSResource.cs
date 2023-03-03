using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Resources.Base;
using SlipeServer.Resources.DGS.Style;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Enums;
using SlipeServer.Server.Resources;
using SlipeServer.Server.Resources.Interpreters.Meta;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace SlipeServer.Resources.DGS;

internal class DGSResource : Resource
{
    internal Dictionary<string, byte[]> AdditionalFiles { get; } = new();
    private readonly Dictionary<LuaValue, LuaValue> DGSRecordedFiles = new();
    private readonly DGSVersion version;
    private readonly DGSStyle? dgsStyle;

    internal DGSResource(MtaServer server, DGSVersion version, DGSStyle? dgsStyle = null)
        : base(server, server.GetRequiredService<RootElement>(), "dgs")
    {
        this.version = version;
        this.dgsStyle = dgsStyle;
        DownloadDGS().Wait();
        //Root.SetData("DGSI_FileInfo", DGSRecordedFiles, DataSyncType.Broadcast);
        server.PlayerJoined += Server_PlayerJoined;
    }

    private async Task DownloadDGS()
    {
        string? customStyle = dgsStyle?.ToString();

#if DEBUG
        if(dgsStyle != null)
            File.WriteAllText("debugStyle.lua", customStyle);
#endif

        var versionAndChecksum = version switch
        {
            DGSVersion.Release_3_520 => ("3.520", new byte[] { 118, 232, 221, 243, 54, 89, 247, 244, 47, 43, 81, 92, 115, 241, 76, 144 }),
            _ => throw new NotImplementedException()
        };

        var versionString = versionAndChecksum.Item1;
        var downloadPath = $"https://github.com/thisdp/dgs/archive/refs/tags/{versionString}.zip";

        using var file = await RemoteResourcesHelper.DownloadOrGetFromCache(downloadPath, $"dgs-{versionString}.zip", versionAndChecksum.Item2);

        using var zip = new ZipArchive(file, ZipArchiveMode.Read);
        var metaXmlEntry = zip.GetEntry($"dgs-{versionString}/meta.xml");

        StreamReader streamReader = new StreamReader(metaXmlEntry.Open());
        MetaXml? metaXml = (MetaXml?)new XmlSerializer(typeof(MetaXml)).Deserialize(streamReader);

        using SHA256 sha256Hash = SHA256.Create();
        foreach (MetaXmlFile item in metaXml.Value.files)
        {
            var entry = zip.GetEntry($"dgs-{versionString}/{item.Source}");
            byte[] data;
            if (customStyle != null && item.Source == "styleManager/Default/styleSettings.txt")
            {
                data = System.Text.UTF8Encoding.UTF8.GetBytes(customStyle);
            }
            else
            {
                using MemoryStream ms = new MemoryStream();
                entry.Open().CopyTo(ms);
                data = ms.ToArray();
            }
            Files.Add(ResourceFileFactory.FromBytes(data, item.Source, ResourceFileType.ClientFile));
            AdditionalFiles.Add(item.Source, data);
            string hash = GetHash(sha256Hash, data);
            DGSRecordedFiles.Add(item.Source, new LuaValue(new LuaValue[] { hash, data.Length }));
        }

        foreach (MetaXmlScript item in metaXml.Value.scripts.Where((MetaXmlScript x) => x.Type == "client"))
        {
            var entry = zip.GetEntry($"dgs-{versionString}/{item.Source}");
            using StreamReader sr = new StreamReader(entry.Open());
            var data = Encoding.Default.GetBytes(sr.ReadToEnd());
            Files.Add(ResourceFileFactory.FromBytes(data, item.Source, ResourceFileType.ClientScript));
            AdditionalFiles.Add(item.Source, data);
        }
        Exports.AddRange(metaXml.Value.exports
            .Where(x => x.Type == "client")
            .Select(x => x.Function));
    }

    private void Server_PlayerJoined(Player obj)
    {
        Root.SetData("DGSI_FileInfo", DGSRecordedFiles, DataSyncType.Broadcast);
    }

    private static string GetHash(HashAlgorithm hashAlgorithm, byte[] input)
    {
        byte[] data = hashAlgorithm.ComputeHash(input);

        var sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        return sBuilder.ToString();
    }
}