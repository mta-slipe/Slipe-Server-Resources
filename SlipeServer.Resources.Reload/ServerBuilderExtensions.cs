using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Parachute;
public static class ServerBuilderExtensions
{
    public static void AddReloadResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ReloadResource(server);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });
        builder.AddLogic<ReloadLogic>();
    }
}
