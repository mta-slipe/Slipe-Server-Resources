using Microsoft.Extensions.Logging;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Events;
using SlipeServer.Server.ElementCollections;
using SlipeServer.Server.Services;

namespace SlipeServer.Resources.Parachute;

public class ReloadLogic
{
    private readonly MtaServer server;
    private readonly LuaEventService luaEventService;
    private readonly ILogger logger;
    private readonly IElementCollection elementCollection;
    private readonly ReloadResource resource;

    public ReloadLogic(MtaServer server,
        LuaEventService luaEventService,
        ILogger logger,
        IElementCollection elementCollection)
    {
        this.server = server;
        this.luaEventService = luaEventService;
        this.logger = logger;
        this.elementCollection = elementCollection;
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
