using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Resources;
using System.Reflection;

namespace SlipeServer.Console;

public class TestResource : Resource
{
    public TestResource(IMtaServer server) : base(server, server.RootElement, "TestResource")
    {
        AddNoClientScript($"{Name}/test.lua", EmbeddedResourceHelper.GetLuaFile("SlipeServer.Console.Test.lua", Assembly.GetExecutingAssembly()));
    }
}
