using Microsoft.Extensions.Logging;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Events;
using SlipeServer.Server.ElementCollections;
using SlipeServer.Server.Services;
using SlipeServer.Resources.Base;

namespace SlipeServer.Resources.Parachute;

public sealed class ParachuteOptions : ResourceOptionsBase;

internal sealed class ParachuteLogic : ResourceLogicBase<ParachuteResource, ParachuteOptions>
{
    private readonly LuaEventService luaEventService;
    private readonly IElementCollection elementCollection;

    public ParachuteLogic(MtaServer server, LuaEventService luaEventService, IElementCollection elementCollection) : base(server)
    {
        this.luaEventService = luaEventService;
        this.elementCollection = elementCollection;

        luaEventService.AddEventHandler("requestAddParachute", HandleRequestAddParachute);
        luaEventService.AddEventHandler("requestRemoveParachute", HandleRequestRemoveParachute);
    }

    public void HandleRequestAddParachute(LuaEvent luaEvent)
    {
        this.logger.LogInformation("{player} started parachuting", luaEvent.Player.Name);

        var otherPlayers = this.elementCollection
            .GetByType<Player>()
            .Except([luaEvent.Player]);
        this.luaEventService.TriggerEventForMany(otherPlayers, "doAddParachuteToPlayer", luaEvent.Player);
    }

    public void HandleRequestRemoveParachute(LuaEvent luaEvent)
    {
        luaEvent.Player.Weapons.Remove(Server.Enums.WeaponId.Parachute);
        this.logger.LogInformation("{player} finished parachuting", luaEvent.Player.Name);

        var otherPlayers = this.elementCollection
            .GetByType<Player>()
            .Except([luaEvent.Player]);
        this.luaEventService.TriggerEventForMany(otherPlayers, "doAddParachuteToPlayer", luaEvent.Player);
    }
}
