using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Reload;

public static class ServerBuilderExtensions
{
    public static void AddReloadResource(this ServerBuilder builder, ReloadOptions options)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ReloadResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
        });

        builder.ConfigureServices(services =>
        {
            services.AddReloadServices(options);
        });

        builder.AddLogic<ReloadLogic>();
    }

    public static IServiceCollection AddReloadServices(this IServiceCollection services, ReloadOptions options)
    {
        services.AddSingleton(Options.Create(options));
        return services;
    }
}
