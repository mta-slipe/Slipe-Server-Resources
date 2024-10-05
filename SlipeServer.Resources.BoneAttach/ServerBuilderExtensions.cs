using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.BoneAttach;

public sealed class BoneAttachOptions
{
    public required BoneAttachVersion Version { get; init; }
}

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
            services.AddBoneAttachServices(version);
        });

        builder.AddLogic<BoneAttachLogic>();
    }

    public static IServiceCollection AddBoneAttachServices(this IServiceCollection services, BoneAttachVersion version)
    {
        services.AddSingleton<IOptions<BoneAttachOptions>>(new OptionsWrapper<BoneAttachOptions>(new BoneAttachOptions
        {
            Version = version
        }));
        services.AddSingleton<BoneAttachService>();
        return services;
    }
}
