using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SlipeServer.Resources.Base;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.Parachute;

public static class ServerBuilderExtensions
{
    public static void AddParachuteResource(this ServerBuilder builder, ParachuteOptions options)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new ParachuteResource(server);
            var additionalFiles = resource.GetAndAddLuaFiles();
            server.AddAdditionalResource(resource, additionalFiles);
        });
        builder.ConfigureServices(services =>
        {
            services.AddParachuteServices(options);
        });
        builder.AddLogic<ParachuteLogic>();
    }

    public static IServiceCollection AddParachuteServices(this IServiceCollection services, ParachuteOptions options)
    {
        services.AddSingleton(Options.Create(options));
        return services;
    }
}
