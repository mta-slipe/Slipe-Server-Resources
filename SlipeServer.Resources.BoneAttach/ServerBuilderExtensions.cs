using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.BoneAttach;

public static class ServerBuilderExtensions
{
    public static void AddBoneAttachResource(this ServerBuilder builder, BoneAttachVersion version, HttpClient? httpClient = null)
    {
        builder.AddBuildStep(server =>
        {
            try
            {
                var resource = new BoneAttachResource(server, version, httpClient ?? server.GetRequiredService<HttpClient>());
                server.AddAdditionalResource(resource, resource.AdditionalFiles);
            }
            catch (Exception ex)
            {
                server.GetRequiredService<ILogger>().LogError(ex, "Failed to add BoneAttach resource");
            }
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<BoneAttachService>();
        });

        builder.AddLogic<BoneAttachLogic>();
    }
}
