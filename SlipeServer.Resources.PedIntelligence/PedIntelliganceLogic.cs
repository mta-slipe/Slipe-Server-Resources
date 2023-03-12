using SlipeServer.Packets.Structs;
using SlipeServer.Resources.PedIntelligence;
using SlipeServer.Resources.PedIntelligence.Interfaces;
using SlipeServer.Resources.PedIntelligence.PedTasks;
using SlipeServer.Server;
using SlipeServer.Server.ElementCollections;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;
using System.Collections.Concurrent;

namespace SlipeServer.Resources.PedIntelligance;

internal class PedIntelliganceLogic
{
    private readonly MtaServer _server;
    private readonly PedIntelliganceService _pedIntelliganceService;
    private readonly IElementCollection _elementCollection;
    private readonly PedIntelliganceResource _resource;
    private readonly ConcurrentDictionary<Ped, IPedIntelliganceState> _pedIntelliganceStates = new();

    public PedIntelliganceLogic(MtaServer server, PedIntelliganceService pedIntelliganceService, LuaEventService luaEventService, IElementCollection elementCollection)
    {
        this._server = server;
        this._pedIntelliganceService = pedIntelliganceService;
        this._elementCollection = elementCollection;
        this._server.PlayerJoined += HandlePlayerJoin;

        this._resource = _server.GetAdditionalResource<PedIntelliganceResource>();
        this._pedIntelliganceService.RelayPedTasks = HandlePedTasks;

        luaEventService.AddEventHandler("pedFinishedTask", HandlePedFinishedTask);
    }

    private void HandlePedFinishedTask(LuaEvent luaEvent)
    {
        var elementId = luaEvent.Parameters.First().ElementId;
        var ped = (Ped)_elementCollection.Get(elementId.Value);
        if (_pedIntelliganceStates[ped].AdvanceToNextTask())
        {
            _pedIntelliganceStates.TryRemove(ped, out var pedIntelliganceState);
            pedIntelliganceState.Complete();
        }
    }

    private void HandlePlayerJoin(Player player)
    {
        this._resource.StartFor(player);
    }

    private IPedIntelliganceState HandlePedTasks(Ped ped, IEnumerable<PedTask> tasks)
    {
        if (ped.Syncer == null)
            throw new Exception("Ped has no syncer");

        var taskState = new PedIntelliganceState(ped, tasks);
        if (!_pedIntelliganceStates.TryAdd(ped, taskState))
            throw new Exception("Ped has already running tasks");

        ped.Syncer.TriggerLuaEvent("onPedTasks", ped.Syncer, ped, taskState.Id.ToString(), tasks.Select(x => x.ToLuaValue()).ToArray());
        return taskState;
    }
}
