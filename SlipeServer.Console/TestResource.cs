using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Resources;
using System.Reflection;

namespace SlipeServer.Console;

public class TestResource : Resource
{
    public TestResource(MtaServer server) : base(server, server.RootElement, "TestResource")
    {
        NoClientScripts[$"{Name}/test.lua"] = EmbeddedResourceHelper.GetLuaFile("SlipeServer.Console.Test.lua", Assembly.GetExecutingAssembly());
    }
}
