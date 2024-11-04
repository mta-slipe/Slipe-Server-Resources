using SlipeServer.Resources.Base;
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

public sealed class PedIntelligenceOptions : ResourceOptionsBase;

internal sealed class PedIntelligenceLogic : ResourceLogicBase<PedIntelligenceResource,  PedIntelligenceOptions>
{
    private readonly PedIntelligenceService pedIntelligenceService;
    private readonly LuaEventService luaEventService;
    private readonly IElementCollection elementCollection;
    private readonly ConcurrentDictionary<Ped, IPedIntelligenceState> pedIntelligenceStates = new();
    private readonly ConcurrentDictionary<Ped, ObstacleAvoidanceStrategies> pedObstacleAvoidanceStrategies = new();

    public PedIntelligenceLogic(MtaServer server, PedIntelligenceService pedIntelligenceService, LuaEventService luaEventService, IElementCollection elementCollection) : base(server)
    {
        this.pedIntelligenceService = pedIntelligenceService;
        this.luaEventService = luaEventService;
        this.elementCollection = elementCollection;

        this.pedIntelligenceService.RelayPedTasks = HandlePedTasks;
        this.pedIntelligenceService.RelayPedObstacleAvoidanceStrategies = HandlePedObstacleAvoidanceStrategies;

        luaEventService.AddEventHandler("pedFinishedTask", HandlePedFinishedTask);
        luaEventService.AddEventHandler("pedStuck", HandlePedStuck);
    }

    private void HandlePedFinishedTask(LuaEvent luaEvent)
    {
        var elementId = luaEvent.Parameters.First().ElementId;
        var ped = (Ped)elementCollection.Get(elementId.Value);
        if (pedIntelligenceStates[ped].AdvanceToNextTask() && this.pedIntelligenceStates.TryRemove(ped, out var pedIntelligenceState))
        {
            ped.Destroyed -= HandleDestroyed;
            pedIntelligenceState.Complete();
        }
    }

    private void HandlePedStuck(LuaEvent luaEvent)
    {
        var elementId = luaEvent.Parameters.First().ElementId;
        var ped = (Ped)elementCollection.Get(elementId.Value);
        if(pedIntelligenceStates.TryRemove(ped, out var pedIntelligenceState))
        {
            pedIntelligenceState.Stop(new PedStuckException(ped));
        }
    }

    private void HandlePedObstacleAvoidanceStrategies(Ped ped, ObstacleAvoidanceStrategies obstacleAvoidanceStrategies)
    {
        pedObstacleAvoidanceStrategies.TryAdd(ped, obstacleAvoidanceStrategies);
    }

    private IPedIntelligenceState HandlePedTasks(Ped ped, IEnumerable<PedTask> tasks)
    {
        if (ped.Syncer == null)
            throw new Exception("Ped has no syncer");

        var taskState = new PedIntelligenceState(ped, tasks);
        if (!pedIntelligenceStates.TryAdd(ped, taskState))
            throw new Exception("Ped has already running tasks");
        ped.Destroyed += HandleDestroyed;
        taskState.Stopped += HandleStopped;
        pedObstacleAvoidanceStrategies.TryGetValue(ped, out var pedObstacleAvoidanceStrategy);
        ped.Syncer.TriggerLuaEvent("onPedTasks", ped.Syncer, ped, (int)pedObstacleAvoidanceStrategy, taskState.Id.ToString(), tasks.Select(x => x.ToLuaValue()).ToArray());
        return taskState;
    }

    private void HandleDestroyed(Element element)
    {
        var ped = (Ped)element;
        pedIntelligenceStates.TryRemove(ped, out var _);
        pedObstacleAvoidanceStrategies.TryRemove(ped, out var _);
        ped.Destroyed -= HandleDestroyed;
    }

    private void HandleStopped(IPedIntelligenceState pedIntelligenceState, Exception? ex)
    {
        pedIntelligenceState.Ped.Syncer.TriggerLuaEvent("stopPedTasks", pedIntelligenceState.Ped);
    }
}
