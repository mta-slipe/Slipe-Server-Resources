using SlipeServer.Resources.GuiProxy.Gui;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Services;

namespace SlipeServer.Resources.GuiProxy;
public class GuiProxyService
{
    private readonly LuaEventService luaEventService;
    private readonly RootElement rootElement;
    private readonly GuiProxyResource resource;

    public GuiProxyResource Resource => this.resource;

    public GuiProxyService(
        MtaServer server,
        LuaEventService luaEventService,
        RootElement rootElement)
    {
        this.luaEventService = luaEventService;
        this.rootElement = rootElement;
        this.resource = server.GetAdditionalResource<GuiProxyResource>();
    }

    public GuiBuilder GetBuilder()
    {
        return new GuiBuilder(this.luaEventService, this.rootElement);
    }

}
