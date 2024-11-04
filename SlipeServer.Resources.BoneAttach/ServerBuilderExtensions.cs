using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.BoneAttach;

public sealed class BoneAttachOptions : ResourceOptionsBase
{
    public required BoneAttachVersion Version { get; init; }
}

public static class ServerBuilderExtensions
{
    public static void AddBoneAttachResource(this ServerBuilder builder, BoneAttachOptions options, HttpClient? httpClient = null)
    {
        builder.AddBuildStep(server =>
        {
            try
            {
                var resource = new BoneAttachResource(server, options.Version, httpClient ?? server.GetRequiredService<HttpClient>());
                server.AddAdditionalResource(resource, resource.AdditionalFiles);
            }
            catch (Exception ex)
            {
                server.GetRequiredService<ILogger>().LogError(ex, "Failed to add BoneAttach resource");
            }
        });

        builder.ConfigureServices(services =>
        {
            services.AddBoneAttachServices(options);
        });

        builder.AddLogic<BoneAttachLogic>();
    }

    public static IServiceCollection AddBoneAttachServices(this IServiceCollection services, BoneAttachOptions options)
    {
        services.AddSingleton(Options.Create(options));
        services.AddSingleton<BoneAttachService>();
        return services;
    }
}
