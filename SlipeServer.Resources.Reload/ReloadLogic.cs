using Microsoft.Extensions.Logging;
using SlipeServer.Resources.Base;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;

namespace SlipeServer.Resources.Reload;

public class ReloadLogic
{
    private readonly MtaServer server;
    private readonly ILogger<ReloadLogic> logger;
    private readonly ReloadResource resource;

    public ReloadLogic(MtaServer server, LuaEventService luaEventService, ILogger<ReloadLogic> logger)
    {
        this.server = server;
        this.logger = logger;
        server.PlayerJoined += HandlePlayerJoin;

        luaEventService.AddEventHandler("relWep", HandleWeaponReloadRequest);

        this.resource = this.server.GetAdditionalResource<ReloadResource>();
    }

    private async void HandlePlayerJoin(Player player)
    {
        try
        {
            await this.resource.StartForAsync(player);
        }
        catch (Exception ex)
        {
            logger.ResourceFailedToStart<ReloadResource>(ex, player);
        }
    }

    public void HandleWeaponReloadRequest(LuaEvent luaEvent)
    {
        luaEvent.Player.ReloadWeapon();
    }
}
