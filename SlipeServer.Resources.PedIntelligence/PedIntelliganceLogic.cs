using SlipeServer.Resources.PedIntelligence.Exceptions;
using SlipeServer.Resources.PedIntelligence.Interfaces;
using SlipeServer.Resources.PedIntelligence.PedTasks;
using SlipeServer.Server;
using SlipeServer.Server.ElementCollections;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Events;
using SlipeServer.Server.Services;
using System.Collections.Concurrent;

namespace SlipeServer.Resources.PedIntelligence;

internal class PedIntelligenceLogic
{
    private readonly MtaServer _server;
    private readonly PedIntelligenceService _pedIntelligenceService;
    private readonly IElementCollection _elementCollection;
    private readonly PedIntelligenceResource _resource;
    private readonly ConcurrentDictionary<Ped, IPedIntelligenceState> _pedIntelligenceStates = new();
    private readonly ConcurrentDictionary<Ped, ObstacleAvoidanceStrategies> _pedObstacleAvoidanceStrategies = new();

    public PedIntelligenceLogic(MtaServer server, PedIntelligenceService pedIntelligenceService, LuaEventService luaEventService, IElementCollection elementCollection)
    {
        this._server = server;
        this._pedIntelligenceService = pedIntelligenceService;
        this._elementCollection = elementCollection;
        this._server.PlayerJoined += HandlePlayerJoin;

        this._resource = _server.GetAdditionalResource<PedIntelligenceResource>();
        this._pedIntelligenceService.RelayPedTasks = HandlePedTasks;
        this._pedIntelligenceService.RelayPedObstacleAvoidanceStrategies = HandlePedObstacleAvoidanceStrategies;

        luaEventService.AddEventHandler("pedFinishedTask", HandlePedFinishedTask);
        luaEventService.AddEventHandler("pedStuck", HandlePedStuck);
    }

    private void HandlePedFinishedTask(LuaEvent luaEvent)
    {
        var elementId = luaEvent.Parameters.First().ElementId;
        var ped = (Ped)_elementCollection.Get(elementId.Value);
        if (_pedIntelligenceStates[ped].AdvanceToNextTask() && _pedIntelligenceStates.TryRemove(ped, out var pedIntelligenceState))
        {
            ped.Destroyed -= HandleDestroyed;
            pedIntelligenceState.Complete();
        }
    }

    private void HandlePedStuck(LuaEvent luaEvent)
    {
        var elementId = luaEvent.Parameters.First().ElementId;
        var ped = (Ped)_elementCollection.Get(elementId.Value);
        if(_pedIntelligenceStates.TryRemove(ped, out var pedIntelligenceState))
        {
            pedIntelligenceState.Stop(new PedStuckException(ped));
        }
    }

    private void HandlePlayerJoin(Player player)
    {
        this._resource.StartFor(player);
    }

    private void HandlePedObstacleAvoidanceStrategies(Ped ped, ObstacleAvoidanceStrategies obstacleAvoidanceStrategies)
    {
        _pedObstacleAvoidanceStrategies.TryAdd(ped, obstacleAvoidanceStrategies);
    }

    private IPedIntelligenceState HandlePedTasks(Ped ped, IEnumerable<PedTask> tasks)
    {
        if (ped.Syncer == null)
            throw new Exception("Ped has no syncer");

        var taskState = new PedIntelligenceState(ped, tasks);
        if (!_pedIntelligenceStates.TryAdd(ped, taskState))
            throw new Exception("Ped has already running tasks");
        ped.Destroyed += HandleDestroyed;
        taskState.Stopped += HandleStopped;
        _pedObstacleAvoidanceStrategies.TryGetValue(ped, out var pedObstacleAvoidanceStrategies);
        ped.Syncer.TriggerLuaEvent("onPedTasks", ped.Syncer, ped, (int)pedObstacleAvoidanceStrategies, taskState.Id.ToString(), tasks.Select(x => x.ToLuaValue()).ToArray());
        return taskState;
    }

    private void HandleDestroyed(Element element)
    {
        var ped = (Ped)element;
        _pedIntelligenceStates.TryRemove(ped, out var _);
        _pedObstacleAvoidanceStrategies.TryRemove(ped, out var _);
        ped.Destroyed -= HandleDestroyed;
    }

    private void HandleStopped(IPedIntelligenceState pedIntelligenceState, Exception? ex)
    {
        pedIntelligenceState.Ped.Syncer.TriggerLuaEvent("stopPedTasks", pedIntelligenceState.Ped);
    }
}
