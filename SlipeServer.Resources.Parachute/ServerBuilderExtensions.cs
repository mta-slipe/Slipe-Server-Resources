using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Parachute;
public static class ServerBuilderExtensions
{
    public static void AddParachuteResource(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ParachuteResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
        });
        builder.AddLogic<ParachuteLogic>();
    }
}
