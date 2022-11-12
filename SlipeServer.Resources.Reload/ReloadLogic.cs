using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;

namespace SlipeServer.Resources.Reload;

public class ReloadLogic
{
    private readonly MtaServer server;
    private readonly ReloadResource resource;

    public ReloadLogic(MtaServer server, LuaEventService luaEventService)
    {
        this.server = server;
        server.PlayerJoined += HandlePlayerJoin;

        luaEventService.AddEventHandler("relWep", HandleWeaponReloadRequest);

        this.resource = this.server.GetAdditionalResource<ReloadResource>();
    }

    private void HandlePlayerJoin(Player player)
    {
        this.resource.StartFor(player);
    }

    public void HandleWeaponReloadRequest(LuaEvent luaEvent)
    {
        luaEvent.Player.ReloadWeapon();
    }
}
