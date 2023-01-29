using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.BoneAttach;

public static class ServerBuilderExtensions
{
    public static void AddBoneAttachResource(this ServerBuilder builder, BoneAttachVersion version)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new BoneAttachResource(server, version);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<BoneAttachService>();
        });

        builder.AddLogic<BoneAttachLogic>();
    }
}
