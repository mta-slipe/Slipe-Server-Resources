using SlipeServer.Server;
using SlipeServer.Server.Services;

namespace SlipeServer.Resources.GuiProxy;
public class GuiProxyLogic
{
    private readonly LuaEventService luaEventService;
    private readonly GuiProxyResource resource;

    public GuiProxyLogic(MtaServer server, LuaEventService luaEventService)
    {
        this.luaEventService = luaEventService;
        this.resource = server.GetAdditionalResource<GuiProxyResource>();

        server.PlayerJoined += HandlePlayerJoin;
    }


    private void HandlePlayerJoin(Server.Elements.Player player)
    {
        this.resource.StartFor(player);
    }
}
