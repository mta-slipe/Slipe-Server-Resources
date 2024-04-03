using Microsoft.Extensions.Logging;
using SlipeServer.Resources.DGS.Style;
using SlipeServer.Server.ServerBuilders;

namespace SlipeServer.Resources.DGS;

public static class ServerBuilderExtensions
{
    public static void AddDGSResource(this ServerBuilder builder, DGSVersion version, DGSStyle? dgsStyle = null, HttpClient? httpClient = null)
    {
        builder.AddBuildStep(server =>
        {
            try
            {
                var resource = new DGSResource(server, version, dgsStyle, httpClient ?? server.GetRequiredService<HttpClient>());
                server.AddAdditionalResource(resource, resource.AdditionalFiles);
            }
            catch (Exception ex)
            {
                server.GetRequiredService<ILogger>().LogError(ex, "Failed to add DGS resource");
            }
        });

        builder.AddLogic<DGSLogic>();
    }
}
