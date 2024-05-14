﻿using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Resources.Base;
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

    internal BoneAttachResource(MtaServer server, BoneAttachVersion version, HttpClient? httpClient = null)
        : base(server, server.RootElement, "boneAttach")
    {
        this.version = version;

        DownloadBoneAttach(httpClient ?? new()).Wait();
        //Root.SetData("DGSI_FileInfo", DGSRecordedFiles, DataSyncType.Broadcast);
        //server.PlayerJoined += Server_PlayerJoined;
    }

    private async Task DownloadBoneAttach(HttpClient httpClient)
    {
        var versionAndChecksum = version switch
        {
            BoneAttachVersion.Release_1_2_0 => ("v1.2.0", new byte[] { 143, 143, 137, 79, 155, 171, 106, 74, 226, 50, 11, 175, 24, 254, 218, 174 }),
            _ => throw new NotImplementedException()
        };
        var versionString = versionAndChecksum.Item1;
        var downloadPath = $"https://github.com/Patrick2562/mtasa-pAttach/releases/download/{versionString}/pAttach-{versionString}.zip";

        using var file = await RemoteResourcesHelper.DownloadOrGetFromCache(httpClient, downloadPath, $"pAttach-{versionString}.zip", versionAndChecksum.Item2);

        using var zip = new ZipArchive(file, ZipArchiveMode.Read);
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