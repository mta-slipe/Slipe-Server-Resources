using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.DGS;

public static class ServerBuilderExtensions
{
    public static void AddDGSResource(this ServerBuilder builder, DGSVersion version)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new DGSResource(server, version);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });

        builder.AddLogic<DGSLogic>();
    }
}
