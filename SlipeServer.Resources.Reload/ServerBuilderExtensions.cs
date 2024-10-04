using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Reload;
public static class ServerBuilderExtensions
{
    public static void AddReloadResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ReloadResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
        });
        builder.AddLogic<ReloadLogic>();
    }
}
