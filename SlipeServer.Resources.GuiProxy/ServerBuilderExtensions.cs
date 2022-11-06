using SlipeServer.Server.ServerBuilders;
using Microsoft.Extensions.DependencyInjection;
using SlipeServer.Resources.GuiProxy.Gui;

namespace SlipeServer.Resources.GuiProxy;

public static class ServerBuilderExtensions
{
    public static void AddGuiProxy(this ServerBuilder builder)
    {
        builder.AddBuildStep(server =>
        {
            var resource = new GuiProxyResource(server);
            server.AddAdditionalResource(resource, resource.AdditionalFiles);
        });
        builder.ConfigureServices(x =>
        {
            x.AddSingleton<GuiProxyService>();
            x.AddTransient<GuiBuilder>();
        });
        builder.AddLogic<GuiProxyLogic>();
    }
}
