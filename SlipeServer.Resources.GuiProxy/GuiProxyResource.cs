﻿using Force.Crc32;
using SlipeServer.Packets.Structs;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.Enums;
using SlipeServer.Server.Resources;
using System.Security.Cryptography;

namespace SlipeServer.Resources.GuiProxy;

public class GuiProxyResource : Resource
{
    public Dictionary<string, byte[]> AdditionalFiles { get; } = new Dictionary<string, byte[]>()
    {
        ["main.lua"] = ResourceFiles.MainLua,
    };

    public GuiProxyResource(MtaServer server)
        : base(server, server.GetRequiredService<RootElement>(), "GuiProxy")
    {
        using var md5 = MD5.Create();

        foreach (var (path, content) in this.AdditionalFiles)
        {
            var hash = md5.ComputeHash(content);
            var checksum = Crc32Algorithm.Compute(content);

            var fileType = path.EndsWith(".lua") ? ResourceFileType.ClientScript : ResourceFileType.ClientFile;
            this.Files.Add(new ResourceFile()
            {
                Name = path,
                AproximateSize = content.Length,
                IsAutoDownload = fileType == ResourceFileType.ClientFile ? true : null,
                CheckSum = checksum,
                FileType = (byte)fileType,
                Md5 = hash
            });
        }
    }
}
