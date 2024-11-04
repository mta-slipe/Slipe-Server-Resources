using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;

namespace SlipeServer.Resources.Reload;

public sealed class ReloadOptions : ResourceOptionsBase;

internal sealed class ReloadLogic : ResourceLogicBase<ReloadResource, ReloadOptions>
{
    public ReloadLogic(MtaServer server, LuaEventService luaEventService, ResourceStartedManager? resourceStartedManager = null) : base(server)
    {
        luaEventService.AddEventHandler("relWep", HandleWeaponReloadRequest);
    }

    public void HandleWeaponReloadRequest(LuaEvent luaEvent)
    {
        luaEvent.Player.ReloadWeapon();
    }
}
